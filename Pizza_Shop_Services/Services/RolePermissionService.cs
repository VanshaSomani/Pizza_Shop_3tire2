using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class RolePermissionService : IRolePermissionService
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IHttpContextAccessor _httpaccessor;

    public RolePermissionService(IRolePermissionRepository rolePermissionRepository , IHttpContextAccessor httpaccessor){
        _rolePermissionRepository = rolePermissionRepository;
        _httpaccessor = httpaccessor;
    }

    public async Task<bool> EditRolePermissionList(List<RoleAndPermissionViewModel> rpList)
    {
        try
        {
            foreach(RoleAndPermissionViewModel newpermission in rpList){
                Roleandpermission updatedRolePremission = await _rolePermissionRepository.GetRolePermissionById(newpermission.Rpid);
                if(newpermission.IsSelected){
                    updatedRolePremission.Isselected = newpermission.IsSelected;
                    updatedRolePremission.Viewpermission = newpermission.Viewpermission;
                    updatedRolePremission.Addandeditpermission = newpermission.Addandeditpermission;
                    updatedRolePremission.Isdeletepermission = newpermission.Isdeletepermission;
                }
                else{
                    updatedRolePremission.Isselected = newpermission.IsSelected;
                }    
                await _rolePermissionRepository.UpdateRolePermission(updatedRolePremission);
            }
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }


    public async Task<List<Role>> GetRoleListAsync()
    {
        return await _rolePermissionRepository.GetAllRole();
    }

    public async Task<List<RoleAndPermissionViewModel>> GetRolePermissionList(int roleId)
    {
        List<Roleandpermission> rolePermissionList = await _rolePermissionRepository.GetAllRolePermission(roleId);
        List<RoleAndPermissionViewModel> role_permission_list = new ();
        foreach(Roleandpermission data in rolePermissionList){
            RoleAndPermissionViewModel RolePermission_data = new RoleAndPermissionViewModel();
            RolePermission_data.Rpid = data.Rpid;
            RolePermission_data.RoleId = data.Roleid;
            RolePermission_data.PermissionId = data.Permissionid;
            RolePermission_data.PermissionName = data.Permission.Pemission;
            RolePermission_data.Viewpermission = (bool)data.Viewpermission;
            RolePermission_data.Addandeditpermission = (bool)data.Addandeditpermission;
            RolePermission_data.Isdeletepermission = (bool)data.Isdeletepermission;
            RolePermission_data.IsSelected = (bool)data.Isselected;
            role_permission_list.Add(RolePermission_data);
        }
        return role_permission_list;
    }

    public async Task<UserLayoutViewModel> GetDataForLayoutViewModel(string rolename){
        Role r = await _rolePermissionRepository.GetRoleByRoleName(rolename);
        List<Roleandpermission> rolePermissionList = await _rolePermissionRepository.GetAllRolePermission(r.Roleid);
        List<RoleAndPermissionViewModel> role_permission_list = new List<RoleAndPermissionViewModel>();
        foreach(Roleandpermission data in rolePermissionList){
            RoleAndPermissionViewModel RolePermission_data = new RoleAndPermissionViewModel();
            RolePermission_data.Rpid = data.Rpid;
            RolePermission_data.RoleId = data.Roleid;
            RolePermission_data.PermissionId = data.Permissionid;
            RolePermission_data.PermissionName = data.Permission.Pemission;
            RolePermission_data.Viewpermission = (bool)data.Viewpermission;
            RolePermission_data.Addandeditpermission = (bool)data.Addandeditpermission;
            RolePermission_data.Isdeletepermission = (bool)data.Isdeletepermission;
            RolePermission_data.IsSelected = (bool)data.Isselected;
            role_permission_list.Add(RolePermission_data);
        }
        UserLayoutViewModel obj = new UserLayoutViewModel{
            UserPageViewList = role_permission_list
        };
        return obj;
    }

    public async Task<Roleandpermission> GetUserAccessDataByRoleAndPermissionName(string rolename , string permissionname){
        Role r = await _rolePermissionRepository.GetRoleByRoleName(rolename);
        Permission p = await _rolePermissionRepository.GetPermissionByPermissionName(permissionname);
        Roleandpermission rp = await _rolePermissionRepository.GetRolePermissionByRoleIdAndPermissionId(r.Roleid , p.Permissionid);
        return rp;
    }

    public async Task<Role> GetRoleNameForSessionData(int roleId){
        return await _rolePermissionRepository.GetRoleByRoleId(roleId);
    }
}
