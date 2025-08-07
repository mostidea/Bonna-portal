namespace Bonna_Portal_Bridge_Api.Models
{
  public class PriceListResponseDto
  {
    public List<object> data { get; set; } = new(); // detaylı tip varsa StockDto vs. yaz
  }

  public class PriceListRequestDto
  {
    public string PRICELIST { get; set; }
  }
  public class StockListResponse
  {
    public bool error { get; set; }
    public int currentPage { get; set; }
    public int totalPages { get; set; }
    public int totalItems { get; set; }
    public int data_length { get; set; }
    public List<object> result { get; set; } = new();
  }
}