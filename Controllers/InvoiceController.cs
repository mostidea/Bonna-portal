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

  }
}
