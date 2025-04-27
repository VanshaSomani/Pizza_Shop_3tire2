using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface ITableAndSectionRepository
{
    public Task<List<Section>> GetAllSections();

    public Task<(List<Stable> tableList , int TableTotalRecord)> GetAllTableBySection(int SectionId , string SearchCriteria , int page , int pageSize);

    public Task<bool> AddSection(Section obj);

    public Task<Section> GetSectionDataById(int SectionId);

    public Task<bool> EditSectionData(Section obj);

    public Task<bool> DeleteSection(Section obj);

    public Task<List<Section>> GetSectionList();

    public Task<bool> AddTable(Stable obj);

    public Task<Stable> EditTable(int tableId);

    public Task<Stable> GetTableById(int TableId);

    public Task<bool> UpdateTable(Stable obj);

    public Task<bool> DeleteTable(Stable obj);

    public Task DeleteTableById(Stable obj);
}
