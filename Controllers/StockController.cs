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
  public class StockController : ControllerBase
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _bonnaApiBaseUrl;
    private readonly IMemoryCache _memoryCache;

    public StockController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
    {
      _httpClientFactory = httpClientFactory;
      _bonnaApiBaseUrl = configuration["ExternalServices:BonnaApiBaseUrl"];
      _memoryCache = memoryCache;
    }

    [HttpPost("List")]
    public async Task<IActionResult> List()
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var request = new
      {
        PRICELIST = "U2",
        newOfferOrderInfo = new
        {
          docType = "",
          quality = "",
          type = ""
        }
      };

      var json = JsonConvert.SerializeObject(request);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var response = await client.PostAsync($"{_bonnaApiBaseUrl}/api/pricelist", content);
      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      var rawResponse = JsonConvert.DeserializeObject<StockListResponse>(responseBody);

      var result = new
      {
        error = rawResponse.error,
        currentPage = rawResponse.currentPage,
        totalPages = rawResponse.totalPages,
        totalItems = rawResponse.totalItems,
        data_length = rawResponse.data_length,
        result = rawResponse.result.Select(x => new
        {
          estext = x.ESTEXT,
          material = x.MATERIAL,
          eankod = x.EANKOD,
          isunsrail = x.ISUNSRAIL,
          paletadet = x.PALETADET,
          availqtY1 = x.AVAILQTY1,
          availstocK1 = x.AVAILSTOCK1,
          availqtY2 = x.AVAILQTY2,
          availstocK2 = x.AVAILSTOCK2,
          pricelist = x.priceListDetail?.PRICELIST,
          price = x.priceListDetail?.PRICE,
          currency = x.priceListDetail?.CURRENCY,
          validfrom = x.priceListDetail?.VALIDFROM,
          validuntil = x.priceListDetail?.VALIDUNTIL,
          pcs = x.PCS
        }).ToList()
      };

      return Ok(result);
    }

    [HttpPost("Search")]
    public async Task<IActionResult> Search([FromQuery] string search, string userId)
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var cacheKey = $"login_{userId}";
      if (!_memoryCache.TryGetValue(cacheKey, out dynamic cachedData))
        return Unauthorized("Cache'de kullanıcı oturumu bulunamadı.");

      var _KPOISCUSTOMER = cachedData.ErpData[0]?.KPOISCUSTOMER?.ToString();
      if (string.IsNullOrEmpty(_KPOISCUSTOMER))
        return BadRequest("KPOCUSTOMER bilgisi bulunamadı.");

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var sabitBody = new
      {
        PRICELIST = "U2",
        productSearch = new
        {
          materialName = "materialName",
          price = "",
          priceOperator = "",
          stock = "",
          stockOperator = "",
          reservation = "",
          reservationOperator = "",
          productType = ""
        },
        status = "boolean",
        //KPOISCUSTOMER = "0000001"
        KPOISCUSTOMER = _KPOISCUSTOMER
      };

      var json = JsonConvert.SerializeObject(sabitBody);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var url = $"{_bonnaApiBaseUrl}/api/pricelist/search?search={search}&KPOISCUSTOMER={_KPOISCUSTOMER}";
      var response = await client.PostAsync(url, content);
      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      return Content(responseBody, "application/json");
    }

  }
}
