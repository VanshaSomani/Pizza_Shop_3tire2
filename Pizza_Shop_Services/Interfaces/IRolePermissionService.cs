using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IRolePermissionService
{
    public Task<List<Role>> GetRoleListAsync();

    public Task<List<RoleAndPermissionViewModel>> GetRolePermissionList(int roleId);

    public Task<bool> EditRolePermissionList(List<RoleAndPermissionViewModel> rpList);

    public Task<Roleandpermission> GetUserAccessDataByRoleAndPermissionName(string rolename , string permissionname);

    public Task<UserLayoutViewModel> GetDataForLayoutViewModel(string rolename);

    public Task<Role> GetRoleNameForSessionData(int roleId);
}
