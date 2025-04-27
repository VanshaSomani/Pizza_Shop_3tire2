using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface ITaxesAndFeesRepository
{
    public Task <(List<Tax> tax_list , int TaxTotalRecord)> GetAllTaxes(string SearchCriteria , int page , int pageSize);

    public Task<bool> AddTax(Tax obj);

    public Task<Tax> GetTaxById(int TaxId);

    public Task<bool> EditTax(Tax obj);

    public Task<bool> DeleteTaxById(Tax obj);

    public Task<bool> EditTaxEnable(int TaxId , bool IsEnable , int UserId);

    public Task<bool> EditDefaultTax(int TaxId , bool Default , int UserId);
}
