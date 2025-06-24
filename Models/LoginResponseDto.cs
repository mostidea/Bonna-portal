namespace Bonna_Portal_Bridge_Api.Models
{
  public class LoginResponseDto
  {
    public bool Error { get; set; }
    public string Token { get; set; }
    public BonnaUser User { get; set; }
    public RolesData RolesData { get; set; }
  }
  public class BonnaUser
  {
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
