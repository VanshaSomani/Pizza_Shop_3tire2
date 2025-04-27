using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface ICustomerRepository
{
    public Task<(List<Customer> CustomerList , int TotalRecord)> GetCustomerList(int page , int pageSize);

    public Task<(List<Customer> CustomerList , int TotalRecord)> GetCustomerListForPartialView(string SortBy , bool Desc ,int page, int pageSize  , string timeDropDown , string FromDate , string ToDate , string SearchCriteria = null);

    public Task<List<Customer>> GetCustomerByTimeDropDown(string timeDropDown , List<Customer> CustomerList);

    public Task<List<Customer>> CustomerDateInterval(string FromDate , string ToDate , List<Customer> CustomerList);

    public Task<Customer> GetCustomerDetailsByCustomerId(int CustomerId);

    public Task<List<Customer>> SortCustomerList(string SortBy , bool Desc , List<Customer> CustomerList);

    public Task<(List<Customer> CustomerList , int TotalRecord)> GetDataForExcel(string SearchCriteria , string FromDate , string ToDate , string timeDropDown);
}
