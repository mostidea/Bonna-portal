using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;

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

    [HttpGet("GetMyProfile")]
    public IActionResult GetMyProfile([FromQuery] string userId)
    {
      // Token kontrolü
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var cacheKey = $"login_{userId}";
      if (!_memoryCache.TryGetValue(cacheKey, out dynamic cachedData))
        return Unauthorized("Cache'de kullanıcı oturumu bulunamadı.");

      if (cachedData.User == null || cachedData.User.Token == null || !string.Equals(token, cachedData.User.Token.ToString(), StringComparison.Ordinal))
        return Unauthorized("Token doğrulanamadı.");

      var user = cachedData.User;
      var profile = new
      {
        adsoyad = user.Namesurname,
        email = user.Email,
        username = user.Username
      };

      return Ok(profile);
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
