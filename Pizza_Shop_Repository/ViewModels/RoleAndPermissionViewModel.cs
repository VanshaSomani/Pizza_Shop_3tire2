namespace Pizza_Shop_Repository.ViewModels;

public class RoleAndPermissionViewModel
{
    public int Rpid { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public string PermissionName { get; set; }
    public bool Viewpermission { get; set; }
    public bool Addandeditpermission { get; set; }
    public bool Isdeletepermission { get; set; }
    public bool IsSelected { get; set; }
}
