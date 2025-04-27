using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class KOTRepository : IKOTRepository
{
    private readonly RmsdemoContext _db;

    public KOTRepository(RmsdemoContext db){
        _db = db;
    }

    #region GetCategoriesForKot
        public async Task<List<Category>> GetCategoriesForKot(){
            return await _db.Categories.Where(c => c.Isdeleted != true).ToListAsync();
        }
    #endregion

    //change
    #region GetDataForKOTCards
        public async Task<List<Order>> GetDataForKOTCards(int CategoryID , string status){
            List<Order> order_List = new List<Order>();
            order_List =  await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderTableMappings).ThenInclude(ot => ot.Table).ThenInclude(ott => ott.Section)
            .Include(o => o.OrderItems.Where(oi => oi.Isdeleted != true)).ThenInclude(oi => oi.Item)
            .Include(o => o.OrderItems.Where(oi => oi.Isdeleted != true))
            .ThenInclude(oo => oo.OrderItemModifiers.Where(oim => oim.Isdeleted != true)).ThenInclude(oom => oom.Modifier)
            .Where(o => o.Isdeleted != true)
            .ToListAsync();
            foreach(Order order in order_List){
                order.OrderItems = order.OrderItems.Where(oi => oi.Status == status).ToList();
            }
            order_List = order_List.Where(o => o.OrderItems.Any()).ToList();

            if(CategoryID != -1){
                foreach(Order order in order_List){
                    order.OrderItems = order.OrderItems.Where(oi => oi.Item != null && oi.Item.Categoryid == CategoryID).ToList();
                }
                order_List = order_List.Where(o => o.OrderItems.Any()).ToList();
            }

            return order_List;
        }
    #endregion

    #region GetOrderItemDetailByOrderId
        public async Task<Order> GetOrderItemDetailByOrderId(int OrderId , string Status , int CategoryID){

            Order order = await _db.Orders
            .Include(o => o.OrderItems.Where(oi => oi.Isdeleted != true)).ThenInclude(oi => oi.Item)
            .Include(o => o.OrderItems.Where(oi => oi.Isdeleted != true))
            .ThenInclude(oo => oo.OrderItemModifiers.Where(oim => oim.Isdeleted != true)).ThenInclude(oom => oom.Modifier)
            .FirstOrDefaultAsync(o => o.Orderid == OrderId);

            order.OrderItems = order.OrderItems.Where(oi => oi.Status == Status).ToList(); 

            if(CategoryID != -1){
                order.OrderItems = order.OrderItems.Where(oi => oi.Item != null && oi.Item.Categoryid == CategoryID).ToList();
            }
            
            return order;
        }
    #endregion

    #region UpdateKotDataByOrderId
        public async Task UpdateKotDataByOrderId(int OrderId , string Status , int ItemId , int newQuantity){
            OrderItem oi = await _db.OrderItems
            .FirstOrDefaultAsync(oi => oi.Orderid == OrderId && oi.Status == Status && oi.Itemid == ItemId && oi.Isdeleted != true);

            if(Status == "In Progress"){
                oi.ReadyItem = newQuantity;
                if(oi.ReadyItem == oi.Quantity){
                    oi.Status = "Ready";
                }
                await _db.SaveChangesAsync();
            }

            if(Status == "Ready"){
                oi.ReadyItem = oi.ReadyItem - newQuantity;
                oi.Status = "In Progress";
                await _db.SaveChangesAsync();
            }
        }
    #endregion
}
