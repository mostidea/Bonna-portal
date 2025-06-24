namespace Bonna_Portal_Bridge_Api.Models
{
  public class StockResponseDto
  {
    public bool Error { get; set; }
    public StockDetailsDto Details { get; set; }
    public List<StockItemDto> Data { get; set; }
  }

  public class StockDetailsDto
  {
    public object Search { get; set; }
    public object Sort { get; set; }
    public object Skip { get; set; }
    public object Limit { get; set; }
    public int Page { get; set; }
    public StockPagesDto Pages { get; set; }
    public int TotalRecords { get; set; }
  }

  public class StockPagesDto
  {
    public bool Previous { get; set; }
    public int Current { get; set; }
    public int Next { get; set; }
    public object Total { get; set; }
  }

  public class StockItemDto
  {
    public string _id { get; set; }
    public string MATERIAL { get; set; }
    public string AVAILQTY1 { get; set; }
    public string AVAILQTY2 { get; set; }
    public string AVAILSTOCK1 { get; set; }
    public string AVAILSTOCK2 { get; set; }
    public string EANKOD { get; set; }
    public string ESTEXT { get; set; }
    public string HASARLI { get; set; }
    public string ISUNSRAIL { get; set; }
    public string KALTE1400 { get; set; }
    public string KALTE1C { get; set; }
    public string KALTE1DHL { get; set; }
    public string KALTE1P { get; set; }
    public string KALTE2400 { get; set; }
    public string KALTE2C { get; set; }
    public string KALTE2P { get; set; }
    public string KATALOGTIPI { get; set; }
    public string KOLEKSIYON { get; set; }
    public string KOLEKSIYONACK { get; set; }
    public string MINLOT { get; set; }
    public string MOQSTOK1 { get; set; }
    public string MOQSTOK2 { get; set; }
    public string PALETADET { get; set; }
    public string PCS { get; set; }
    public string PRICEVISIBLE { get; set; }
    public string QUNIT { get; set; }
    public string TAMAMLAYICI { get; set; }
    public string TSTEXT { get; set; }
    public string URUNSTATU { get; set; }
    public int __v { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public string MAINMATGRP { get; set; }
  }
  public class StockDetailResponseDto
  {
    public bool Error { get; set; }
    public StockItemDto Data { get; set; }
  }
}