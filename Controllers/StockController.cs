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
    public async Task<IActionResult> List([FromBody] PriceListRequestDto request)
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var json = JsonConvert.SerializeObject(request);
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var response = await client.PostAsync($"{_bonnaApiBaseUrl}/api/pricelist", content);
      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      return Content(responseBody, "application/json");
    }

    [HttpPost("Search")]
    public async Task<IActionResult> Search([FromQuery] string search, [FromQuery] string KPOISCUSTOMER, string userId)
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var cacheKey = $"login_{userId}";
      if (!_memoryCache.TryGetValue(cacheKey, out dynamic cachedData))
        return Unauthorized("Cache'de kullanıcı oturumu bulunamadı.");

      // 💡 ErpData bir liste, ilk öğeden KPOCUSTOMER alınmalı
      var kpocustomer = cachedData.ErpData[0]?.KPOCUSTOMER?.ToString();
      if (string.IsNullOrEmpty(kpocustomer))
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
        KPOISCUSTOMER = "0000001"
      };

      var json = JsonConvert.SerializeObject(sabitBody);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var url = $"{_bonnaApiBaseUrl}/api/pricelist/search?search={search}&KPOISCUSTOMER={KPOISCUSTOMER}";
      var response = await client.PostAsync(url, content);
      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      return Content(responseBody, "application/json");
    }

  }
}
