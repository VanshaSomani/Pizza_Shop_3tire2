using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Bean;
using Pizza_Shop_Services.Interfaces;
using Pizza_Shop_Services.Services;

namespace Pizza_Shop_Presentation.Controllers;

public class UserLoginController : Controller
{
    private readonly IUserLoginService _userLoginServices;
    private readonly INotyfService _notyf;
    private readonly IRolePermissionService _rolePermissionServices;

    public UserLoginController(IUserLoginService userLoginServices , INotyfService notyf , IRolePermissionService rolePermissionServices){
        _userLoginServices = userLoginServices;
        _notyf = notyf;
        _rolePermissionServices = rolePermissionServices;
    }

    #region Login Get
        public async Task<IActionResult> Login(){
            try
            {
                string jwt = Request.Cookies["jwtToken"];
                if(jwt != null && Request.Cookies["RememberMe"] != null){
                    (bool check , int userid , string role) = await _userLoginServices.RememberMe(jwt);

                    UserLayoutViewModel userLayout_Permission = await _rolePermissionServices.GetDataForLayoutViewModel(role);
                    var json_userLayout_Permission = JsonConvert.SerializeObject(userLayout_Permission);
                    HttpContext.Session.SetString("Layout_Page_Permission",json_userLayout_Permission);

                    if(check == true && userid != 0){
                        if(role == "chef"){
                            HttpContext.Session.SetString("UserId", userid.ToString());
                            _notyf.Success(Constant.LoginSuccess , 1);
                            return RedirectToAction("UserList" , "User");
                        }
                        else{
                            HttpContext.Session.SetString("UserId", userid.ToString());
                            _notyf.Success(Constant.LoginSuccess , 1);
                            return RedirectToAction("Dashboard" , "Dashboard");
                        }
                    }
                }
                return View();
            }
            catch (Exception)
            {
                _notyf.Error(Constant.AuthorizationFail , 1);
                return View();
            }
        }
    #endregion.

    #region Login Post
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel obj){
            try
            {
                (string result , int userid , string role) = await _userLoginServices.LoginAsync(obj);

                UserLayoutViewModel userLayout_Permission = await _rolePermissionServices.GetDataForLayoutViewModel(role);
                var json_userLayout_Permission = JsonConvert.SerializeObject(userLayout_Permission);
                HttpContext.Session.SetString("Layout_Page_Permission",json_userLayout_Permission);

                if(result != null || userid != null){
                    Response.Cookies.Append("jwtToken", result , new CookieOptions{
                        Expires = DateTime.UtcNow.AddDays(30)
                    });
                    HttpContext.Session.SetString("UserId", userid.ToString());
                    if(obj.RemeberMe){
                        Response.Cookies.Append("RememberMe", obj.RemeberMe.ToString(), new CookieOptions{
                        Expires = DateTime.UtcNow.AddDays(30)
                    });
                    }
                }
                else{
                    _notyf.Error(Constant.LoginFail , 1);
                    ViewBag.ErrorMsg = "Invalid Email or Password";
                    return View(obj);
                }
                if(role == "chef"){
                    _notyf.Success(Constant.LoginSuccess , 1);
                    return RedirectToAction("Dashboard" , "Dashboard");
                }
                else{
                    _notyf.Success(Constant.LoginSuccess , 1);
                    return RedirectToAction("UserList" , "User");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                _notyf.Error(Constant.LoginInvalid , 1);
                return View(obj);
            }
        }
    #endregion

    #region Logout
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            Response.Cookies.Delete("RememberMe");
            Response.Cookies.Delete("jwtToken");
            
            return RedirectToAction("Login" , "UserLogin");
        }
    #endregion

    #region ForgotPassowrd get
        [HttpGet]
        public async Task<IActionResult> ForgotPassword(){
            return View();
        }
    #endregion

    #region StoredEmail
        public async Task StoredEmail(string email){
            TempData["Email"] = email;
        }
    #endregion

    #region ForgotPassword post
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel obj){
            try
            {
                await _userLoginServices.ForgotPasswordAsync(obj.Email);
                _notyf.Success(Constant.ResetEmailSent , 2);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Email does not exist";
                _notyf.Error(Constant.InvalidEmail , 2);
                return View(obj);
            }
            
        }
    #endregion

    #region ResetPassword Get
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email , string token){
            ResetPasswordViewModel user = await _userLoginServices.ResetPasswordAsync(email , token);
            if(user != null){
                return View(user);
            }
            else{
                return NotFound("User is not found or link ise expired");
            }
        }
    #endregion

    #region ResetPassword post
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel obj){
            bool check = await _userLoginServices.ResetPasswordUpdate(obj);
            if(check){
                _notyf.Success(Constant.PasswordResetSuccess , 2);
                return RedirectToAction("Login" , "UserLogin");
            }
            _notyf.Error(Constant.PasswordResetFail , 2);
            return View(obj);
        }
    #endregion

}
