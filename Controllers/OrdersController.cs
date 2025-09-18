using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;

namespace Bonna_Portal_Bridge_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _bonnaApiBaseUrl;
    private readonly IMemoryCache _memoryCache;

    public OrdersController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
    {
      _httpClientFactory = httpClientFactory;
      _bonnaApiBaseUrl = configuration["ExternalServices:BonnaApiBaseUrl"];
      _memoryCache = memoryCache;
    }

    [HttpPost("List")]
    public async Task<IActionResult> List([FromQuery] string userId)
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var cacheKey = $"login_{userId}";
      if (!_memoryCache.TryGetValue(cacheKey, out dynamic cachedData))
        return Unauthorized("Cache'de kullanıcı oturumu bulunamadı.");

      var kpocustomer = cachedData.ErpData[0]?.KPOCUSTOMER?.ToString();
      if (string.IsNullOrEmpty(kpocustomer))
        return BadRequest("KPOCUSTOMER bilgisi bulunamadı.");

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var requestBody = new
      {
        language = "T",
        info = new
        {
          KPOCUSTOMER = kpocustomer,
          newOfferOrderInfo = new
          {
            docType = "",
            quality = "",
            type = ""
          }
        }
      };

      var json = JsonConvert.SerializeObject(requestBody);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var url = $"{_bonnaApiBaseUrl}/api/orderERP";
      var response = await client.PostAsync(url, content);
      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      var result = JsonConvert.DeserializeObject<GetOrderListResponse>(responseBody);
      var filteredList = new List<object>();

      foreach (var order in result.data)
      {
        var itemsRequestBody = new
        {
          language = "T",
          info = new
          {
            type = "orderItems",
            orderData = new
            {
              DOCTYPE = order.DOCTYPE,
              DOCNUM = order.DOCNUM
            }
          }
        };

        var itemsJson = JsonConvert.SerializeObject(itemsRequestBody);
        var itemsContent = new StringContent(itemsJson, Encoding.UTF8, "application/json");
        var itemsUrl = $"{_bonnaApiBaseUrl}/api/oitemsERP";
        var itemsResponse = await client.PostAsync(itemsUrl, itemsContent);
        var itemsResponseBody = await itemsResponse.Content.ReadAsStringAsync();

        float acikMiktarToplam = 0;
        float rezervMiktarToplam = 0;
        float toplamadaMiktarToplam = 0;
        float sevkMiktarToplam = 0;
        float miktarToplam = 0;

        if (itemsResponse.IsSuccessStatusCode)
        {
          var itemsResult = JsonConvert.DeserializeObject<GetOrderItemsResponseDto>(itemsResponseBody);
          acikMiktarToplam = itemsResult.data.Select(i => float.TryParse(i.ACIKMIKTAR, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : 0).Sum();
          rezervMiktarToplam = itemsResult.data.Select(i => float.TryParse(i.REZERVEMIKTAR, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : 0).Sum();
          toplamadaMiktarToplam = itemsResult.data.Select(i => float.TryParse(i.TOPLAMADA, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : 0).Sum();
          sevkMiktarToplam = itemsResult.data.Select(i => float.TryParse(i.SEVKMIKTAR, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : 0).Sum();
          miktarToplam = itemsResult.data.Select(i => float.TryParse(i.MIKTAR, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : 0).Sum();
        }

        filteredList.Add(new
        {
          sevktarihi = order.FABRIKASEVKTARIHI,
          indirim1 = order.INDIRIM1,
          indirim2 = order.INDIRIM2,
          indirim3 = order.INDIRIM3,
          kdvsiztoplam = order.GENELTOPLAM - order.KDVTUTARI,
          kdvtutari = order.KDVTUTARI,
          geneltoplam = order.GENELTOPLAM,
          faturatoplam = order.FATURATOPLAMI,
          toplamindirim = order.TOPLAMINDIRIM,
          odemetipi = order.ODEMETIPI,
          fiyatlistesi = order.FIYATLISTESI,
          kalite = order.KALITE,
          belgesahip = order.BELGESAHIP,
          bayi = order.BAYI,
          adres = order.ADRES,
          yorum = order.YORUM,
          doctype = order.DOCTYPE,
          docnum = order.DOCNUM,
          belgeno = order.BELGENO,
          musteri = order.MUSTERI,
          tarih = DateTime.TryParse(order.TARIH, out var parsedDate) ? parsedDate.ToString("dd.MM.yyyy") : order.TARIH,
          durum = order.DURUM,
          belgetip = order.BELGETIP,
          acikMiktarToplam = Math.Round(acikMiktarToplam, 1),
          rezervMiktarToplam = Math.Round(rezervMiktarToplam, 1),
          toplamadaMiktarToplam = Math.Round(toplamadaMiktarToplam, 1),
          sevkMiktarToplam = Math.Round(sevkMiktarToplam, 1),
          miktarToplam = Math.Round(miktarToplam, 1)
        });
      }
      return Ok(filteredList);
    }

    [HttpPost("Items")]
    public async Task<IActionResult> Items([FromBody] GetOrderItemsRequestDto dto)
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var requestBody = new
      {
        language = "T",
        info = new
        {
          type = "orderItems",
          orderData = new
          {
            DOCTYPE = dto.DOCTYPE,
            DOCNUM = dto.DOCNUM
          }
        }
      };

      var json = JsonConvert.SerializeObject(requestBody);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var url = $"{_bonnaApiBaseUrl}/api/oitemsERP";
      var response = await client.PostAsync(url, content);
      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      //var cacheKey = $"login_{dto.Userid}";
      //if (!_memoryCache.TryGetValue(cacheKey, out dynamic cachedData))
      //return Unauthorized("Cache'de kullanıcı oturumu bulunamadı.");

      // 💡 ErpData bir liste, ilk öğeden KPOCUSTOMER alınmalı
      //var kpocustomer = cachedData.ErpData[0]?.KPOCUSTOMER?.ToString();

      var result = JsonConvert.DeserializeObject<GetOrderItemsResponseDto>(responseBody);
      var filteredItems = result.data.Select(item => new
      {
        item.MALZEMEACIKLAMA,
        item.MALZEMEKODU,
        item.BIRIMFIYAT,
        item.TOPLAMFIYAT,
        item.MIKTAR,
        fiyatlistesi = "U2"
      }).ToList();

      return Ok(filteredItems);
    }

    [HttpPost("ExportPdf")]
    public IActionResult ExportPdf([FromBody] ExportPdfRequestDto dto)
    {
      // Yetki kontrolü örneği
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      // 1. PDF içeriğini oluştur (örnek: teklif/sipariş detaylarını ekle)
      var sb = new StringBuilder();
      sb.AppendLine($"Dil: {dto.lang}");
      sb.AppendLine($"Kullanıcı: {JsonConvert.SerializeObject(dto.userInfo)}");
      sb.AppendLine($"Tür: {dto.info.type}");

      if (dto.info.type == "offerItems" && dto.info.offerData != null)
      {
        sb.AppendLine("Teklif Bilgileri:");
        sb.AppendLine(JsonConvert.SerializeObject(dto.info.offerData));
      }
      if (dto.info.type == "orderItems" && dto.info.orderData != null)
      {
        sb.AppendLine("Sipariş Bilgileri:");
        sb.AppendLine(JsonConvert.SerializeObject(dto.info.orderData));
      }

      sb.AppendLine("Kalemler:");
      foreach (var item in dto.offer_order_ItemsData)
      {
        sb.AppendLine($"Malzeme: {item.MALZEMEACIKLAMA}, Kod: {item.MALZEMEKODU}, Miktar: {item.MIKTAR}, Fiyat: {item.TOPLAMFIYAT}");
      }

      var contentBytes = Encoding.UTF8.GetBytes(sb.ToString());

      var base64 = Convert.ToBase64String(contentBytes);

      return Ok(new
      {
        success = true,
        fileBase64 = base64,
        fileName = "export.pdf" // Gerçek PDF olursa uzantı pdf olmalı
      });
    }

    public class ExportPdfRequestDto
    {
      public InfoDto info { get; set; }
      public List<OrderItemDto> offer_order_ItemsData { get; set; }
      public object userInfo { get; set; }
      public string lang { get; set; }
    }

    public class InfoDto
    {
      public string type { get; set; } // "offerItems" veya "orderItems"
      public object offerData { get; set; }
      public object orderData { get; set; }
    }

  }
}
