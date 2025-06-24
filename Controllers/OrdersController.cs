using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
  private readonly IHttpClientFactory _httpClientFactory;

  public OrdersController(IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory;
  }

  [HttpGet("GetOrders")]
  public async Task<IActionResult> GetOrders([FromHeader(Name = "Authorization")] string authorizationHeader)
  {
    if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
      return BadRequest("Authorization header eksik veya geçersiz.");

    var token = authorizationHeader.Replace("Bearer ", "").Trim();

    var client = _httpClientFactory.CreateClient();
    client.BaseAddress = new Uri("https://api-portal.bonna.com.tr/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    try
    {
      var response = await client.GetAsync("api/orders/");
      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

      var json = await response.Content.ReadAsStringAsync();

      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      };

      var result = JsonSerializer.Deserialize<OrderApiResponse>(json, options);

      return Ok(result);
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Sunucu hatası: {ex.Message}");
    }
  }

  [HttpGet("GetOrderDetail/{orderId}")]
  public async Task<IActionResult> GetOrderDetail(string orderId, [FromHeader(Name = "Authorization")] string authorizationHeader)
  {
    if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
      return BadRequest("Authorization header eksik veya geçersiz.");

    var token = authorizationHeader.Replace("Bearer ", "").Trim();

    var client = _httpClientFactory.CreateClient();
    client.BaseAddress = new Uri("https://api-portal.bonna.com.tr/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    try
    {
      var response = await client.GetAsync($"api/orders/{orderId}");

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, "API isteği başarısız oldu.");

      var content = await response.Content.ReadAsStringAsync();
      var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
      var result = JsonSerializer.Deserialize<OrderDetailResponse>(content, options);

      return Ok(result);
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Hata oluştu: {ex.Message}");
    }
  }


}
