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
  public class HomeController : ControllerBase
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _bonnaApiBaseUrl;
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;

    public HomeController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
    {
      _httpClientFactory = httpClientFactory;
      _bonnaApiBaseUrl = configuration["ExternalServices:BonnaApiBaseUrl"];
      _memoryCache = memoryCache;
    }

    [HttpPost("GetAllCounts")]
    public async Task<IActionResult> GetAllCounts([FromQuery] string userId, [FromBody] PriceListRequestDto stockRequest)
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

      // Orders
      var ordersRequest = new
      {
        language = "T",
        info = new { KPOCUSTOMER = kpocustomer }
      };
      var ordersContent = new StringContent(JsonConvert.SerializeObject(ordersRequest), Encoding.UTF8, "application/json");
      var ordersResponse = await client.PostAsync($"{_bonnaApiBaseUrl}/api/orderERP", ordersContent);
      var ordersCount = 0;
      if (ordersResponse.IsSuccessStatusCode)
      {
        var ordersResult = JsonConvert.DeserializeObject<GetOrderListResponse>(await ordersResponse.Content.ReadAsStringAsync());
        ordersCount = ordersResult?.data?.Count ?? 0;
      }

      // Invoices
      var invoiceRequest = new
      {
        info = new
        {
          //userData = new { KPOCUSTOMER = kpocustomer },
          userData = new { KPOCUSTOMER = "M00000653" },
          search = new { CUSTOMER = "" },
          documentType = "0",
          eNumber = "",
          startDate = "2000-05-05",
          endDate = "2425-05-08",
          minPrice = "",
          maxPrice = "",
          product = "",
          status = "",
          sNumber = "",
          overdueProducts = "",
          sampleOrders = ""
        }
      };
      var invoiceContent = new StringContent(JsonConvert.SerializeObject(invoiceRequest), Encoding.UTF8, "application/json");
      var invoiceResponse = await client.PostAsync($"{_bonnaApiBaseUrl}/api/invoiceERP", invoiceContent);
      var invoicesCount = 0;
      if (invoiceResponse.IsSuccessStatusCode)
      {
        var invoiceResult = JsonConvert.DeserializeObject<InvoiceResponseDto>(await invoiceResponse.Content.ReadAsStringAsync());
        invoicesCount = invoiceResult?.data?.Count ?? 0;
      }

      // Stocks
      var stockContent = new StringContent(JsonConvert.SerializeObject(stockRequest), Encoding.UTF8, "application/json");
      var stockResponse = await client.PostAsync($"{_bonnaApiBaseUrl}/api/pricelist", stockContent);
      var stocksCount = 0;
      if (stockResponse.IsSuccessStatusCode)
      {
        var stockResult = JsonConvert.DeserializeObject<StockListResponse>(await stockResponse.Content.ReadAsStringAsync());
        stocksCount = stockResult?.totalItems ?? 0;
      }

      return Ok(new
      {
        ordersCount,
        invoicesCount,
        stocksCount
      });
    }


  }
}
