namespace Bonna_Portal_Bridge_Api.Models
{
  public class GetOrderListResponse
  {
    public bool error { get; set; }
    public List<OrderDto> data { get; set; } = new();
  }

  public class OrderDto
  {
    //public float kdvsiztoplam { get; set; }
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
    public float FATURATOPLAMI { get; set; }
    public string TOPLAMINDIRIM { get; set; }
    public float KDVTUTARI { get; set; }
    public float GENELTOPLAM { get; set; }
    public string KALITE { get; set; }
  }

  public class GetOrderItemsRequestDto
  {
    public string DOCTYPE { get; set; }
    public string DOCNUM { get; set; }
    //public string Userid { get; set; }
  }

  public class GetOrderItemsResponseDto
  {
    public bool error { get; set; }
    public List<OrderItemDto> data { get; set; }
  }

  public class OrderItemDto
  {
    public string MALZEMEACIKLAMA { get; set; }
    public string MALZEMEKODU { get; set; }
    public string BIRIMFIYAT { get; set; }
    public string NETFIYAT { get; set; }
    public string BIRIMTOPLAMFIYAT { get; set; }
    public string TOPLAMFIYAT { get; set; }
    public string MIKTAR { get; set; }
    public string PARABIRIMI { get; set; }
    public string REZERVEMIKTAR { get; set; }
    public string SEVKMIKTAR { get; set; }
    public string TOPLAMADA { get; set; }
    public string ACIKMIKTAR { get; set; }
    public string TALEPTARIHI { get; set; }
    public string GECERLILIKTARIHI { get; set; }
    public string SEVKTARIHI { get; set; }
    public string EANKOD { get; set; }
    public string GTIP { get; set; }
    public string GROSSWEIGHT { get; set; }
    public string NETWEIGHT { get; set; }
    public string HACIM { get; set; }
    public string KUTUICIADET { get; set; }
    public string PALETADET { get; set; }
    public string ITEMNUMBER { get; set; }
    public string TOTALINDIRIM { get; set; }
    public string KDVSIZTOPLAM { get; set; }
  }
}
