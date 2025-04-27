using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IOrderExcelService
{
    public Task<byte[]> ExportDataToExcel(OrderListViewModel obj , string SearchCriteria , string timeDropDown , string Status);
}
