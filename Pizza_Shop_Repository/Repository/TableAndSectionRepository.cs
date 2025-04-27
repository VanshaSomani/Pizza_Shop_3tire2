using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class TableAndSectionRepository : ITableAndSectionRepository
{
    private readonly RmsdemoContext _db;

    public TableAndSectionRepository(RmsdemoContext db){
        _db = db;
    }

    #region GetAllSections
        public async Task<List<Section>> GetAllSections(){
            List<Section> section_list = await _db.Sections.Where(s => s.Isdeleted != true).ToListAsync();
            return section_list;
        }
    #endregion

    #region GetAllTableBySection
        public async Task<(List<Stable> tableList , int TableTotalRecord)> GetAllTableBySection(int SectionId , string SearchCriteria  , int page , int pageSize){
            IQueryable<Stable> table_list = _db.Stables
            .Where(t => t.Sectionid == SectionId && t.Isdeleted != true);

            if(!string.IsNullOrEmpty(SearchCriteria)){
                table_list = table_list.Where(t => t.Tablename.ToLower().Contains(SearchCriteria.ToLower()));
            }

            int totalrecord = await table_list.CountAsync();

            List<Stable> reduced_table_list = await table_list
            .Skip((page -1)*pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (reduced_table_list , totalrecord);
        }
    #endregion

    #region AddSection
        public async Task<bool> AddSection(Section obj){
            try
            {
                _db.Sections.Add(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region GetSectionDataById
        public async Task<Section> GetSectionDataById(int SectionId){
            return await _db.Sections.FirstOrDefaultAsync(s => s.Sectionid == SectionId && s.Isdeleted != true);
        }
    #endregion

    #region EditSectionData
        public async Task<bool> EditSectionData(Section obj){
            try
            {
                _db.Sections.Update(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }   
    #endregion

    #region DeleteSection
        public async Task<bool> DeleteSection(Section obj){
            try
            {
                _db.Sections.Update(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region GetSectionList
        public async Task<List<Section>> GetSectionList(){
            return await _db.Sections.Where(s => s.Isdeleted != true).ToListAsync();
        }
    #endregion

    #region AddTable
        public async Task<bool> AddTable(Stable obj){
            try
            {
                _db.Stables.Add(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region EditTable
        public async Task<Stable> EditTable(int tableId){
            Stable table = await _db.Stables.FirstOrDefaultAsync(t => t.Tableid == tableId);
            return table;
        }
    #endregion

    #region GetTableById
        public async Task<Stable> GetTableById(int TableId){
            return await _db.Stables.FirstOrDefaultAsync(t => t.Tableid == TableId && t.Isdeleted != true);
        }
    #endregion

    #region UpdateTable
        public async Task<bool> UpdateTable(Stable obj){
            try
            {
                _db.Stables.Update(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }

        }
    #endregion

    #region DeleteTable
        public async Task<bool> DeleteTable(Stable obj){
            try
            {
                _db.Stables.Update(obj);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region DeleteTableById
        public async Task DeleteTableById(Stable obj){
            try
            {
                _db.Stables.Update(obj);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
            }
        }
    #endregion

}
