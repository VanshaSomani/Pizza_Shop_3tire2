using Microsoft.AspNetCore.Mvc;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Presentation.Controllers;

public class OrderAppKOTController : Controller
{
    private readonly IKOTService _kotService;
    public OrderAppKOTController(IKOTService kotService)
    {
        _kotService = kotService;
    }


    #region KOTView
    public async Task<IActionResult> KOT()
    {
        KOTViewModel obj = await _kotService.GetDataForKOT();
        obj.KOTCardData = await _kotService.GetKOTCardsDetails();
        return View(obj);
        // return PartialView("PartialView/OrderAppKOTPartialView" , obj);
    }
    #endregion

    #region
    public async Task<IActionResult> GetCategorywiseDataForKotCards(int CategoryID, string Status)
    {
        KOTViewModel obj = new KOTViewModel();
        obj.KOTCardData = await _kotService.GetKOTCardsDetails(CategoryID, Status);
        return PartialView("PartialView/KOTCardsPartialView", obj);
    }
    #endregion

    #region GetKotModalInfo
    public async Task<IActionResult> GetKotModalInfo(int OrderId, string Status, int CategoryID)
    {
        KOTModalInfoViewModel obj = await _kotService.GetKotDataByOrderId(OrderId, Status, CategoryID);
        return PartialView("PartialView/KOTModalPartialView", obj); ;
    }
    #endregion

    #region UpdateKOTItemInfo
    public async Task<IActionResult> UpdateKOTItemInfo(int OrderId, string Status, List<UpdateItem> UpdatedItem)
    {
        if (await _kotService.UpdateKotData(OrderId, Status, UpdatedItem))
        {
            return Json(new { status = true });
        }
        return Json(new { status = false });
        // return Json(new { status = await _kotService.UpdateKotData(OrderId, Status, UpdatedItem) });
    }
    #endregion
}
