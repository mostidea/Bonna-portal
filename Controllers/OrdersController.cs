using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
  private readonly IHttpClientFactory _httpClientFactory;

  public OrdersController(IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory;
  }

}
