using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Bean;
using Pizza_Shop_Services.Interfaces;
using Pizza_Shop_Services.Services;

namespace Pizza_Shop_Presentation.Controllers;

public class RoleAndPermissionController :  Controller
{
    private readonly IRolePermissionService _rolePermissionServices;
    private readonly INotyfService _notyf;
    private readonly IJWTService _jwtService;

    public RoleAndPermissionController(IRolePermissionService rolePermissionServices , INotyfService notyf , IJWTService jwtService){
        _rolePermissionServices = rolePermissionServices;
        _notyf = notyf;
        _jwtService = jwtService;
    }

    #region RoleList Get
        [HasPermission ("RoleAndPermission" , "viewpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RoleList(){
            return View(await _rolePermissionServices.GetRoleListAsync());
        }
    #endregion

    #region RolePermission Get
        [HasPermission ("RoleAndPermission" , "viewpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RolePermission(int roleId , string roleName){
            ViewBag.RoleName = roleName;
            return View(await _rolePermissionServices.GetRolePermissionList(roleId));
        }
    #endregion

    #region RolePermission Post 
        [HasPermission ("RoleAndPermission" , "addandeditpermission")]
        [Authorize]
        public async Task<IActionResult> RolePermission(List<RoleAndPermissionViewModel> obj){
            if(await _rolePermissionServices.EditRolePermissionList(obj)){

                var role = await _rolePermissionServices.GetRoleNameForSessionData(obj.First().RoleId);
                var token = HttpContext.Request.Cookies["jwtToken"];
                var Userrole = _jwtService.GetClaimValue(token , "role");
                if(role.Roles == Userrole){
                    var userLayout_Permission = await _rolePermissionServices.GetDataForLayoutViewModel(role.Roles);
                    var json_userLayout_Permission = JsonConvert.SerializeObject(userLayout_Permission);
                    HttpContext.Session.SetString("Layout_Page_Permission",json_userLayout_Permission);
                }

                _notyf.Success(MessageHelper.GetEditSuccessMessage(Constant.Permission),1);        
                return RedirectToAction("RoleList" , "RoleAndPermission");
            }
            _notyf.Error(MessageHelper.GetEditErrorMessage(Constant.Permission),1);        
            return RedirectToAction("RoleList" , "RoleAndPermission");
        }
    #endregion
}
