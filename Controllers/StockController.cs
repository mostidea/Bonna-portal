using System.Net.Http.Headers;
using System.Text;
using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bonna_Portal_Bridge_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StockController : ControllerBase
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _bonnaApiBaseUrl;

    public StockController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
      _httpClientFactory = httpClientFactory;
      _bonnaApiBaseUrl = configuration["ExternalServices:BonnaApiBaseUrl"];
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
    public async Task<IActionResult> Search([FromQuery] string search, [FromQuery] string KPOISCUSTOMER)
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

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
