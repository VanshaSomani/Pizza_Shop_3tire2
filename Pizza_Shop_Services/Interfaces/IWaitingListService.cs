using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IWaitingListService
{
    public Task<WaitingListViewModel> GetDataForWaitingList(int SectionId , int TotalPage = 0, int page = 1 , int PageSize = 5);
    public Task<WaitingListViewModel> GetDataForWaitingListPartialView(int SectionId  , int TotalPage , int page = 1 , int PageSize = 5);
    public Task<AssignWaitingTokenViewModel> GetDataForAssignWaitingToken(int SectionId ,  int WaitingId);
    public Task<bool> DeleteWaitingAsync(int WaitingTokenId);
    public Task<OffCanvasCustomerDetailViewModel> GetWaitingListForEdit(int WaitingListId);
    public Task<bool> EditWaitingTokenAsync(OffCanvasCustomerDetailViewModel obj);
    public Task<bool> AssignTableToCustomer(AssignWaitingTokenViewModel obj , List<int> TableIds);
}
