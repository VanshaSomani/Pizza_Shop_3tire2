using Microsoft.AspNetCore.Mvc;

namespace Pizza_Shop_Presentation.Controllers;

public class ErrorPageController : Controller
{
    #region AccessDenied
        public async Task<IActionResult> AccessDenied(){
            return View();
        }
    #endregion

    #region NotFound
        public async Task<IActionResult> NotFound(){
            return View();
        }
    #endregion
}
