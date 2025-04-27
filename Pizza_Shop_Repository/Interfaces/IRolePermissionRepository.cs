using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface IRolePermissionRepository
{
    public Task<List<Role>> GetAllRole();

    public Task<List<Roleandpermission>> GetAllRolePermission(int roleId);

    public Task<Roleandpermission> GetRolePermissionById(int Rpid);

    public Task UpdateRolePermission(Roleandpermission obj);

    public Task<Role> GetRoleByRoleName(string rolename);

    public Task<Permission> GetPermissionByPermissionName(string permissionname);

    public Task<Roleandpermission> GetRolePermissionByRoleIdAndPermissionId(int roleid , int permissionid);

    public Task<Role> GetRoleByRoleId(int roleId);
} 
