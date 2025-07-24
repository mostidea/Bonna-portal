using System.Net.Http.Headers;
using System.Text;
using Bonna_Portal_Bridge_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bonna_Portal_Bridge_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class InvoiceController : ControllerBase
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public InvoiceController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
      _httpClientFactory = httpClientFactory;
      _configuration = configuration;
    }

    [HttpPost("GetInvoices")]
    public async Task<IActionResult> GetInvoices()
    {
      if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        return Unauthorized("Authorization header eksik.");

      var token = authorizationHeader.ToString().Replace("Bearer ", "").Replace("bearer ", "", StringComparison.OrdinalIgnoreCase);
      if (string.IsNullOrEmpty(token))
        return Unauthorized("Token geçersiz.");

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var requestBody = new
      {
        info = new
        {
          userData = new
          {
            KPOCUSTOMER = "M00000653"
          },
          search = new
          {
            CUSTOMER = ""
          },
          documentType = "0",
          eNumber = "",
          startDate = "2025-05-05",
          endDate = "2025-05-08",
          minPrice = "",
          maxPrice = "",
          product = "",
          status = "",
          sNumber = "",
          overdueProducts = "",
          sampleOrders = ""
        }
      };

      var json = JsonConvert.SerializeObject(requestBody);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var url = $"{_configuration["ExternalServices:BonnaApiBaseUrl"]}/api/invoiceERP";
      var response = await client.PostAsync(url, content);
      var responseBody = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<InvoiceResponseDto>(responseBody);

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, responseBody);

      return Content(responseBody, "application/json");
    }

  }
}
