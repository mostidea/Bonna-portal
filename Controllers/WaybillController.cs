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
       
  }
}
