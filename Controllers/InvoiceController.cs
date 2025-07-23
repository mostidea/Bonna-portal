using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Bonna_Portal_Bridge_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class InvoiceController : ControllerBase
  {
    private readonly IHttpClientFactory _httpClientFactory;

    public InvoiceController(IHttpClientFactory httpClientFactory)
    {
      _httpClientFactory = httpClientFactory;
    }

    [HttpPost("GetInvoice")]
    public async Task<IActionResult> GetInvoiceAsync([FromBody] InvoiceRequestModel request)
    {
      if (string.IsNullOrWhiteSpace(request.Token))
        return BadRequest("Token gereklidir.");

      var payload = new
      {
        info = new
        {
          userData = request.UserData,
          documentType = request.DocumentType,
          eNumber = request.ENumber,
          startDate = request.StartDate,
          endDate = request.EndDate,
          minPrice = request.MinPrice,
          maxPrice = request.MaxPrice,
          product = request.Product,
          status = request.Status
        }
      };

      var client = _httpClientFactory.CreateClient();
      client.BaseAddress = new Uri("https://api-portal.bonna.com.tr/");
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);

      var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await client.PostAsync("api/invoiceERP", content);
      var responseBody = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      return Content(responseBody, "application/json");
    }
  }

  public class InvoiceRequestModel
  {
    public string Token { get; set; } = null!;
    public UserDataModel UserData { get; set; } = null!;
    public string DocumentType { get; set; } = "0";
    public string ENumber { get; set; } = "";
    public string StartDate { get; set; } = null!;
    public string EndDate { get; set; } = null!;
    public string MinPrice { get; set; } = "";
    public string MaxPrice { get; set; } = "";
    public string Product { get; set; } = "";
    public string Status { get; set; } = "";
  }

  public class UserDataModel
  {
    public string CLIENT { get; set; }
    public string COMPANY { get; set; }
    public string USERNAME { get; set; }
    public string SALDEPT { get; set; }
    public string EMAIL { get; set; }
    public string KPOISCUSTOMER { get; set; }
    public string KPOISNATION { get; set; }
    public string KPOAUTHORITY { get; set; }
    public string BPHONE { get; set; }
    public string BMOBILE { get; set; }
    public string KPOCUSTOMER { get; set; }
    public string KPOCURRENCY { get; set; }
    public string CCUSTNAME { get; set; }
    public string KPOCUSADRNUM { get; set; }
    public string KPOCOMMISSIONER { get; set; }
    public string KPOCOMADRNUM { get; set; }
    public object[] TMPLOGINDETAILCUS { get; set; }
    public object[] TMPCUSADR { get; set; }
  }

}
