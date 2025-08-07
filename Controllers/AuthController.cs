using System.Text;
using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Bonna_Portal_Bridge_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IHttpClientFactory _HttpClientFactory;
    private readonly string _BonnaApiBaseUrl;
    private readonly IMemoryCache _memoryCache;

    public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
    {
      _HttpClientFactory = httpClientFactory;
      _BonnaApiBaseUrl = configuration["ExternalServices:BonnaApiBaseUrl"];
      _memoryCache = memoryCache;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromQuery] LoginRequestDto model)
    {
      var client = _HttpClientFactory.CreateClient();
      var requestJson = JsonConvert.SerializeObject(new { username = model.Username, password = model.Password });
      var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

      var response = await client.PostAsync($"{_BonnaApiBaseUrl}/api/auth/login", content);
      if (!response.IsSuccessStatusCode)
      {
        return StatusCode((int)response.StatusCode, "Bonna API erişim hatası.");
      }

      var responseString = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<LoginResponseDto>(responseString);
      if (result == null || result.Error)
      {
        return BadRequest("Giriş başarısız.");
      }

      var cacheKey = $"login_{result.User._id}";
      _memoryCache.Set(cacheKey, new { User = result.User, ErpData = result.ErpData, RolesData = result.RolesData }, TimeSpan.FromDays(365));
      return Ok(new { token = result.Token, userId = result.User._id });
      //return Ok(result);
    }

    [HttpGet("GetCachedLoginData/{userId}")]
    public IActionResult GetCachedLoginData(string userId)
    {
      var cacheKey = $"login_{userId}";
      if (_memoryCache.TryGetValue(cacheKey, out var cachedData))
      {
        return Ok(cachedData);
      }
      return NotFound("Cache'de veri bulunamadı.");
    }

    //[HttpPost("Login")]
    //public async Task<IActionResult> Login([FromQuery] LoginRequestDto model)
    //{
    //  var client = _HttpClientFactory.CreateClient();
    //  var requestJson = JsonConvert.SerializeObject(new { username = model.Username, password = model.Password });
    //  var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

    //  var response = await client.PostAsync($"{_BonnaApiBaseUrl}/api/auth/login", content);
    //  if (!response.IsSuccessStatusCode)
    //  {
    //    return StatusCode((int)response.StatusCode, "Bonna API erişim hatası.");
    //  }

    //  var responseString = await response.Content.ReadAsStringAsync();

    //  var result = JsonConvert.DeserializeObject<LoginResponseDto>(responseString);
    //  if (result == null || result.Error)
    //  {
    //    return BadRequest("Giriş başarısız.");
    //  }

    //  return Ok(result);
    //}

  }
}
