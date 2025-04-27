using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly RmsdemoContext _db;

    public RolePermissionRepository(RmsdemoContext db){
        _db = db;
    }


    #region GetAllRole
        public async Task<List<Role>> GetAllRole()
        {
            return await _db.Roles.ToListAsync();
        }
    #endregion

    #region GetAllRolePermission
        public async Task<List<Roleandpermission>> GetAllRolePermission(int roleId)
        {
            return _db.Roleandpermissions.Include(r => r.Role).Include(r => r.Permission).Where(rp => rp.Roleid == roleId).ToList();
        }
    #endregion

    #region GetRolePermissionById
        public async Task<Roleandpermission> GetRolePermissionById(int Rpid){
            return await _db.Roleandpermissions.FirstOrDefaultAsync(u => u.Rpid == Rpid);
        }
    #endregion

    #region UpdateRolePermission
        public async Task UpdateRolePermission(Roleandpermission obj)
        {
            try
            {
                _db.Roleandpermissions.Update(obj);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
            }
        }
    #endregion

    #region GetRoleIdByRoleName
        public async Task<Role> GetRoleByRoleName(string rolename){
            return await _db.Roles.FirstOrDefaultAsync(r => r.Roles == rolename);
        }
    #endregion

    #region GetPermissionByPermissionName
        public async Task<Permission> GetPermissionByPermissionName(string permissionname){
            return await _db.Permissions.FirstOrDefaultAsync(p => p.Pemission == permissionname);
        }
    #endregion

    #region GetRolePermissionByRoleIdAndPermissionId
        public async Task<Roleandpermission> GetRolePermissionByRoleIdAndPermissionId(int roleid , int permissionid){
            return await _db.Roleandpermissions.Include(r => r.Permission).FirstOrDefaultAsync(rp => rp.Roleid == roleid && rp.Permissionid == permissionid);
        }
    #endregion

    #region GetRoleByRoleId
        public async Task<Role> GetRoleByRoleId(int roleId){
            return await _db.Roles.FirstOrDefaultAsync(r => r.Roleid == roleId);
        }
    #endregion
}
