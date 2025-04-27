using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IOrderService
{
    public Task<OrderListViewModel> GetOrderListViewModel(int page = 1 , int pageSize = 5);

    public Task<OrderListViewModel> GetOrderListViewModelForPartialView(int page , int pageSize , string timeDropDown , string SearchCriteria , string Status , string SortBy , bool Desc , string FromDate , string ToDate);

    public Task<byte[]> ExportAsync(string SearchCriteria , string timeDropDown , string Status);
    public Task<OrderDetailViewModel> GetOrderDetailsAsync(int OrderId);
}
