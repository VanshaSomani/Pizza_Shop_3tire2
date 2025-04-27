using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface IOrderRepository
{
    public Task<(List<Order> order_list , int TotalRecord)> GetAllOrders(int page , int pageSize);

    public Task<(List<Order> order_list , int TotalRecord)> GetAllOrderForPartialView(string SearchCriteria , int page , int pageSize , string Status , string SortBy , bool Desc , string timeDropDown , string FromDate , string ToDate);

    public Task<List<Order>> SortOrderList(string SortBy , bool Desc , List<Order> Order_List);

    public Task<List<Order>> DateInterval(string FromDate , string ToDate , List<Order> Order_List);

    public Task<(List<Order> order_list , int TotalRecord)> GetDataForExcel(string SearchCriteria , string timeDropDown , string Status);
    public Task<Order> GetEntireOrderDetails(int OrderId);
}
