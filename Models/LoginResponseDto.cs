namespace Bonna_Portal_Bridge_Api.Models
{
  public class ForgotPasswordResponseDto
  {
    public bool error { get; set; }
    public string message { get; set; }
    public string result { get; set; }
  }
  public class ForgotPasswordRequestDto
  {
    public string Email { get; set; }
  }

  public class LoginResponseDto
  {
    public bool Error { get; set; }
    public string Token { get; set; }
    public BonnaUser User { get; set; }
    public List<ErpData> ErpData { get; set; }
    public RolesData RolesData { get; set; }
  }

  public class BonnaUser
  {
    public string Token { get; set; }
    public string _id { get; set; }
    public string Namesurname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Language { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsController { get; set; }
    public bool IsSuper { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int __v { get; set; }
  }

  public class ErpData
  {
    public string CLIENT { get; set; }
    public string COMPANY { get; set; }
    public string USERNAME { get; set; }
    public string SALDEPT { get; set; }
    public string EMAIL { get; set; }
    public string KPOISCUSTOMER { get; set; }
    public string KPOISNATION { get; set; }
    public string KPOAUTHORITY { get; set; }
    public string BPHONE { get; set; }
    public string BMOBILE { get; set; }
    public string KPOCUSTOMER { get; set; }
    public string KPOCURRENCY { get; set; }
    public string CCUSTNAME { get; set; }
    public string KPOCUSADRNUM { get; set; }
    public string KPOCOMMISSIONER { get; set; }
    public string KPOCOMADRNUM { get; set; }
    public List<TmpLoginDetailCus> TMPLOGINDETAILCUS { get; set; }
    public List<TmpCusAdr> TMPCUSADR { get; set; }
  }

  public class TmpLoginDetailCus
  {
    public string ISCUSTORVEND { get; set; }
    public string CUSTOMER { get; set; }
    public string NAME { get; set; }
    public string ADDRESS { get; set; }
    public string CITY { get; set; }
    public string TELNUM { get; set; }
    public string TLXNUM { get; set; }
    public string CUSTCOND { get; set; }
    public string PRICELIST { get; set; }
    public string PAYMCOND { get; set; }
    public string SALDEPT { get; set; }
    public string KPORAPORBOLGE { get; set; }
    public string CUSTGRP { get; set; }
    public string REGION { get; set; }
    public string COUNTRY { get; set; }
    public string CURRENCY { get; set; }
    public string DISCOUNT1 { get; set; }
    public string DISCOUNT2 { get; set; }
    public List<TmpCust> TMPCUST { get; set; }
    public List<TmpDocType> TMPDOCTYPE { get; set; }
    public List<TmpVOption> TMPVOPTION { get; set; }
  }

  public class TmpCust
  {
    public string LEADCODE { get; set; }
    public string LEADNAME { get; set; }
  }

  public class TmpDocType
  {
    public string DOCTYPE { get; set; }
    public string STEXT { get; set; }
    public string KPOKDVORAN { get; set; }
    public string STATUS { get; set; }
    public string KARGOBEDELI { get; set; }
  }

  public class TmpVOption
  {
    public string VALUE { get; set; }
    public string STEXT { get; set; }
    public string QCODE { get; set; }
  }

  public class TmpCusAdr
  {
    public string CUSTOMER { get; set; }
    public string ADRNUM { get; set; }
    public string ADDRESSLINE1 { get; set; }
    public List<string> TAXDEPT { get; set; }
    public List<string> CITY { get; set; }
    public string ZIPSTREET { get; set; }
    public string COUNTRY { get; set; }
    public string CURRENCY { get; set; }
    public List<string> TLXNUM { get; set; }
  }

  public class RolesData
  {
    public PermissionRoles PermissionRoles { get; set; }
    public string _id { get; set; }
    public string UserId { get; set; }
    public string PermissionDesc { get; set; }
    public string PermissionValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int __v { get; set; }
  }

  public class PermissionRoles
  {
    public Permission dashboard { get; set; }
    public Permission waybill { get; set; }
    public Permission invoice { get; set; }
    public Permission stock { get; set; }
    public Permission stockQuantity { get; set; }
    public Permission reservationQuantity { get; set; }
    public Permission offer { get; set; }
    public Permission order { get; set; }
    public Permission newOffer { get; set; }
    public Permission newOrder { get; set; }
    public Permission bonnaPos { get; set; }
    public Permission bonnaPosTransactions { get; set; }
    public Permission productPrice { get; set; }
    public Permission paymentTracking { get; set; }
    public Permission pendingProducts { get; set; }
    public Permission newCustomer { get; set; }
    public Permission customers { get; set; }
    public Permission download { get; set; }
    public Permission qt02 { get; set; }
  }

  public class Permission
  {
    public bool Write { get; set; }
    public bool Read { get; set; }
  }

  public class LoginRequestDto
  {
    public string Username { get; set; }
    public string Password { get; set; }
  }

}
