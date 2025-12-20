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
      public List<StockDto> result { get; set; } = new();
   }

   public class StockDto
   {
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
      public string MAINMATGRP { get; set; }
      public string KPOIDLESTOCK { get; set; }
      public string ATILSTOKORTALAMASATIS { get; set; }
      public PriceListDetailDto priceListDetail { get; set; }
      public bool isCampaign { get; set; }
   }

   public class PriceListDetailDto
   {
      public string PRICELIST { get; set; }
      public string PRICE { get; set; }
      public string CURRENCY { get; set; }
      public string VALIDFROM { get; set; }
      public string VALIDUNTIL { get; set; }
   }
}