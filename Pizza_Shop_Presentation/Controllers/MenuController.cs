using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;
using Pizza_Shop_Services.Services;

namespace Pizza_Shop_Presentation.Controllers;

public class MenuController : Controller
{
    private readonly IMenuService _menuService;
    private readonly INotyfService _notyf;
    public MenuController(IMenuService menuService , INotyfService notyf){
        _menuService = menuService;
        _notyf = notyf;
    }
    #region MenuItem Get
        [HasPermission ("Menu" , "viewpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MenuItems(int categoryId , string searchcriteria , int page = 1 , int pagesize = 5){
            (ItemCategoryViewModel obj , int totalRecord , int modifireTotalRecord) = await _menuService.GetCategoryAsync(categoryId , searchcriteria , page , pagesize);
            obj.Categoryid = obj.ItemList.First().Categoryid;
            if(obj.ModifiersList.Count() != 0){
                obj.Modifiergroupid = obj.ModifiersList.First().ModifierModifierGroupMappings.First().ModifierGroupId;
            }
            obj.Modifiergroupid = obj.ModifiersList.First().ModifierModifierGroupMappings.First().ModifierGroupId;
            ViewData["Categories"] = await _menuService.GetAllCategoryAsync();
            ViewData["ModifierGroup"] = await _menuService.GetAllModifierGroupAsync();
            return View(obj);
        }
    #endregion

    #region GetItemByCategoryId
        [HasPermission ("Menu" , "viewpermission")]
        [Authorize]
        public async Task<IActionResult> GetItemByCategoryId(int categoryId , string searchcriteria , int page = 1 , int pagesize = 5){
            (ItemCategoryViewModel obj , int totalRecord , int modifireTotalRecord) = await _menuService.GetCategoryAsync(categoryId , searchcriteria , page , pagesize);
            ViewBag.CategoryId = categoryId;
            if(obj.ModifiersList.Count() != 0){
                obj.Modifiergroupid = obj.ModifiersList.First().ModifierModifierGroupMappings.First().ModifierGroupId;
            }
            //obj.Modifiergroupid = obj.ModifiersList.First().ModifierModifierGroupMappings.First().ModifierGroupId;
            obj.Categoryid = categoryId;
            return PartialView("PartialView/ItemListPartialView" , obj);
        }
    #endregion

    #region getModifier for Add Item
        [HasPermission ("Menu" , "addandeditpermission")]
        public async Task<List<Modifier>> GetModForAddItem(int MgId){
            return await _menuService.GetModifiresAddItems(MgId);
        }
    #endregion

    #region GetModifiersByMGId
        [HasPermission ("Menu" , "viewpermission")]
        public async Task<IActionResult> GetModifiersByMGId(int Mgid , string searchcriteria , int page = 1 , int pagesize = 5){
            (ItemCategoryViewModel obj , int modifireTotalRecord) = await _menuService.GetModifiers(Mgid , searchcriteria , page , pagesize);
            if(obj.ModifiersList.Count() != 0){
                obj.Modifiergroupid = obj.ModifiersList.First().ModifierModifierGroupMappings.First().ModifierGroupId;
            }
            ViewData["ModifierGroup"] = await _menuService.GetAllModifierGroupAsync();
            return PartialView("PartialView/ModifiersListPartialView" , obj);
        }
    #endregion

    #region GetDataForExistingModifiers
        [HasPermission ("Menu" , "viewpermission")]
        public async Task<IActionResult> GetDataForExistingModifiers(int page = 1 , int pagesize = 5 , string searchcriteria = null){
            (ItemCategoryViewModel obj , int totalRecord) = await _menuService.GetAllModifiers(page , pagesize , searchcriteria);
            return PartialView("PartialView/AddExistingModifierModalPartialView" , obj);
        }
    #endregion 

    #region GetAllMgforPartialView
        [HasPermission ("Menu" , "viewpermission")]
        public async Task<IActionResult> GetAllMgforPartialView(){
            ItemCategoryViewModel obj = await _menuService.GetAllMG();
            return PartialView("PartialView/ModifierGroupListPartialView" , obj);
        }
    #endregion

    #region AddModifier
        [HasPermission ("Menu" , "addandeditpermission")]
        public async Task<IActionResult> AddModifier(ItemCategoryViewModel obj){
            if(await _menuService.AddModifierAsync(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region AddAllModifiersFromExModifier
        public async Task AddAllModifiersFromExModifier(List<int> mid , int MgId){
            await _menuService.AddModifierToMG(mid , MgId);
        }
    #endregion

    #region DeleteModifier
        [HasPermission ("Menu" , "isdeletepermission")]
        [Authorize]
        public async Task<IActionResult> DeleteModifier(ItemCategoryViewModel obj){
            if(await _menuService.DeletModifierAsync(obj.ModifierId , obj.Modifiergroupid)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region DeleteModifierMG
        public async Task<RedirectToActionResult> DeleteModifierMG(List<int> modifier , int MgId){
            try
            {
                for(int i = 0 ; i < modifier.Count ; i++){
                    await _menuService.DeletModifierAsync(modifier[i] , MgId);
                }
                TempData["status"] = true;
                _notyf.Error("Modifier Deleted Successfully",1);
                return RedirectToAction("GetModifiersByMGId" , "Menu" , new{Mgid = MgId});
            }
            catch (Exception e)
            {
                _notyf.Error("Something Went Wrong",1);
                return RedirectToAction("GetModifiersByMGId" , "Menu" , new{Mgid = MgId});
            }
        }
    #endregion

    #region AddCategory
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCategory(ItemCategoryViewModel obj){
            if(await _menuService.AddCategoryAsync(obj)){
                _notyf.Success("Category Added Successfully",1);        
                return RedirectToAction("MenuItems" , "Menu");
            }
            _notyf.Error("Something went wrong",1);        
            return RedirectToAction("MenuItems" , "Menu");
        }
    #endregion

    #region AddModifierGroup
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        public async Task<IActionResult> AddModifierGroup(string mgName , string mgDesc , List<int> mids){
            if(await _menuService.AddModifierGroupAsync(mgName , mgDesc , mids)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region EditModifierGroup
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditModifierGroup(int MGid , List<int> midList){
            try
            {
                (ItemCategoryViewModel obj ,bool status) = await _menuService.EditMGAsync(MGid , midList);
                return PartialView("PartialView/EditModiferGroupModalPartialView" , obj);
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                _notyf.Error("Something Went Wrong",1);
                return RedirectToAction("MenuItems" , "Menu");
            }
        }
    #endregion
    
    #region EditCategory
        [HasPermission ("Menu" , "addandeditpermission")]
        [HttpGet]
        public async Task<ItemCategoryViewModel> EditCategory(int categoryId){
            ItemCategoryViewModel category = await _menuService.EditCategoryAsync(categoryId);
            return category;
        }
    #endregion

    #region EditCategory Post
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditCategory(ItemCategoryViewModel obj){
            if(await _menuService.UpdateCategory(obj)){
                _notyf.Success("Category Editted Successfully",1);        
                return RedirectToAction("MenuItems" , "Menu");
            }
            _notyf.Error("Something Went Wrong",1);        
            return RedirectToAction("MenuItems" , "Menu");
        }
    #endregion

    #region EditModifierGroup Post
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditModifierGroup(ItemCategoryViewModel obj){
            if(await _menuService.UpdateMG(obj)){
                return Json(new {status = true});
            }
           return Json(new {status = false});
        }
    #endregion
    
    #region DeleteCategory
        [HasPermission ("Menu" , "isdeletepermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(ItemCategoryViewModel obj){
            if(await _menuService.DeleteCategoryAsync(obj)){
                _notyf.Success("Category Deleted",1);        
                return RedirectToAction("MenuItems" , "Menu");
            }
            _notyf.Error("Something Went Wrong",1);        
            return RedirectToAction("MenuItems" , "Menu");
        }
    #endregion

    #region EditModifer Get
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditModifer(int mid){
            try
            {
                (ItemCategoryViewModel obj , bool status) = await _menuService.EditModifierAsync(mid);
                ViewData["ModifierGroup"] = await _menuService.GetAllModifierGroupAsync();
                return PartialView("PartialView/EditModifierModalPartialView" , obj);
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                _notyf.Error("Something Went Wrong",1);
                return RedirectToAction("MenuItems" , "Menu");
            }
        }
    #endregion

    #region EditModifer Post
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        public async Task<IActionResult> EditModifer(ItemCategoryViewModel obj){
            if(await _menuService.UpdateModifier(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region DeleteMG
        [HasPermission ("Menu" , "isdeletepermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteMG(ItemCategoryViewModel obj){
            if(await _menuService.DeleteMGAsync(obj)){
                return Json(new {status = true});
            }
            else{
                return Json(new {status = false});
            }
        }
    #endregion

    #region DeleteMG
        [HasPermission ("Menu" , "isdeletepermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteMG(int MGid){
            try
            {
                (ItemCategoryViewModel obj ,bool status) = await _menuService.EditMGAsync(MGid);
                return PartialView("PartialView/DeleteModifierGroupPartialView" , obj);
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                _notyf.Error("Something Went Wrong",1);
                return RedirectToAction("MenuItems" , "Menu");
            }
        }
    #endregion

    #region AddItems
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddItems([FromForm] ItemCategoryViewModel obj , string ModiferGroupData){
            if(!string.IsNullOrEmpty(ModiferGroupData)){
                obj.ItemModifierGroupMapList = JsonConvert.DeserializeObject<List<ItemModifierGroupMapViewModel>>(ModiferGroupData);
            }
            if(await _menuService.AddItemsAsync(obj)){
                return Json(new {status = true});              
            }
            return Json(new {status = false});
        }
    #endregion

    #region DeleteItem
        [HasPermission ("Menu" , "isdeletepermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(ItemCategoryViewModel obj){
            if(await _menuService.DeleteItemAsync(obj)){
                _notyf.Success("Item Deleted",1);
                return RedirectToAction("MenuItems" , "Menu" , new{categoryId = obj.Categoryid});
            }
            _notyf.Error("Something went wrong",1);   
            return RedirectToAction("MenuItems" , "Menu" , new{categoryId = obj.Categoryid});
        }
    #endregion

    #region GetEditItemData
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        public async Task<IActionResult> EditItem(int itemid){
            try
            {
                ItemCategoryViewModel item = await _menuService.EditItemAsync(itemid);
                ViewData["Categories"] = await _menuService.GetAllCategoryAsync();
                ViewData["ModifierGroup"] = await _menuService.GetAllModifierGroupAsync();
                return PartialView("PartialView/EditItemModalPartialView" , item);
            }
            catch (Exception)
            {
                return Json(new {status = false});
            }
        }
    #endregion

    #region EditItem Post
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditItem(ItemCategoryViewModel obj , string ModifierGroupData){
            if(!string.IsNullOrEmpty(ModifierGroupData)){
                obj.ItemModifierGroupMapList = JsonConvert.DeserializeObject<List<ItemModifierGroupMapViewModel>>(ModifierGroupData);
            }
            if(await _menuService.UpdateItem(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region DeleteModifierGroupForEditItem
        [HttpPost]
        public async Task<IActionResult> DeleteModifierGroupForEditItem(List<int> modifierIdList , int itemId , int CategoryId){
            if(await _menuService.DeleteModifierGroupInEditItem(modifierIdList , itemId)){
                _notyf.Success("Item Editted",1);
                return RedirectToAction("MenuItems" , "Menu" , new{categoryId = CategoryId});
            }
            _notyf.Error("Something went wrong",1);
            return RedirectToAction("MenuItems" , "Menu" , new{categoryId = CategoryId});

        }
    #endregion

    #region MassDelete
        [HasPermission ("Menu" , "isdeletepermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MassDelete(List<int> itemid , int CategoryId){
            try
            {
                for(int i = 0 ; i < itemid.Count ; i++){
                    await _menuService.DeleteMassItem(itemid[i]);
                }
                _notyf.Success("Items Deleted",1);
                return RedirectToAction("GetItemByCategoryId" , "Menu" , new{categoryId = CategoryId});
            }
            catch (Exception e)
            {
                _notyf.Error("Something went wrong",1);
                return RedirectToAction("GetItemByCategoryId" , "Menu" , new{categoryId = CategoryId});
            }
        }
    #endregion

    #region MassDeleteModifiers
        [HasPermission ("Menu" , "isdeletepermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MassDeleteModifier(List<int> modifierIds , int modifierGroupId){
            await _menuService.ModifierDeleteMass(modifierIds , modifierGroupId);
            return Json(new {status = true});
        }        
    #endregion

    #region ChangeItemAvailable
        [HasPermission ("Menu" , "addandeditpermission")]
        [Authorize]
        public async Task<IActionResult> ChangeItemAvailable(int ItemId , bool Status){
            if(await _menuService.EditAvailableItem(ItemId , Status)){
                return Json(new {status = true});
            }
            return Json(new {status = false});  
        }
    #endregion

}