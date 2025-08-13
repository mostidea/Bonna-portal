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

      var filteredList = new List<object>(); // ✅ EKLENDİ

      foreach (var order in result.data) // ✅ EKLENDİ
      {
        // ✅ Sipariş detaylarını almak için Items servisine istek
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

        decimal acikMiktarToplam = 0;
        decimal rezervMiktarToplam = 0;
        decimal toplamadaMiktarToplam = 0;
        decimal sevkMiktarToplam = 0;
        if (itemsResponse.IsSuccessStatusCode)
        {
          var itemsResult = JsonConvert.DeserializeObject<GetOrderItemsResponseDto>(itemsResponseBody);
          acikMiktarToplam = itemsResult.data.Select(i => decimal.TryParse(i.ACIKMIKTAR, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : 0).Sum();
          rezervMiktarToplam = itemsResult.data.Select(i => decimal.TryParse(i.REZERVEMIKTAR, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : 0).Sum();
          toplamadaMiktarToplam = itemsResult.data.Select(i => decimal.TryParse(i.TOPLAMADA, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : 0).Sum();
          sevkMiktarToplam = itemsResult.data.Select(i => decimal.TryParse(i.SEVKMIKTAR, NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : 0).Sum();
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
          acikMiktarToplam,
          rezervMiktarToplam,
          toplamadaMiktarToplam,
          sevkMiktarToplam
        });
      }

      return Ok(filteredList); // ✅ foreach sonrası dönülüyor
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

  }
}
