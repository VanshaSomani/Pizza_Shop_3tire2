using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;
using Pizza_Shop_Services.Services;

namespace Pizza_Shop_Presentation.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerService _customerService;
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    #region Customer
    [HasPermission("Customer","viewpermission")]
    [Authorize]
    public async Task<IActionResult> Customer()
    {
        CustomerListViewModel obj = await _customerService.GetCustomerDetailsList();
        return View(obj);
    }
    #endregion

    #region GetCustomerForPartialList
    [HasPermission("Customer","viewpermission")]
    [Authorize]
    public async Task<IActionResult> GetCustomerForPartialList(string SortBy, bool Desc, int pageSize, string SearchCriteria, string timeDropDown, string FromDate, string ToDate, int page = 1)
    {
        CustomerListViewModel obj = await _customerService.GetCustomerDetailsListForPartialView(SortBy, Desc, page, pageSize, SearchCriteria, FromDate, ToDate, timeDropDown);
        return PartialView("PartialView/CustomerListPartialView", obj);
    }
    #endregion

    #region CustomerHistory
    [HasPermission("Customer","viewpermission")]
    [Authorize]
    public async Task<IActionResult> CustomerHistory(int CustomerId)
    {
        try
        {
            CustomerListViewModel obj = await _customerService.GetCustomerHistory(CustomerId);
            return PartialView("PartialView/CustomerHistoryModalPartialView", obj);
        }
        catch (Exception)
        {
            return Json(new {status = false});
        }
    }
    #endregion

    #region Export Btn
    [HasPermission("Customer","viewpermission")]
    [Authorize]
    public async Task<IActionResult> Exports(string SearchCriteria, string FromDate, string ToDate, string timeDropDown)
    {
        byte[] customerData = await _customerService.ExportAsync(SearchCriteria, FromDate, ToDate, timeDropDown);
        return File(customerData, "application/vnd.openxmlformats-officedocument.spreadsheetsml.sheet", "OrdersData.xlsx");
    }
    #endregion
}
