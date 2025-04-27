using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class TaxesAndFeesRepository : ITaxesAndFeesRepository
{
    private readonly RmsdemoContext _db;

    public TaxesAndFeesRepository(RmsdemoContext db)
    {
        _db = db;
    }

    #region GetAllTaxes
    public async Task<(List<Tax> tax_list, int TaxTotalRecord)> GetAllTaxes(string SearchCriteria, int page, int pageSize)
    {
        IQueryable<Tax> Tax_list = _db.Taxes
        .Where(t => t.Isdeleted != true);

        if (!string.IsNullOrEmpty(SearchCriteria))
        {
            Tax_list = Tax_list.Where(t => t.Taxname.ToLower().Contains(SearchCriteria.ToLower()));
        }

        int totalrecord = await Tax_list.CountAsync();

        List<Tax> Reduced_Tax_list = await Tax_list.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (Reduced_Tax_list, totalrecord);
    }
    #endregion

    #region AddTax
    public async Task<bool> AddTax(Tax obj)
    {
        try
        {
            _db.Taxes.Add(obj);
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

    #region GetTaxById
    public async Task<Tax> GetTaxById(int TaxId)
    {
        return await _db.Taxes.FirstOrDefaultAsync(t => t.Taxid == TaxId && t.Isdeleted != true);
    }
    #endregion

    #region EditTax
    public async Task<bool> EditTax(Tax obj)
    {
        try
        {
            _db.Taxes.Update(obj);
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

    #region DeleteTaxById
    public async Task<bool> DeleteTaxById(Tax obj)
    {
        try
        {
            _db.Taxes.Update(obj);
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

    #region EditTaxEnable
    public async Task<bool> EditTaxEnable(int TaxId, bool IsEnable, int UserId)
    {
        try
        {
            Tax obj = await _db.Taxes.FirstOrDefaultAsync(t => t.Taxid == TaxId && t.Isdeleted != true);
            obj.Isenable = IsEnable;
            obj.Updatedby = UserId;
            obj.Updatedat = DateTime.Now;
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

    #region EditDefaultTax
    public async Task<bool> EditDefaultTax(int TaxId, bool Default, int UserId)
    {
        try
        {
            Tax obj = await _db.Taxes.FirstOrDefaultAsync(t => t.Taxid == TaxId && t.Isdeleted != true);
            obj.Defaulttax = Default;
            obj.Updatedby = UserId;
            obj.Updatedat = DateTime.Now;
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

}
