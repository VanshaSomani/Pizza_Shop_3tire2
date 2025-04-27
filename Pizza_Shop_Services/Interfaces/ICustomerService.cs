using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface ICustomerService
{
    public Task<CustomerListViewModel> GetCustomerDetailsList(int page = 1 , int pageSize = 5);

    public Task<CustomerListViewModel> GetCustomerDetailsListForPartialView(string SortBy , bool Desc ,int page , int pageSize , string SearchCriteria , string FromDate , string ToDate , string timeDropDown);

    public Task<CustomerListViewModel> GetCustomerHistory(int CustomerId);

    public Task<byte[]> ExportAsync(string SearchCriteria , string FromDate , string ToDate , string timeDropDown);

}
