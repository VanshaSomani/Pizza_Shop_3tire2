using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;
using Pizza_Shop_Services.Services;

namespace Pizza_Shop_Presentation.Controllers;

public class TableAndSectionController : Controller
{
    private readonly ITableAndSectionService _tabelSectionService;

    private readonly INotyfService _notyf;

    public TableAndSectionController(ITableAndSectionService tabelSectionService , INotyfService notyf){
        _tabelSectionService = tabelSectionService;
        _notyf = notyf;
    }

    #region TableSection Get
        [HasPermission("TableAndSection","viewpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TableSection(int page = 1 , int pageSize = 5){
            (TableSectionViewModel obj , int TableRecord) = await _tabelSectionService.GetSectionAsync(page , pageSize);
            return View(obj);
        }
    #endregion

    #region GetTableForTablePartial
        [HasPermission("TableAndSection","viewpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTableForTablePartial(int SectionId , string SearchCriteria , int page = 1 , int pageSize = 5){
            (TableSectionViewModel obj , int TableRecord) = await _tabelSectionService.LoadTablesSection(SectionId , SearchCriteria , page , pageSize);
            return PartialView("PartialView/TableListPartialView", obj);
        }
    #endregion

    #region GetSectionForSectionPartial
        [HasPermission("TableAndSection","viewpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSectionForSectionPartial(){
            return PartialView("PartialView/SectionListPartialView", await _tabelSectionService.LoadSection());
        }
    #endregion

    #region GetSectionDataForEdit
        [HasPermission("TableAndSection","addandeditpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSectionDataForEdit(int SectionId){
            try
            {
                return PartialView("PartialView/EditSectionModalViewModal" , await _tabelSectionService.GetDataSection(SectionId));
            }
            catch (Exception)
            {
                return Json(new {status = false});
            }
        }
    #endregion

    #region AddSection
        [HasPermission("TableAndSection","addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSection(TableSectionViewModel obj){
            if(await _tabelSectionService.AddSectionAsync(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region EditSection
        [HasPermission("TableAndSection","addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditSection(TableSectionViewModel obj){
                if(await _tabelSectionService.UpdateSection(obj)){
                    return Json(new {status = true});
                }
            return Json(new {status = false});
        }
    #endregion

    #region DeleteSection
        [HasPermission("TableAndSection","isdeletepermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteSection(TableSectionViewModel obj){
            if(await _tabelSectionService.DeleteSectionById(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region GetDataForAddTable
        [HasPermission("TableAndSection","addandeditpermission")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDataForAddTable(){
            ViewData["Section"] = await _tabelSectionService.getAllSection();
            TableSectionViewModel obj = new TableSectionViewModel();
            return PartialView("PartialView/AddTableModalPartialView" , obj);
        }
    #endregion

    #region AddTable
        [HasPermission("TableAndSection","addandeditpermission")]
        [Authorize]
        public async Task<IActionResult> AddTable(TableSectionViewModel obj){
            if(await _tabelSectionService.AddTableAsync(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region Get Data For Edit Table
        [HttpGet]
        public async Task<IActionResult> GetEditTable(int tableId){
            try
            {
                ViewData["Section"] = await _tabelSectionService.getAllSection();
                return PartialView("PartialView/EditTableModalPartialView" , await _tabelSectionService.GetTableEditAsync(tableId));
            }
            catch (Exception)
            {
                return Json(new {status = false});
            }
        }
    #endregion

    #region Edit Table
        [HasPermission("TableAndSection","addandeditpermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditTable(TableSectionViewModel obj){
                if(await _tabelSectionService.EditTableAsync(obj)){
                    return Json(new {status = true});
                }
            return Json(new {status = false});
        }
    #endregion

    #region Delete Table Post
        [HasPermission("TableAndSection","isdeletepermission")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteTable(TableSectionViewModel obj){
            if(await _tabelSectionService.DeleteTableAsync(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = true});
        }
    #endregion

    #region Massdelete
        [HasPermission("TableAndSection","isdeletepermission")]
        [Authorize]
        public async Task<IActionResult> Massdelete(List<int> TableIds , int SectionId){
            if(await _tabelSectionService.MultipleDeleteTable(TableIds , SectionId)){
                return Json(new {status = true});
            }
            else{
                return Json(new {status = false});
            }
        }
    #endregion

}
