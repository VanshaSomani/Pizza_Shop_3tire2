using Microsoft.AspNetCore.Mvc.Rendering;
using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface ITableAndSectionService
{
    public Task<(TableSectionViewModel obj , int TableTotalRecord)> GetSectionAsync(int page , int pageSize);

    public Task<(TableSectionViewModel obj , int TableTotalRecord)> LoadTablesSection(int SectionId , string SearchCriteria , int page , int pageSize);

    public Task<bool> AddSectionAsync(TableSectionViewModel obj);

    public Task<TableSectionViewModel> LoadSection();

    public Task<TableSectionViewModel> GetDataSection(int SectionId);

    public Task<bool> UpdateSection(TableSectionViewModel obj);

    public Task<bool> DeleteSectionById(TableSectionViewModel obj);

    public Task<SelectList> getAllSection();

    public Task<bool> AddTableAsync(TableSectionViewModel obj);

    public Task<TableSectionViewModel> GetTableEditAsync(int tableId);

    public Task<bool> EditTableAsync(TableSectionViewModel obj);

    public Task<bool> DeleteTableAsync(TableSectionViewModel obj);

    public Task<bool> MultipleDeleteTable(List<int>TableIds , int SectionId);
}
