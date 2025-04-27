using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface ITaxesAndFeesService
{
    public Task<(TaxesAndFeesViewModel obj , int TaxTotalRecord)> GetTaxesAndFeesAsync(string SearchCriteria , int page , int pageSize);

    public Task<bool> AddTaxAsync(TaxesAndFeesViewModel obj);

    public Task<TaxesAndFeesViewModel> GetTaxEdit(int TaxId); 

    public Task<bool> UpdateTax(TaxesAndFeesViewModel obj);

    public Task<bool> DeleteTaxAsync(int TaxId);

    public Task<bool> ChangeTaxEnabled(int TaxId , bool IsEnable);

    public Task<bool> ChangeDefaultTax(int TaxId , bool Default);
}
