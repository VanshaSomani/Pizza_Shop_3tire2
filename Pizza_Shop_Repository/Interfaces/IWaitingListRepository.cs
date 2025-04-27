using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface IWaitingListRepository
{
    public Task<List<Section>> GetAllSection();
    public Task<(List<Waitinglist> waiting_list , int totalrecord)> GetWaitingListBySection(int TotalPage , int page , int PageSize , int SectionId = -1);
    public Task<Waitinglist> GetWaitingDataById(int WaitingTokenId);
    public Task DeleteWaitingList(Waitinglist obj);
    public Task<Customer> CustomerByCustomerId(int CustomerId);
    public Task EditWaitingToken(Waitinglist obj);
    public Task EditCustomer(Customer obj);
    public Task<List<Stable>> GetTableBySectionId(int SectionId);
}
