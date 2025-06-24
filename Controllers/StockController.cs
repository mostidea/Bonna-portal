using System.Net.Http.Headers;
using System.Text.Json;
using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("GetStocks")]
    public async Task<ActionResult<StockResponseDto>> GetStocks([FromHeader(Name = "Authorization")] string authorizationHeader, [FromQuery] int page = 1, [FromQuery] int limit = 10000) // varsayılan limit yüksek ama isteyen daha küçük de gönderebilir
    {
      if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        return BadRequest("Authorization header eksik veya geçersiz.");

      var token = authorizationHeader.Replace("Bearer ", "").Trim();

      var client = _httpClientFactory.CreateClient();

      client.BaseAddress = new Uri("https://api-portal.bonna.com.tr/");
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var response = await client.GetAsync($"api/stock?page={page}&limit={limit}");

      if (!response.IsSuccessStatusCode)
      {
        var errorContent = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, errorContent);
      }

      var jsonString = await response.Content.ReadAsStringAsync();

      var result = JsonSerializer.Deserialize<StockResponseDto>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

      return Ok(result);
    }

    //[HttpGet("GetStocks")]
    //public async Task<ActionResult<StockResponseDto>> GetStocks([FromHeader(Name = "Authorization")] string authorizationHeader)
    //{
    //  if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
    //    return BadRequest("Authorization header eksik veya geçersiz.");

    //  var token = authorizationHeader.Replace("Bearer ", "").Trim();

    //  // SAYFALAMA DEĞERLERİ SABİT (sadece burası sabit olacak)
    //  int page = 1;
    //  int limit = 5;

    //  var client = _httpClientFactory.CreateClient();

    //  client.BaseAddress = new Uri("https://api-portal.bonna.com.tr/");
    //  client.DefaultRequestHeaders.Accept.Clear();
    //  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //  client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //  var response = await client.GetAsync($"api/stock?page={page}&limit={limit}");

    //  if (!response.IsSuccessStatusCode)
    //  {
    //    var errorContent = await response.Content.ReadAsStringAsync();
    //    return StatusCode((int)response.StatusCode, errorContent);
    //  }

    //  var jsonString = await response.Content.ReadAsStringAsync();

    //  var result = JsonSerializer.Deserialize<StockResponseDto>(jsonString,
    //      new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //  return Ok(result);
    //}


    //[HttpGet("GetStocks")]
    //public async Task<ActionResult<StockResponseDto>> GetStocks([FromHeader(Name = "Authorization")] string authorizationHeader)
    //{
    //  if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
    //    return BadRequest("Authorization header eksik veya geçersiz.");

    //  var token = authorizationHeader.Replace("Bearer ", "").Trim();

    //  var client = _httpClientFactory.CreateClient();

    //  client.BaseAddress = new Uri("https://api-portal.bonna.com.tr/");
    //  client.DefaultRequestHeaders.Accept.Clear();
    //  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //  client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //  var response = await client.GetAsync("api/stock/");

    //  if (!response.IsSuccessStatusCode)
    //  {
    //    var errorContent = await response.Content.ReadAsStringAsync();
    //    return StatusCode((int)response.StatusCode, errorContent);
    //  }

    //  var jsonString = await response.Content.ReadAsStringAsync();

    //  var result = JsonSerializer.Deserialize<StockResponseDto>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //  return Ok(result);
    //}

    [HttpGet("GetStockDetail/{id}")]
    public async Task<ActionResult<StockDetailResponseDto>> GetStockDetail(string id, [FromHeader(Name = "Authorization")] string authorizationHeader)
    {
      if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        return BadRequest("Authorization header eksik veya geçersiz.");

      var token = authorizationHeader.Replace("Bearer ", "").Trim();

      var client = _httpClientFactory.CreateClient();

      client.BaseAddress = new Uri("https://api-portal.bonna.com.tr/");
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var response = await client.GetAsync($"api/stock/{id}");

      if (!response.IsSuccessStatusCode)
      {
        var errorContent = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, errorContent);
      }

      var jsonString = await response.Content.ReadAsStringAsync();

      var result = JsonSerializer.Deserialize<StockDetailResponseDto>(jsonString,
          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

      return Ok(result);
    }

  }
}
