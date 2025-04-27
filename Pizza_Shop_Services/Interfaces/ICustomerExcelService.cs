using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface ICustomerExcelService
{
    public Task<byte[]> ExportDataToExcel(CustomerListViewModel obj , string SearchCriteria , string FromDate , string ToDate , string timeDropDown);
}
