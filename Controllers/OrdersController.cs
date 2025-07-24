using System.Net.Http.Headers;
using System.Text;
using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly string _bonnaApiBaseUrl;

  public OrdersController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
  {
    _httpClientFactory = httpClientFactory;
    _bonnaApiBaseUrl = configuration["ExternalServices:BonnaApiBaseUrl"];
  }

  [HttpPost("GetOrderList")]
  public async Task<IActionResult> GetOrderList()
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
        KPOCUSTOMER = "M00000653"
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
    return Ok(result);
  }

}
