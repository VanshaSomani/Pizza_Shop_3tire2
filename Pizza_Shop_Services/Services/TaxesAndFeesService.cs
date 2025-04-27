using Microsoft.AspNetCore.Http;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class TaxesAndFeesService : ITaxesAndFeesService
{
    private readonly ITaxesAndFeesRepository _taxesAndFeesRepository;
    private readonly IHttpContextAccessor _httpaccessor;

    public TaxesAndFeesService(ITaxesAndFeesRepository taxesAndFeesRepository , IHttpContextAccessor httpaccessor){
        _taxesAndFeesRepository = taxesAndFeesRepository;
        _httpaccessor = httpaccessor;
    }

    #region GetTaxesAndFeesAsync
        public async Task<(TaxesAndFeesViewModel obj , int TaxTotalRecord)> GetTaxesAndFeesAsync(string SearchCriteria , int page , int pageSize){
            TaxesAndFeesViewModel obj = new TaxesAndFeesViewModel();
            (obj.TaxList , int TaxTotalRecord) = await _taxesAndFeesRepository.GetAllTaxes(SearchCriteria , page , pageSize);
            obj.TaxPaggination = new PagginationViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecord = TaxTotalRecord,
                TotalPage = (int)Math.Ceiling((double)TaxTotalRecord / pageSize),
                MinRow = ((page - 1) * pageSize) + 1,
                MaxRow = ((page - 1) * pageSize) + obj.TaxList.Count()
            };
            return (obj , TaxTotalRecord);
        }
    #endregion

    #region AddTaxAsync
        public async Task<bool> AddTaxAsync(TaxesAndFeesViewModel obj){
            try
            {
                Tax new_tax = new Tax();
                new_tax.Taxname = obj.TaxName;
                new_tax.Taxtype = obj.TaxType;
                new_tax.Isenable = obj.IsEnabled;
                new_tax.Defaulttax = obj.DefaultTax;
                new_tax.Taxamount = obj.TaxAmount;
                new_tax.Isdeleted = false;
                new_tax.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                new_tax.Createdat = DateTime.Now;
                return await _taxesAndFeesRepository.AddTax(new_tax);
            }
            catch (Exception)
            {   
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region GetTaxEdit
        public async Task<TaxesAndFeesViewModel> GetTaxEdit(int TaxId){
            Tax t = await _taxesAndFeesRepository.GetTaxById(TaxId);
            TaxesAndFeesViewModel obj = new TaxesAndFeesViewModel();
            obj.TaxId = t.Taxid;
            obj.TaxName = t.Taxname;
            obj.TaxAmount = (int)t.Taxamount;
            obj.TaxType = t.Taxtype;
            obj.IsEnabled = t.Isenable;
            obj.DefaultTax = t.Defaulttax;
            return obj;
        }
    #endregion

    #region UpdateTax
        public async Task<bool> UpdateTax(TaxesAndFeesViewModel obj){
            try
            {
                Tax tax = await _taxesAndFeesRepository.GetTaxById(obj.TaxId);
                tax.Taxname = obj.TaxName;
                tax.Taxtype = obj.TaxType;
                tax.Taxamount = obj.TaxAmount;
                tax.Isenable = obj.IsEnabled;
                tax.Defaulttax = obj.DefaultTax;
                tax.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                tax.Updatedat = DateTime.Now;
                return await _taxesAndFeesRepository.EditTax(tax);
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region DeleteTaxAsync
        public async Task<bool> DeleteTaxAsync(int TaxId){
            try
            {
                Tax tax = await _taxesAndFeesRepository.GetTaxById(TaxId);
                tax.Isdeleted = true;
                tax.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                tax.Updatedat = DateTime.Now;
                return await _taxesAndFeesRepository.DeleteTaxById(tax);
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region ChangeTaxEnabled
        public async Task<bool> ChangeTaxEnabled(int TaxId , bool IsEnable){
            return await _taxesAndFeesRepository.EditTaxEnable(TaxId , IsEnable , Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")));
        }
    #endregion

    #region ChangeDefaultTax
        public async Task<bool> ChangeDefaultTax(int TaxId , bool Default){
            return await _taxesAndFeesRepository.EditDefaultTax(TaxId , Default , Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")));
        }
    #endregion
}
