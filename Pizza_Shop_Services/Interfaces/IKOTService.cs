using Pizza_Shop_Repository.ViewModels;

namespace Pizza_Shop_Services.Interfaces;

public interface IKOTService
{
    public Task<KOTViewModel> GetDataForKOT();
    public Task<List<KOTCardDetailsViewModel>> GetKOTCardsDetails(int CategoryID = -1 , string status = "In Progress");
    public Task<KOTModalInfoViewModel> GetKotDataByOrderId(int OrderId , string Status , int CategoryID);
    public Task<bool> UpdateKotData(int OrderId , string Status , List<UpdateItem> UpdatedItem);
}
