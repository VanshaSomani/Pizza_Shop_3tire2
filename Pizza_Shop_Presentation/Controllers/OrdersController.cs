using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Bean;
using Pizza_Shop_Services.Interfaces;
using Pizza_Shop_Services.Services;

namespace Pizza_Shop_Presentation.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    private readonly INotyfService _notyf;

    public OrdersController(IOrderService orderService , INotyfService notyf)
    {
        _orderService = orderService;
        _notyf = notyf;
    }

    #region Order
    [HasPermission("Order","viewpermission")]
    [Authorize]
    public async Task<IActionResult> Order()
    {
        return View(await _orderService.GetOrderListViewModel());
    }
    #endregion

    #region GetOrderPartialView
    [HasPermission("Order","viewpermission")]
    [Authorize]
    public async Task<IActionResult> GetOrderPartialView(string SortBy, bool Desc, string SearchCriteria, string timeDropDown, string FromDate, string ToDate, int page = 1, int pageSize = 5, string Status = null)
    {
        OrderListViewModel obj = await _orderService.GetOrderListViewModelForPartialView(page, pageSize, timeDropDown, SearchCriteria, Status, SortBy, Desc, FromDate, ToDate);
        return PartialView("PartialView/OrderListPartialView", obj);
    }
    #endregion

    #region Export Btn
    [HasPermission("Order","viewpermission")]
    [Authorize]
    public async Task<IActionResult> Exports(string SearchCriteria, string timeDropDown, string Status)
    {
        byte[] orderData = await _orderService.ExportAsync(SearchCriteria, timeDropDown, Status);
        return File(orderData, "application/vnd.openxmlformats-officedocument.spreadsheetsml.sheet", "OrdersData.xlsx");
    }
    #endregion

    //change
    #region ViewOrder
    [HasPermission("Order","viewpermission")]
    [Authorize]
    public async Task<IActionResult> ViewOrder(int OrderId)
    {
        try
        {
            OrderDetailViewModel obj = await _orderService.GetOrderDetailsAsync(OrderId);
            return View(obj);
        }
        catch (Exception)
        {
            _notyf.Error(MessageHelper.GetInvalidErrorMessage(Constant.Order));
            return RedirectToAction("Order" , "Orders");
        }
    }
    #endregion

    //change
    #region ExportInvoiceToPDF
    [HasPermission("Order","viewpermission")]
    [Authorize]
    public async Task<IActionResult> ExportInvoiceToPDF(int OrderId)
    {
        try
        {
            OrderDetailViewModel obj = await _orderService.GetOrderDetailsAsync(OrderId);
            return PartialView("PartialView/DownloadInvoicePartialView", obj);
        }
        catch (Exception)
        {
            return Json(new {status = false});
        }
    }
    #endregion

}
