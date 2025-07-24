namespace Bonna_Portal_Bridge_Api.Models
{
  public class InvoiceResponseDto
  {
    public bool error { get; set; }
    public string message { get; set; }
    public List<InvoiceData> body { get; set; } = new();
  }

  public class InvoiceData
  {
    public string CUSORDNUM { get; set; }
    public string DOCNUM { get; set; }
    public string DOCDATE { get; set; }
    public string DUEDATE { get; set; }
    public string STATUS { get; set; }
    public string CURR { get; set; }
    public string AMOUNT { get; set; }
    public string BTYPE { get; set; }
    public string SALESGROUP { get; set; }
    public string DEPARTMENT { get; set; }
    public string REPRESENTATIVE { get; set; }
    public string CUSTNAME { get; set; }
    public string CUSTNUM { get; set; }
    public string CITY { get; set; }
    public string COUNTRY { get; set; }
    public string TAXNUM { get; set; }
    public string TAXOFFICE { get; set; }
    public string TEL { get; set; }
    public string EMAIL { get; set; }
    public string ADRNUM { get; set; }
    public string ADRTEXT { get; set; }
  }
}
