using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Bean;
using Pizza_Shop_Services.Interfaces;
using Pizza_Shop_Services.Services;

namespace Pizza_Shop_Presentation.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly INotyfService _notyf;

    public UserController(IUserService userService , INotyfService notyf){
        _userService = userService;
        _notyf = notyf;
    }

    #region UserList Get
        [HasPermission ("Users" , "viewpermission")]
        [Authorize]
        public async Task<IActionResult> UserList(string sortBy , bool desc , string searchcriteria , int page = 1 , int pagesize = 5 ){
            TempData["UserId"] = HttpContext.Session.GetString("UserId");
            (List<UserListViewModel> user_list , int totalrecord) = await _userService.UserListAsync(sortBy , desc , page , pagesize , searchcriteria);
            ViewBag.PageSize = pagesize;
            // var users_count = user_list.Count();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = (int)Math.Ceiling((double)totalrecord/pagesize);
            ViewBag.TotalRecords = totalrecord;
            ViewBag.MinRowCount = ((page - 1)*pagesize)+1;
            ViewBag.MaxRowCount = ViewBag.MinRowCount + user_list.Count() - 1;
            return View(user_list);
        }
    #endregion

    #region UserList Get
        [HasPermission ("Users" , "viewpermission")]
        [Authorize]
        public async Task<IActionResult> UserListPartialData(string sortBy , bool desc , string searchcriteria , int page = 1 , int pagesize = 5){
            TempData["UserId"] = HttpContext.Session.GetString("UserId");
            (List<UserListViewModel> user_list , int totalrecord) = await _userService.UserListAsync(sortBy , desc , page , pagesize , searchcriteria);
            ViewBag.PageSize = pagesize;
            // var users_count = user_list.Count();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = (int)Math.Ceiling((double)totalrecord/pagesize);
            ViewBag.TotalRecords = totalrecord;
            ViewBag.MinRowCount = ((page - 1)*pagesize)+1;
            ViewBag.MaxRowCount = ViewBag.MinRowCount + user_list.Count() - 1;
            return PartialView("PartialView/UserListPartialView" , user_list);
        }
    #endregion

    #region ChangePassword Get
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePassword(){
            return View();
        }
    #endregion

    #region ChangePassword Post
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel obj){
            if(await _userService.ChangePasswordAsync(obj)){
                _notyf.Success(MessageHelper.GetEditSuccessMessage(Constant.Password),3);
                return RedirectToAction("UserList" , "User");
            }
            _notyf.Error(MessageHelper.GetEditErrorMessage(Constant.Password),3);
            return View();
        }
    #endregion

    #region UserList Delete
        [HasPermission ("Users" , "isdeletepermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId){
            if(await _userService.UserDeleteAsync(userId)){
                _notyf.Success(MessageHelper.GetDeleteSuccessMessage(Constant.User),3);
                return RedirectToAction("UserList" , "User");
            }
            _notyf.Error(MessageHelper.GetDeleteErrorMessage(Constant.User),3);
            return RedirectToAction("UserList" , "User");
        }
    #endregion

    #region AddUser get
        [HasPermission ("Users" , "addandeditpermission")]
        [HttpGet]
        public async Task<IActionResult> AddUser(){
            var (countries , roles) = await _userService.GetAddUserAsync();
            ViewData["Countries"] = countries;
            ViewData["Roles"] = roles;
            return View();
        }
    #endregion

    #region GetStates
        public async Task<List<State>> GetStates(int Countryid){
            return await _userService.GetStatesAsync(Countryid);
        }
    #endregion

    #region GetCities
        public async Task<List<City>> GetCities(int Stateid){
            return await _userService.GetCitiesAsync(Stateid);
        }
    #endregion

    #region AddUser post
        [HasPermission ("Users" , "addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel obj){
            if(await _userService.AddUserAsync(obj)){
                _notyf.Success(MessageHelper.GetAddSuccessMessage(Constant.User),3);
                return RedirectToAction("UserList" , "User");
            }
            _notyf.Error(MessageHelper.GetAddErrorMessage(Constant.User),3);
            return RedirectToAction("UserList" , "User");
        }
    #endregion

    #region EditUser Get
        [HasPermission ("Users" , "addandeditpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditUser(int userid){
            EditUserViewModel obj = await _userService.GetEditUserAsync(userid);
            var (countries , roles) = await _userService.GetAddUserAsync();
            ViewData["Countries"] = countries;
            ViewData["Roles"] = roles;
            var (states , cities) = await _userService.GetSelectedStatesCities(obj.Countryid , obj.Stateid);
            ViewData["States"] = states;
            ViewData["Cities"] = cities;
            return View(obj);
        }
    #endregion

    #region EditUser Post
        [HasPermission ("Users" , "addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel obj){
            if(await _userService.EditUserAsync(obj)){
                _notyf.Success(MessageHelper.GetEditSuccessMessage(Constant.User),1);
                return RedirectToAction("UserList" , "User");
            }
            _notyf.Error(MessageHelper.GetEditErrorMessage(Constant.User),3);
            return RedirectToAction("UserList" , "User");
        }
    #endregion

    #region UserProfile Get
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserProfile(){
            ProfileViewModel obj = await _userService.GetProfileViewModelAsync();
            var (countries , roles) = await _userService.GetAddUserAsync();
            ViewData["Countries"] = countries;
            var (states , cities) = await _userService.GetSelectedStatesCities(obj.Countryid , obj.Stateid);
            ViewData["States"] = states;
            ViewData["Cities"] = cities;
            return View(obj);
        }
    #endregion

    #region UserProfile Post
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserProfile(ProfileViewModel obj){
            if(await _userService.ProfileViewModelAsync(obj)){
                _notyf.Success(MessageHelper.GetEditSuccessMessage(Constant.Profile),3);
                return RedirectToAction("UserList" , "User");
            }
            _notyf.Error(MessageHelper.GetEditErrorMessage(Constant.Profile),1);
            return RedirectToAction("UserList" , "User");
        }
    #endregion
}
