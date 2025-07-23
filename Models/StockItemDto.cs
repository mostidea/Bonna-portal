namespace Bonna_Portal_Bridge_Api.Models
{
  public class PriceListRequestDto
  {
    public string PRICELIST { get; set; }
  }

  public class PriceListSearchRequestDto
  {
    public string PRICELIST { get; set; } = null!;
    public ProductSearch productSearch { get; set; } = new();
    public string status { get; set; } = "boolean";
    public string KPOISCUSTOMER { get; set; } = null!;
  }

  public class ProductSearch
  {
    public string materialName { get; set; } = "materialName";
    public string price { get; set; } = "";
    public string priceOperator { get; set; } = "";
    public string stock { get; set; } = "";
    public string stockOperator { get; set; } = "";
    public string reservation { get; set; } = "";
    public string reservationOperator { get; set; } = "";
    public string productType { get; set; } = "";
  }
  public class PriceListResponseDto
  {
    public int status { get; set; }
    public string message { get; set; }
    public List<PriceListItemDto> result { get; set; }
  }

  public class PriceListItemDto
  {
    public string materialcode { get; set; }
    public string materialname { get; set; }
    public decimal price { get; set; }
    public string currency { get; set; }
    public decimal pricewithvat { get; set; }
    public int vat { get; set; }
    public string brand { get; set; }
    public string category { get; set; }
    public string productgroup { get; set; }
    public string color { get; set; }
    public string volume { get; set; }
    public string unit { get; set; }
    public bool isnew { get; set; }
  }

}