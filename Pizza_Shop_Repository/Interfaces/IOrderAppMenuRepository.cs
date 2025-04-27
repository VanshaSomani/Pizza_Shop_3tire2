using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface IOrderAppMenuRepository
{
    public Task<List<Category>> GetAllCategories();
    public Task<List<Item>> GetAllItemByCategoryId(int CategoryID , string SearchData = null);
    public Task<List<Tax>> GetAllEnabledTax();
    public Task<Order> GetTaxByOrderId(int OrderId);
    public Task<Order> OrderByOrderId(int OrderId);
    public Task<Order> OrderStatusByOrderId(int OrderId);
    public Task<Item> GetItemByItemId(int ItemId);
    public Task EditItem(Item obj);
    public Task<Item> GetItemModifierGroupMappingByItemId(int ItemId);
    public Task<Order> GetCustomerByOrderId(int OrderId);
    public Task<Customer> GetCustomerByCustomerId(int CustomerId);
    public Task UpdateCustomer(Customer obj);
    public Task UpdateOrder(Order obj);
    public Task<OrderItem> GetOrderItemByOrderItemId(int OrderItemId);
    public Task UpdateOrderItem(OrderItem obj);
    public Task<OrderItem> AddOrderItem(OrderItem obj);
    public Task AddOrderItemModifier(OrderItemModifier obj);
    public Task AddOrderTax(InvoceTax obj);
    public Task<OrderPayment> GetOrderPaymentById(int OrderPaymentId);
    public Task UpdateOrderPayment(OrderPayment obj);
    public Task<OrderPayment> AddOrderPayment(OrderPayment obj);
    public Task<Invoice> AddInvoice(Invoice obj);
}
