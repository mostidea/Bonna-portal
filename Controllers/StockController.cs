using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bonna_Portal_Bridge_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StockController : ControllerBase
  {
    private readonly IHttpClientFactory _httpClientFactory;

    public StockController(IHttpClientFactory httpClientFactory)
    {
      _httpClientFactory = httpClientFactory;
    }

    public class PriceListRequestDto
    {
      public string PRICELIST { get; set; }
    }

    [HttpPost("StockAll")]
    public async Task<IActionResult> StockAll([FromBody] PriceListRequestDto request)
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var client = _httpClientFactory.CreateClient();
      client.BaseAddress = new Uri("https://api-portal.bonna.com.tr/");
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var json = JsonConvert.SerializeObject(request);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await client.PostAsync("api/pricelist/", content);

      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      return Content(responseBody, "application/json");
    }

  }
}
