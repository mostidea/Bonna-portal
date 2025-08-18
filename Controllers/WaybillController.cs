using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Bonna_Portal_Bridge_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class WaybillController : ControllerBase
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    public WaybillController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
    {
      _httpClientFactory = httpClientFactory;
      _configuration = configuration;
      _memoryCache = memoryCache;
    }

    [HttpPost("List")]
    public async Task<IActionResult> List([FromQuery] string userId)
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var cacheKey = $"login_{userId}";
      if (!_memoryCache.TryGetValue(cacheKey, out dynamic cachedData))
        return Unauthorized("Cache'de kullanıcı oturumu bulunamadı.");
      var kpocustomer = cachedData.ErpData[0]?.KPOCUSTOMER?.ToString();

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var requestBody = new
      {
        info = new
        {
          userData = new
          {
            KPOCUSTOMER = "M00000653"
            //KPOCUSTOMER = kpocustomer
          },
          search = new
          {
            CUSTOMER = ""
          },
          documentType = "0",
          eNumber = "",
          startDate = "2025-05-05",
          endDate = "2025-05-08",
          minPrice = "",
          maxPrice = "",
          product = "",
          status = "",
          sNumber = "",
          overdueProducts = "",
          sampleOrders = ""
        }
      };

      var json = JsonConvert.SerializeObject(requestBody);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var url = $"{_configuration["ExternalServices:BonnaApiBaseUrl"]}/api/invoiceERP";
      var response = await client.PostAsync(url, content);
      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      var result = JsonConvert.DeserializeObject<InvoiceResponseDto>(responseBody);

      var filteredData = result.data.Select(x => new
      {
        fiyatlistesi = x.FIYATLISTESI,
        solusturmatarihi = x.SOLUSTURMATARIHI,
        sevkyeri = x.SEVKYERI,
        musteriad = x.MUSTERIADI,
        tarih = x.SSEVKTARIHI,
        faturano = x.DOCNUM,
        PRINTEDNUM = x.PRINTEDNUM,
        sevkdepo = x.SEVKEDILENDEPO
      });

      return Ok(filteredData);
    }

    [HttpGet("Detail")]
    public async Task<IActionResult> Detail([FromQuery] string userId, [FromQuery] string faturano)
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var cacheKey = $"login_{userId}";
      if (!_memoryCache.TryGetValue(cacheKey, out dynamic cachedData))
        return Unauthorized("Cache'de kullanıcı oturumu bulunamadı.");
      var kpocustomer = cachedData.ErpData[0]?.KPOCUSTOMER?.ToString();

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var requestBody = new
      {
        info = new
        {
          userData = new
          {
            KPOCUSTOMER = "M00000653"
            //KPOCUSTOMER = kpocustomer
          },
          search = new
          {
            CUSTOMER = ""
          },
          documentType = "0",
          eNumber = "",
          startDate = "2025-05-05",
          endDate = "2025-05-08",
          minPrice = "",
          maxPrice = "",
          product = "",
          status = "",
          sNumber = "",
          overdueProducts = "",
          sampleOrders = ""
        }
      };

      var json = JsonConvert.SerializeObject(requestBody);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var url = $"{_configuration["ExternalServices:BonnaApiBaseUrl"]}/api/invoiceERP";
      var response = await client.PostAsync(url, content);
      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      var result = JsonConvert.DeserializeObject<InvoiceResponseDto>(responseBody);

      var invoice = result.data.FirstOrDefault(x => x.DOCNUM == faturano);
      if (invoice == null)
        return NotFound("Fatura bulunamadı.");

      var items = new List<object>();
      foreach (var item in invoice.SALITEMS)
      {
        var itemObj = item as Newtonsoft.Json.Linq.JObject;
        items.Add(new
        {
          sevkmiktar = itemObj?["SEVKMIKTAR"]?.ToString(),
          urunKodu = itemObj?["URUN"]?.ToString(),
          urunAdi = itemObj?["URUNACIKLAMASI"]?.ToString(),
          siparisNo = itemObj?["REFDOCNUM"]?.ToString(),
          birimFiyat = itemObj?["BIRIMFIYAT"]?.ToString(),
          toplamFiyat = itemObj?["TOTALFIYAT"]?.ToString(),
          miktar = itemObj?["QUANTITY"]?.ToString(),
          birim = itemObj?["BIRIM"]?.ToString(),
          paraBirimi = itemObj?["DCURRENCY"]?.ToString(),
          voptions = itemObj?["VOPTIONS"]?.ToString()
        });
      }

      return Ok(items);
    }

  }
}
