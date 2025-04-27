using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface IKOTRepository
{
    public Task<List<Category>> GetCategoriesForKot();
    public Task<List<Order>> GetDataForKOTCards(int CategoryID , string status);
    public Task<Order> GetOrderItemDetailByOrderId(int OrderId , string Status , int CategoryID);
    public Task UpdateKotDataByOrderId(int OrderId , string Status , int ItemId , int newQuantity);
}
