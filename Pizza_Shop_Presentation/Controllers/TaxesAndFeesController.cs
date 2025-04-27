using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;
using Pizza_Shop_Services.Services;

namespace Pizza_Shop_Presentation.Controllers;

public class TaxesAndFeesController : Controller
{
    private readonly ITaxesAndFeesService _taxesAndFeesService; 

    public TaxesAndFeesController(ITaxesAndFeesService taxesAndFeesService){
        _taxesAndFeesService = taxesAndFeesService;
    }

    #region TaxesAndFees Get
        [HasPermission("TaxAndFee","viewpermission")]
        [Authorize]
        public async Task<IActionResult> TaxFees(){
            (TaxesAndFeesViewModel obj , int TaxTotalRecord) = await _taxesAndFeesService.GetTaxesAndFeesAsync(null , 1 , 5);
            return View(obj);
        }
    #endregion

    #region Taxes List Partial View
        [HasPermission("TaxAndFee","viewpermission")]
        [Authorize]
        public async Task<PartialViewResult> GetDataForTaxlistPartialView(string SearchCriteria , int page = 1 , int pageSize = 5){
            (TaxesAndFeesViewModel obj , int TaxTotalRecord) = await _taxesAndFeesService.GetTaxesAndFeesAsync(SearchCriteria , page , pageSize);
            return PartialView("PartialView/TaxesListPartialView" , obj);
        }
    #endregion

    #region AddTax
        [HasPermission("TaxAndFee","addandeditpermission")]
        [Authorize]
        public async Task<IActionResult> AddTax(TaxesAndFeesViewModel obj){
            if(await _taxesAndFeesService.AddTaxAsync(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region Edit Tax Get
        [HasPermission("TaxAndFee","addandeditpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditTax(int TaxId){
            try
            {
                return PartialView("PartialView/EditTaxModalPartialView" , await _taxesAndFeesService.GetTaxEdit(TaxId));
            }
            catch (Exception)
            {
                return Json(new {status = false});
            }
        }
    #endregion

    #region Edit Tax Post
        [HasPermission("TaxAndFee","addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditTax(TaxesAndFeesViewModel obj){
            if(await _taxesAndFeesService.UpdateTax(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region DeleteTax
        [HasPermission("TaxAndFee","isdeletepermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteTax(int TaxId){
            if(await _taxesAndFeesService.DeleteTaxAsync(TaxId)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region TaxIsEnabled
        [HasPermission("TaxAndFee","addandeditpermission")]
        [Authorize]
        public async Task<IActionResult> TaxIsEnabled(int TaxId , bool IsEnable){
            if(await _taxesAndFeesService.ChangeTaxEnabled(TaxId , IsEnable)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region DefaultTaxChange
        [HasPermission("TaxAndFee","addandeditpermission")]
        [Authorize]
        public async Task<IActionResult> DefaultTaxChange(int TaxId , bool Default){
            if(await _taxesAndFeesService.ChangeDefaultTax(TaxId , Default)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion
}
