using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IOrderAppTableService
{
    public Task<OrderAppTableList> GetDataForTableAccoordian();
    public Task<OffCanvasWaitingListViewModel> GetWaitingListForOfCanvas(int SectionId);
    public Task<OffCanvasCustomerDetailViewModel> GetCustomerDetailForOfCanvas(int SectionId = -1 , int CustomerId = -1 , string Email = null);
    public Task<bool> AddWaitingList(OffCanvasCustomerDetailViewModel obj);
    public Task<(int OrderId , int CustomerId, bool status)> AssignTableToCustomer(OffCanvasCustomerDetailViewModel obj , List<int> TableIds);
    public Task<OffCanvasCustomerDetailViewModel> GetCustomerDetailByEmail(int SectionId , string Email);
}
