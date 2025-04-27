using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Presentation.Controllers;

public class OrderAppWaitingListController : Controller
{
    private readonly IWaitingListService _waitingService;
    public OrderAppWaitingListController(IWaitingListService waitingService){
        _waitingService = waitingService;
    }

    #region OrderWaitingList
        public async Task<IActionResult> OrderWaitingList(int SectionId = -1){
            WaitingListViewModel obj = await _waitingService.GetDataForWaitingList(SectionId);
            return View(obj);
        }
    #endregion

    #region GetWaitingListForPartialView
        public async Task<IActionResult> GetWaitingListForPartialView(int SectionId  , int page , int PageSize , int TotalPage){
            WaitingListViewModel obj = await _waitingService.GetDataForWaitingListPartialView(SectionId , TotalPage , page , PageSize);
            return PartialView("PartialView/WaitingListTablePartialView" , obj);
        }
    #endregion

    #region DeleteWaitingToken
        public async Task<IActionResult> DeleteWaitingToken(int WaitingTokenId){
            if(await _waitingService.DeleteWaitingAsync(WaitingTokenId)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region EditWaitingListGet
        public async Task<IActionResult> EditWaitingListGet(int WaitingTokenId){
            OffCanvasCustomerDetailViewModel obj = await _waitingService.GetWaitingListForEdit(WaitingTokenId);
            return PartialView("PartialView/EditWaitingTokenModalPartialView",obj);
        }
    #endregion

    #region EditWaitingToken
        public async Task<IActionResult> UpdateWaitingToken(OffCanvasCustomerDetailViewModel obj){
            if(await _waitingService.EditWaitingTokenAsync(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region GetAssignWaitingToken
        public async Task<IActionResult> GetAssignWaitingToken(int SectionId , int WaitingId){
            AssignWaitingTokenViewModel obj = await _waitingService.GetDataForAssignWaitingToken(SectionId , WaitingId);
            return PartialView("PartialView/AssignWaitingModalPartialView",obj);
        }
    #endregion

    #region Name
        public async Task<IActionResult> AssignTable(AssignWaitingTokenViewModel obj , string TableIds){
            List<int> tableIds = JsonConvert.DeserializeObject<List<int>>(TableIds);
             return Json(new {status = await _waitingService.AssignTableToCustomer(obj , tableIds)});
        }
    #endregion
}
