namespace Bonna_Portal_Bridge_Api.Models
{
  public class GetOrderListResponse
  {
    public bool error { get; set; }
    public List<OrderDto> data { get; set; } = new();
  }

  public class OrderDto
  {
    public string DOCTYPE { get; set; }
    public string DOCNUM { get; set; }
    public string BELGENO { get; set; }
    public string BAYI { get; set; }
    public string MUSTERIKODU { get; set; }
    public string MUSTERI { get; set; }
    public string TARIH { get; set; }
    public string GECERLILIKTARIHI { get; set; }
    public string TUTAR { get; set; }
    public string PARABIRIMI { get; set; }
    public string MUSTERPNO { get; set; }
    public string DURUM { get; set; }
    public string KISMIMI { get; set; }
    public string YORUM { get; set; }
    public string BELGESAHIP { get; set; }
    public string BELGETIP { get; set; }
    public string ADRES { get; set; }
    public string ODEMEKOSULU { get; set; }
    public string ODEMETIPI { get; set; }
    public string KPOORDERCHANNEL { get; set; }
    public string KPOISCUSTADR { get; set; }
    public string VATKEY { get; set; }
    public string FIYATLISTESI { get; set; }
    public string FABRIKASEVKTARIHI { get; set; }
    public string INDIRIM1 { get; set; }
    public string INDIRIM2 { get; set; }
    public string INDIRIM3 { get; set; }
    public string FATURATOPLAMI { get; set; }
    public string TOPLAMINDIRIM { get; set; }
    public string KDVTUTARI { get; set; }
    public string GENELTOPLAM { get; set; }
    public string KALITE { get; set; }
  }

}
