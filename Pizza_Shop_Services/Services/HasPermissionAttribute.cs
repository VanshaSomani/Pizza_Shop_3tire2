using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class HasPermissionAttribute : Attribute, IAsyncAuthorizationFilter
{                 
    private readonly string _page;
    private readonly string _permission;

    public HasPermissionAttribute(string page , string permission){
        _page = page;
        _permission = permission;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var token = httpContext.Request.Cookies["jwtToken"];
        var permissionService = context.HttpContext.RequestServices.GetService<IRolePermissionService>();

        //check token is null or not
        if(string.IsNullOrEmpty(token)){
            context.Result = new RedirectToActionResult("Login" , "UserLogin" , null);
            return;
        }

        //check permissionService is null or not
        if(permissionService == null){
            context.Result = new RedirectToActionResult("Login" , "UserLogin" , null);
            return;
        }

        var handler = new JwtSecurityTokenHandler();

        try
        {
            var jwtToken = handler.ReadJwtToken(token) as JwtSecurityToken;

            //check jwtToken is null or not
            if(jwtToken == null){
                context.Result = new RedirectToActionResult("Login" , "UserLogin" , null);
                return;
            }

            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            //check role is null or not
            if(string.IsNullOrEmpty(role)){
                context.Result = new RedirectToActionResult("Login" , "UserLogin" , null);
                return;
            }

            var RolePermissionData = await permissionService.GetUserAccessDataByRoleAndPermissionName(role , _page);

            //check RolePermissionData is null or not
            if(RolePermissionData == null){
                context.Result = new RedirectToActionResult("Login" , "UserLogin" , null);
                return;
            }

            bool canAccess = _permission switch{
                "viewpermission" => RolePermissionData.Viewpermission == true,
                "addandeditpermission" => RolePermissionData.Addandeditpermission == true,
                "isdeletepermission" => RolePermissionData.Isdeletepermission == true,
                _ => false
            };

            /*httpContext.Items[$"HasPermission_{_page}_CanView"] = RolePermissionData.Viewpermission;
            httpContext.Items[$"HasPermission_{_page}_CanAddAndEdit"] = RolePermissionData.Addandeditpermission;
            httpContext.Items[$"HasPermission_{_page}_CanDelete"] = RolePermissionData.Isdeletepermission;*/

            if(!canAccess){
                if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Result = new JsonResult(new
                    {
                        access = false,
                        message = "Permission Denied."
                    });
                    return;
                }
                context.Result = new RedirectToActionResult("AccessDenied" , "ErrorPage" , null);
                return;
            }

            var identity = new ClaimsIdentity(jwtToken.Claims , "jwt");
            httpContext.User = new ClaimsPrincipal(identity);
        }
        catch
        {
            context.Result = new RedirectToActionResult("Login" , "UserLogin" , null);
        }
    }

}
