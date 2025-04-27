using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Presentation.Controllers;

public class OrderAppMenuController : Controller
{
    private readonly IOrderAppMenuService _orderAppMenuService;
    public OrderAppMenuController(IOrderAppMenuService orderAppMenuService){
        _orderAppMenuService = orderAppMenuService;
    }
    #region OrderMenu
        public async Task<IActionResult> OrderMenu(int customerId , int orderId , int CategoryID){
            return View(await _orderAppMenuService.GetDataForMenu(CategoryID , customerId , orderId));
        }
    #endregion

    #region CheckOrderStatus
        [HttpPost]
        public async Task<IActionResult> CheckOrderStatus(int OrderId){
            return Json(new {status = await _orderAppMenuService.GetOrderStatusByOrderId(OrderId)});
        }
    #endregion

    #region GetItemsForViewModel    
        [HttpPost]
        public async Task<IActionResult> GetItemsForViewModel(int CategoryID , int customerId , string SearchData){
            return PartialView("PartialView/OrderAppItemCardPartialView" , await _orderAppMenuService.GetItemList(CategoryID , customerId , SearchData));
        }
    #endregion

    #region MarkItemFavroite
        [HttpPost]
        public async Task<IActionResult> MarkItemFavroite(int CategoryID , int ItemId , bool Isfavourite){
            return Json(new {status = await _orderAppMenuService.EditItemFavroite(CategoryID , ItemId , Isfavourite)});
        }
    #endregion

    #region OpenModifierGroupModal
        public async Task<IActionResult> OpenModifierGroupModal(int ItemId){
            MenuItemModalViewModel obj = await _orderAppMenuService.GetDataForModifierGroupModal(ItemId);
            if(obj.ModifierGroupList.Count != 0){           
                return PartialView("PartialView/ModifierGroupModalPartialView" , obj);
            }
            return Json(new {status = false});
        }
    #endregion
    
    #region GetCustomerDetailForModal
        [HttpGet]
        public async Task<IActionResult> GetCustomerDetailForModal(int CustomerId , int OrderId){
            MenuCustomerDetailViewModel obj = await _orderAppMenuService.GetCustomerDetail(CustomerId , OrderId);
            return PartialView("PartialView/CustomerDetailModalPartialView" , obj);
        }
    #endregion

    #region UpdateCustomerDetails
        [HttpPost]
        public async Task<IActionResult> UpdateCustomerDetails(MenuCustomerDetailViewModel obj){
            if(await _orderAppMenuService.UpdateCustomerInfo(obj)){
                return Json(new {status = true});   
            }
            return Json(new {status = false});
        }
    #endregion

    #region GetOrderCommentForModal
        [HttpGet]
        public async Task<IActionResult> GetOrderCommentForModal(int OrderId){
            OrderCommentViewModel obj = await _orderAppMenuService.GetOrderInstruction(OrderId);
            return PartialView("PartialView/OrderCommentModalPartialView" , obj);
        }
    #endregion

    #region AddOrderComment
        [HttpPost]
        public async Task<IActionResult> AddOrderComment(OrderCommentViewModel obj){
            if(await _orderAppMenuService.AddOrderCommentToOrder(obj)){
                return Json(new {status = true});   
            }
            return Json(new {status = false});
        }        
    #endregion

    #region CheckOrderReadyQuantity
        public async Task<int> CheckOrderReadyQuantity(int OrderItemId){
            return await _orderAppMenuService.GetOrderItemReadyQuantity(OrderItemId);
        }
    #endregion

    #region SaveOrderDetails
        [HttpPost]
        public async Task SaveOrderDetails(string order_id , string order_status , string selected_item , string deleted_item , string enabled_tax){
            int orderid = JsonConvert.DeserializeObject<int>(order_id);
            string orderstatus = JsonConvert.DeserializeObject<string>(order_status);
            List<OrderItemViewModel> save_item = JsonConvert.DeserializeObject<List<OrderItemViewModel>>(selected_item);
            List<int> delete_item = JsonConvert.DeserializeObject<List<int>>(deleted_item);
            List<MenuTaxDataViewModel> tax_list = JsonConvert.DeserializeObject<List<MenuTaxDataViewModel>>(enabled_tax);
            // await _orderAppMenuService.SaveOrder(orderid, orderstatus, save_item, delete_item, tax_list);
        }
    #endregion
}
