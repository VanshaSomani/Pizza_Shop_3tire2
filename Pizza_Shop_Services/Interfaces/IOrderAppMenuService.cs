using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IOrderAppMenuService
{
    public Task<OrderAppMenuViewModel> GetDataForMenu(int CategoryID , int customerId , int OrderId);
    public Task<bool> GetOrderStatusByOrderId(int OrderId);
    public Task<OrderAppMenuViewModel> GetItemList(int CategoryID , int customerId , string SearchData);
    public Task<bool> EditItemFavroite(int CategoryID , int ItemId , bool Isfavourite);
    public Task<MenuItemModalViewModel> GetDataForModifierGroupModal(int ItemId);
    public Task<MenuCustomerDetailViewModel> GetCustomerDetail(int CustomerId , int OrderId);
    public Task<bool> UpdateCustomerInfo(MenuCustomerDetailViewModel obj);
    public Task<OrderCommentViewModel> GetOrderInstruction(int OrderId);
    public Task<int> GetOrderItemReadyQuantity(int OrderItemId);
    public Task<bool> AddOrderCommentToOrder(OrderCommentViewModel obj);
    public Task SaveOrder(int orderid, string orderstatus, List<OrderItemViewModel> save_item, List<int> delete_item, List<MenuTaxDataViewModel> tax_list);

}
