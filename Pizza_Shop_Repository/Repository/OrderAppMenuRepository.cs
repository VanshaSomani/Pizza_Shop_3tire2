using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class OrderAppMenuRepository : IOrderAppMenuRepository
{
    private readonly RmsdemoContext _db;
    public OrderAppMenuRepository(RmsdemoContext db)
    {
        _db = db;
    }

    #region GetAllCategories
        public async Task<List<Category>> GetAllCategories(){
            List<Category> Categories = await _db.Categories.Where(c => c.Isdeleted != true).ToListAsync();
            return Categories;
        }
    #endregion

    #region GetItems
        public async Task<List<Item>> GetAllItemByCategoryId(int CategoryID , string SearchData){
            IQueryable<Item> query = _db.Items.Where(i => i.Isdeleted != true);

            if(CategoryID != 0 && CategoryID != -1 && CategoryID != -2){
                query = query.Where(i => i.Categoryid == CategoryID);
            }
            if(CategoryID == -2){
                query = query.Where(i => i.Isfavourite == true);
            }
            if(!String.IsNullOrEmpty(SearchData)){
                query = query.Where(i => i.Itemname.ToLower().Contains(SearchData.ToLower()));
            }
            List<Item> Items = await query.OrderBy(i => i.Itemid).ToListAsync();
            return Items;
        }
    #endregion

    #region OrderByOrderId
        public async Task<Order> OrderByOrderId(int OrderId){
            return await _db.Orders
            .Include(o => o.OrderTableMappings).ThenInclude(ot => ot.Table).ThenInclude(t => t.Section)
            .Include(o => o.OrderItems.Where(oi => oi.Isdeleted != true)).ThenInclude(oi => oi.Item)
            .Include(o => o.OrderItems.Where(oi => oi.Isdeleted != true))
            .ThenInclude(oi => oi.OrderItemModifiers.Where(oim => oim.Isdeleted != true)).ThenInclude(oim => oim.Modifier)
            .FirstOrDefaultAsync(o => o.Orderid == OrderId);
        }
    #endregion

    #region OrderStatusByOrderId
        public async Task<Order> OrderStatusByOrderId(int OrderId){
            return await _db.Orders.FirstOrDefaultAsync(o => o.Orderid == OrderId && (o.Status == "Pending" || o.Status == "In Progress"));
        }
    #endregion

    #region GetAllDefaultTax
        public async Task<List<Tax>> GetAllEnabledTax(){
            return await _db.Taxes.Where(t => t.Isenable == true && t.Isdeleted != true).ToListAsync();
        }
    #endregion

    #region GetTaxByOrderId
        public async Task<Order> GetTaxByOrderId(int OrderId){
            return await _db.Orders
            .Include(i => i.InvoceTaxes)
            .ThenInclude(i => i.Tax)
            .FirstOrDefaultAsync(i => i.Orderid == OrderId);
        }
    #endregion

    #region GetItemByItemId
        public async Task<Item> GetItemByItemId(int ItemId){
            return await _db.Items.FirstOrDefaultAsync(i => i.Itemid == ItemId && i.Isdeleted != true);
        }
    #endregion

    #region EditItem
        public async Task EditItem(Item obj){
            _db.Items.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region GetItemModifierGroupMappingByItemId
        public async Task<Item> GetItemModifierGroupMappingByItemId(int ItemId){
            Item item = await _db.Items
            .Include(i => i.ItemModifierGroups.Where(im => im.Isdeleted != true))
            .ThenInclude(im => im.ModifierGroup)
            .ThenInclude(mg => mg.ModifierModifierGroupMappings.Where(mmg => mmg.IsDeleted != true))
            .ThenInclude(mmg => mmg.Modifier)
            .FirstOrDefaultAsync(i => i.Itemid == ItemId);
            return item;
        }
    #endregion

    #region GetCustomerByOrderId
        public async Task<Order> GetCustomerByOrderId(int OrderId){
            return await _db.Orders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Orderid == OrderId);
        }
    #endregion

    #region GetCustomerByCustomerId
        public async Task<Customer> GetCustomerByCustomerId(int CustomerId){
            return await _db.Customers.FirstOrDefaultAsync(c => c.Customerid == CustomerId);
        }
    #endregion

    #region UpdateCustomer
        public async Task UpdateCustomer(Customer obj){
            _db.Customers.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region UpdateOrder
        public async Task UpdateOrder(Order obj){
            _db.Orders.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region GetOrderItemByOrderItemId
        public async Task<OrderItem> GetOrderItemByOrderItemId(int OrderItemId){
            return await _db.OrderItems
            .Include(oi => oi.OrderItemModifiers.Where(oim => oim.Isdeleted != true))
            .FirstOrDefaultAsync(oi => oi.Orderitemid == OrderItemId && oi.Isdeleted != true);
        }
    #endregion

    #region UpdateOrderItem
        public async Task UpdateOrderItem(OrderItem obj){
            _db.OrderItems.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region AddOrderItem
        public async Task<OrderItem> AddOrderItem(OrderItem obj){
            await _db.OrderItems.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
    #endregion

    #region AddOrderItemModifier
        public async Task AddOrderItemModifier(OrderItemModifier obj){
            await _db.OrderItemModifiers.AddAsync(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region AddOrderTax
        public async Task AddOrderTax(InvoceTax obj){
            await _db.InvoceTaxes.AddAsync(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region GetOrderPaymentById
        public async Task<OrderPayment> GetOrderPaymentById(int OrderPaymentId){
            return await _db.OrderPayments.FirstOrDefaultAsync(op => op.PaymentId == OrderPaymentId);
        }
    #endregion

    #region UpdateOrderPayment
        public async Task UpdateOrderPayment(OrderPayment obj){
            _db.OrderPayments.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion
    
    #region AddOrderPayment
        public async Task<OrderPayment> AddOrderPayment(OrderPayment obj){
            await _db.OrderPayments.AddAsync(obj);
            await _db.SaveChangesAsync(); 
            return obj;       
        }
    #endregion

    #region AddInvoice
        public async Task<Invoice> AddInvoice(Invoice obj){
            await _db.Invoices.AddAsync(obj);
            await _db.SaveChangesAsync(); 
            return obj;   
        }
    #endregion
}
