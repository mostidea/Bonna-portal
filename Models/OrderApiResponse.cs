namespace Bonna_Portal_Bridge_Api.Models
{
  public class OrderApiResponse
  {
    public bool Error { get; set; }
    public int Lenght { get; set; }
    public List<OrderData> Data { get; set; }
  }

  public class OrderData
  {
    public string _id { get; set; }
    public string uid { get; set; }
    public string plan { get; set; }
    public string pcom { get; set; }
    public string pdoctype { get; set; }
    public string pdocnum { get; set; }
    public string pvalf { get; set; }
    public string pvalu { get; set; }
    public string pbayi { get; set; }
    public string psalnotes { get; set; }
    public string pdiscrate { get; set; }
    public string pdiscrate2 { get; set; }
    public string porderdetail { get; set; }
    public string pname { get; set; }
    public string comment { get; set; }
    public string createdDate { get; set; }
    public bool status { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public List<OrderItem> pitems { get; set; }
  }

  // Bu kısmı TUT
  public class OrderItem
  {
    public string ITEMNUM { get; set; }
    public string MATERIAL { get; set; }
    public string QUANTITY { get; set; }
    public string VOPTIONS { get; set; }
    public string DESDLVDATE { get; set; }
    public string KPOPRODDATE2 { get; set; }
    public string KPOISTERMINATED { get; set; }
    public string ITEMTYPE { get; set; }
    public string DISCKEY1 { get; set; }
    public string DISCTYPE1 { get; set; }
    public string DISCRATE1 { get; set; }
    public string DISCKEY2 { get; set; }
    public string DISCTYPE2 { get; set; }
    public string DISCRATE2 { get; set; }
    public string INTORDERNUM { get; set; }
    public string _id { get; set; }
  }

  public class OrderDetailResponse
  {
    public bool Error { get; set; }
    public OrderDetailData Data { get; set; }
  }

  public class OrderDetailData
  {
    public string _id { get; set; }
    public List<OrderItem> Pitems { get; set; }
    public string Uid { get; set; }
    public string Plan { get; set; }
    public string Pcom { get; set; }
    public string Pdoctype { get; set; }
    public string Pdocnum { get; set; }
    public string Pvalf { get; set; }
    public string Pvalu { get; set; }
    public string Pbayi { get; set; }
    public string Pcus { get; set; }
    public string Prefdoctype { get; set; }
    public string Prefdocn { get; set; }
    public string Psalnotes { get; set; }
    public string Pisfixquan { get; set; }
    public string Piscustadr { get; set; }
    public string Padrrow { get; set; }
    public string Pmodi { get; set; }
    public string Pisrev { get; set; }
    public string Pdiscrate { get; set; }
    public string Prevtype { get; set; }
    public string Prevreason { get; set; }
    public string Ppaymcond { get; set; }
    public string Pdiscrate2 { get; set; }
    public string Pdiscrate3 { get; set; }
    public string Pdiscrate4 { get; set; }
    public string Pdiscrate5 { get; set; }
    public string Porderdetail { get; set; }
    public string Padrnum { get; set; }
    public string Pkpomustno { get; set; }
    public string Pkpomustipno { get; set; }
    public string Pcustpurinfo { get; set; }
    public string Pname { get; set; }
    public string Comment { get; set; }
    public string CreatedDate { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int __v { get; set; }
  }
}
