namespace Bonna_Portal_Bridge_Api.Models
{
  public class InvoiceResponseDto
  {
    public bool error { get; set; }
    public string message { get; set; }
    public List<InvoiceData> data { get; set; } = new();
  }

  public class InvoiceData
  {
    public string DOCTYPE { get; set; }
    public string DOCNUM { get; set; }
    public string STATUS { get; set; }
    public string PRINTESNUM { get; set; }
    public string PRINTEDNUM { get; set; }
    public string PRINTEFNUM { get; set; }
    public string TOPLAMAORAN { get; set; }
    public string MUSTERIADI { get; set; }
    public string KALITE { get; set; }
    public string PLATFORM { get; set; }
    public string SIPAMACI { get; set; }
    public string SEVKYERI { get; set; }
    public string SEVKEDILENDEPO { get; set; }
    public string FIYATLISTESI { get; set; }
    public string GCURRENCY { get; set; }
    public List<object> SALITEMS { get; set; }
    public string SFORDERID { get; set; }
    public string SSEVKTARIHI { get; set; }
    public string SOLUSTURMATARIHI { get; set; }
    public string SSONTERMINTARIHI { get; set; }
    public string STALEPTARIHI { get; set; }
  }
}
