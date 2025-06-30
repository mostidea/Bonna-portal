using System.Text;
using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bonna_Portal_Bridge_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IHttpClientFactory _HttpClientFactory;
    private readonly string _BonnaApiBaseUrl;

    public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
      _HttpClientFactory = httpClientFactory;
      _BonnaApiBaseUrl = configuration["ExternalServices:BonnaApiBaseUrl"];
    }

    

    [HttpPost("LoginToBonna")]
    public async Task<IActionResult> LoginToBonna([FromQuery] LoginRequestDto model)
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

      return Ok(result);
    }


  }
}
