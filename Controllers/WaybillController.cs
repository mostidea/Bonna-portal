using Bonna_Portal_Bridge_Api.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Bonna_Portal_Bridge_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class WaybillController : ControllerBase
  {
    private readonly IHttpClientFactory _httpClientFactory;

    public WaybillController(IHttpClientFactory httpClientFactory)
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
        var response = await client.GetAsync("api/waybill/");
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


  }
}
