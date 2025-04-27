using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.Repository;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class TableAndSectionService : ITableAndSectionService
{
    private readonly ITableAndSectionRepository _tableSectionRepository;

    private readonly IHttpContextAccessor _httpaccessor;

    public TableAndSectionService(ITableAndSectionRepository tableSectionRepository, IHttpContextAccessor httpaccessor)
    {
        _tableSectionRepository = tableSectionRepository;
        _httpaccessor = httpaccessor;
    }

    #region GetSectionAsync
    public async Task<(TableSectionViewModel obj, int TableTotalRecord)> GetSectionAsync(int page, int pageSize)
    {
        TableSectionViewModel obj = new TableSectionViewModel();
        obj.SectionList = await _tableSectionRepository.GetAllSections();
        (obj.TableList, int TableTotalRecord) = await _tableSectionRepository.GetAllTableBySection(obj.SectionList.First().Sectionid, null, page, pageSize);
        obj.TablePagination = new PagginationViewModel
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalRecord = TableTotalRecord,
            TotalPage = (int)Math.Ceiling((double)TableTotalRecord / pageSize),
            MinRow = ((page - 1) * pageSize) + 1,
            MaxRow = ((page - 1) * pageSize) + obj.TableList.Count()
        };
        return (obj, TableTotalRecord);
    }
    #endregion

    #region LoadTablesSection
    public async Task<(TableSectionViewModel obj, int TableTotalRecord)> LoadTablesSection(int SectionId, string SearchCriteria, int page, int pageSize)
    {
        TableSectionViewModel obj = new TableSectionViewModel();
        obj.SectionList = await _tableSectionRepository.GetAllSections();
        if (SectionId == 0)
        {
            (obj.TableList, int TableTotalRecord1) = await _tableSectionRepository.GetAllTableBySection(obj.SectionList.First().Sectionid, null, page, pageSize);
            obj.TablePagination = new PagginationViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecord = TableTotalRecord1,
                TotalPage = (int)Math.Ceiling((double)TableTotalRecord1 / pageSize),
                MinRow = ((page - 1) * pageSize) + 1,
                MaxRow = ((page - 1) * pageSize) + obj.TableList.Count()
            };
            return (obj, TableTotalRecord1);
        }
        (obj.TableList, int TableTotalRecord) = await _tableSectionRepository.GetAllTableBySection(SectionId, SearchCriteria, page, pageSize);
        obj.TablePagination = new PagginationViewModel
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalRecord = TableTotalRecord,
            TotalPage = (int)Math.Ceiling((double)TableTotalRecord / pageSize),
            MinRow = ((page - 1) * pageSize) + 1,
            MaxRow = ((page - 1) * pageSize) + obj.TableList.Count()
        };
        return (obj, TableTotalRecord);
    }
    #endregion

    #region AddSectionAsync
    public async Task<bool> AddSectionAsync(TableSectionViewModel obj)
    {
        try
        {
            Section new_section = new Section();
            new_section.Sectionname = obj.SectionName;
            new_section.Sectiondesc = obj.SectionDesc;
            new_section.Isdeleted = false;
            new_section.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            new_section.Createdat = DateTime.Now;
            return await _tableSectionRepository.AddSection(new_section);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region LoadSection
    public async Task<TableSectionViewModel> LoadSection()
    {
        TableSectionViewModel obj = new TableSectionViewModel();
        obj.SectionList = await _tableSectionRepository.GetAllSections();
        return obj;
    }
    #endregion

    #region GetDataSection
    public async Task<TableSectionViewModel> GetDataSection(int SectionId)
    {
        Section section = await _tableSectionRepository.GetSectionDataById(SectionId);
        TableSectionViewModel obj = new TableSectionViewModel();
        obj.SectionId = section.Sectionid;
        obj.SectionName = section.Sectionname;
        obj.SectionDesc = section.Sectiondesc;
        return obj;
    }
    #endregion

    #region UpdateSection
    public async Task<bool> UpdateSection(TableSectionViewModel obj)
    {
        try
        {
            Section section = await _tableSectionRepository.GetSectionDataById(obj.SectionId);
            section.Sectionname = obj.SectionName;
            section.Sectiondesc = obj.SectionDesc;
            section.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            return await _tableSectionRepository.EditSectionData(section);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region DeleteSectionById
    public async Task<bool> DeleteSectionById(TableSectionViewModel obj)
    {
        try
        {
            Section section = await _tableSectionRepository.GetSectionDataById(obj.SectionId);
            section.Isdeleted = true;
            section.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            return await _tableSectionRepository.DeleteSection(section);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region getAllSection
    public async Task<SelectList> getAllSection()
    {
        return new SelectList(await _tableSectionRepository.GetSectionList(), "Sectionid", "Sectionname");
    }
    #endregion

    #region AddTableAsync
    public async Task<bool> AddTableAsync(TableSectionViewModel obj)
    {
        try
        {
            Stable new_table = new Stable();
            new_table.Tablename = obj.TableName;
            new_table.Sectionid = obj.SectionId;
            new_table.Capacity = obj.TableCapacity;
            //change
            new_table.TableStatus = "Available";
            new_table.Isdeleted = false;
            new_table.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            return await _tableSectionRepository.AddTable(new_table);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region Get Table Details
    public async Task<TableSectionViewModel> GetTableEditAsync(int tableId)
    {
        Stable table = await _tableSectionRepository.EditTable(tableId);
        TableSectionViewModel obj = new TableSectionViewModel();
        obj.TableId = table.Tableid;
        obj.SectionId = table.Sectionid;
        obj.TableName = table.Tablename;
        obj.TableCapacity = table.Capacity;
        //change
        obj.TableStatus = table.TableStatus;
        return obj;
    }
    #endregion

    #region EditTableAsync
    public async Task<bool> EditTableAsync(TableSectionViewModel obj)
    {
        try
        {
            Stable table = await _tableSectionRepository.GetTableById(obj.TableId);
            table.Tablename = obj.TableName;
            table.Capacity = obj.TableCapacity;
            table.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            table.Updatedat = DateTime.Now;
            return await _tableSectionRepository.UpdateTable(table);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region DeleteTableAsync
    public async Task<bool> DeleteTableAsync(TableSectionViewModel obj)
    {
        try
        {
            Stable table = await _tableSectionRepository.GetTableById(obj.TableId);
            table.Isdeleted = true;
            table.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            table.Updatedat = DateTime.Now;
            return await _tableSectionRepository.DeleteTable(table);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

    #region MultipleDeleteTable
    public async Task<bool> MultipleDeleteTable(List<int> TableIds, int SectionId)
    {
        try
        {
            for (int i = 0; i < TableIds.Count(); i++)
            {
                try
                {
                    Stable table = await _tableSectionRepository.GetTableById(TableIds[i]);
                    table.Isdeleted = true;
                    table.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                    table.Updatedat = DateTime.Now;
                    await _tableSectionRepository.DeleteTableById(table);
                }
                catch (Exception)
                {
                    // Console.WriteLine(e);
                    return false;
                }
            }
            return true;
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return false;
        }
    }
    #endregion

}
