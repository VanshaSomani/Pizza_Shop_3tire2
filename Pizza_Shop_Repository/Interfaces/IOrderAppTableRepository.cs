using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Interfaces;

public interface IOrderAppTableRepository
{
    public Task<List<Section>> GetSectionAndTableList();
    public Task<List<Waitinglist>> GetWaitingListBySectionId(int SectionId);
    public Task<List<Section>> GetSections();
    public Task<Customer> GetCustomerAsync(int CustomerId);
    public Task<Customer> GetCustomerByEmailAsync(string Email);
    public Task AddWaitingList(Waitinglist w);
    public Task<Customer> AddCustomer(Customer newCustomer);
    public Task<OrderTableMapping> AddDataToOrderTableMapping(OrderTableMapping obj);
    public Task<Waitinglist> GetWaitinglistByCustomerId(int CustomerId);
    public Task UpdateWaitingList(Waitinglist obj);
    public Task<Order> GenerateOrder(Order obj);
    public Task<Stable> GetTableByTableId(int TableId);
    public Task UpdateTableStatus(Stable obj);
    public Task<Customer> GetCustomerWaitingListByEmail(string Email);
}
