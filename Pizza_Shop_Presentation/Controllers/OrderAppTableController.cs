using System.Security.Cryptography.X509Certificates;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Presentation.Controllers;

public class OrderAppTableController : Controller
{
    private readonly IOrderAppTableService _tableService;
    private readonly INotyfService _notyf;
    public OrderAppTableController(IOrderAppTableService tableService , INotyfService notyf){
        _tableService = tableService;
        _notyf = notyf;
    }
    #region GetTableView
        public async Task<IActionResult> OrderTable(){
            OrderAppTableList obj = await _tableService.GetDataForTableAccoordian();
            return View(obj);
        }
    #endregion

    #region OpenOffCanvas
        public async Task<IActionResult> GetOffCanavasWaitingList(int SectionId){
            OffCanvasWaitingListViewModel obj = await _tableService.GetWaitingListForOfCanvas(SectionId);
            return PartialView("PartialView/OffcanvasWaitinglist",obj);
        }
    #endregion

    #region GetOffCanvasCustomerDetails
        public async Task<IActionResult> GetOffCanvasCustomerDetails(int SectionId , int CustomerId){
            OffCanvasCustomerDetailViewModel obj = await _tableService.GetCustomerDetailForOfCanvas(SectionId , CustomerId);
            return PartialView("PartialView/OffCanvasCustomerDetail",obj);
        }
    #endregion

    #region GetCustomerDetailFromEmail
        public async Task<IActionResult> GetCustomerDetailFromEmail(int SectionId , string Email){
            OffCanvasCustomerDetailViewModel obj = await _tableService.GetCustomerDetailByEmail(SectionId , Email);
            return PartialView("PartialView/OffCanvasCustomerDetail",obj);
        }
    #endregion

    #region GetDataForAddWaitingToken
        public async Task<IActionResult> GetDataForAddWaitingToken(int SectionId , string Email){
            OffCanvasCustomerDetailViewModel obj = await _tableService.GetCustomerDetailForOfCanvas(SectionId , 0 , Email);
            return PartialView("PartialView/AddWaitingTokenModalPartialView",obj);
        }
    #endregion

    #region AddCustomerToWaitingList
        public async Task<IActionResult> AddCustomerToWaitingList(OffCanvasCustomerDetailViewModel obj){
            if(await _tableService.AddWaitingList(obj)){
                return Json(new {status = true});
            }
            return Json(new {status = false});
        }
    #endregion

    #region AssignTable
        public async Task<IActionResult> AssignTable([FromForm] OffCanvasCustomerDetailViewModel obj , string SelectedTable){
            List<int> tableIds = JsonConvert.DeserializeObject<List<int>>(SelectedTable);
            (int OrderId , int CustomerId , bool status) = await _tableService.AssignTableToCustomer(obj , tableIds);
            if(status == true){
                var redirectUrl = Url.Action("OrderMenu" , "OrderAppMenu" , new {orderId = OrderId , customerId = CustomerId});
                return Json(new {status = true , redirectUrl});
            }
            else{
                return Json(new {status = false});
            }
        }
    #endregion
}
