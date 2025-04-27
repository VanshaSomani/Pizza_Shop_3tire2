using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Shop_Services.Services;

namespace Pizza_Shop_Presentation.Controllers;

public class DashboardController : Controller
{
    // [HasPermission ("" , "viewpermission")]
    [Authorize]
    public async Task<IActionResult> Dashboard(){
        return View();
    }
}
