using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class OrderAppTableRepository : IOrderAppTableRepository
{
    private readonly RmsdemoContext _db;
    public OrderAppTableRepository(RmsdemoContext db){
        _db = db;
    }

    #region GetSectionAndTableList
        public async Task<List<Section>> GetSectionAndTableList(){
            List<Section> Sections = await _db.Sections
            .Include(s => s.Stables.Where(t => t.Isdeleted != true))
            .ThenInclude(t => t.OrderTableMappings.Where(t => t.Isdeleted != true))
            .Where(s => s.Isdeleted != true)
            .OrderBy(s => s.Sectionid)
            .ToListAsync();

            return Sections;
        }
    #endregion

    #region GetWaitingListBySectionId
        public async Task<List<Waitinglist>> GetWaitingListBySectionId(int SectionId){
            List<Waitinglist> waitinglist = await _db.Waitinglists
            .Include(w => w.Customer)
            .Where(w => w.Sectionid == SectionId && 
            w.Isdeleted != true && 
            w.Isassigned == false)
            .ToListAsync();

            return waitinglist;
        }
    #endregion 

    #region GetSections
        public async Task<List<Section>> GetSections(){
            return await _db.Sections
            .Where(s => s.Isdeleted != true)
            .ToListAsync();
        }
    #endregion

    #region GetCustomerAsync
        public async Task<Customer> GetCustomerAsync(int CustomerId){
            Customer c = await _db.Customers
            .Include(c => c.Waitinglists.Where(w => w.Isassigned == false))
            .FirstOrDefaultAsync(c => c.Customerid == CustomerId);
            return c;
        }
    #endregion

    #region GetCustomerByEmailAsync
        public async Task<Customer> GetCustomerByEmailAsync(string Email){
            Customer c = await _db.Customers.FirstOrDefaultAsync(c => c.Email == Email);
            return c;
        }
    #endregion

    #region GetCustomerWaitingListByEmail
        public async Task<Customer> GetCustomerWaitingListByEmail(string Email){
            Customer c = await _db.Customers
            .Include(c => c.Waitinglists.Where(w => w.Isassigned == false))
            .FirstOrDefaultAsync(c => c.Email == Email);
            return c;
        }
    #endregion

    #region AddWaitingList
        public async Task AddWaitingList(Waitinglist w){
            await _db.Waitinglists.AddAsync(w);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region AddCustomer
        public async Task<Customer> AddCustomer(Customer newCustomer){
            await _db.Customers.AddAsync(newCustomer);
            await _db.SaveChangesAsync();
            return newCustomer;
        }
    #endregion

    #region AddDataToOrderTableMapping
        public async Task<OrderTableMapping> AddDataToOrderTableMapping(OrderTableMapping obj){
            await _db.OrderTableMappings.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
    #endregion

    #region GetWaitinglistByCustomerId
        public async Task<Waitinglist> GetWaitinglistByCustomerId(int CustomerId){
            return await _db.Waitinglists.FirstOrDefaultAsync(w => w.Customerid == CustomerId 
            && w.Isassigned == false 
            && w.Isdeleted != true);

        }
    #endregion

    #region UpdateWaitingList
        public async Task UpdateWaitingList(Waitinglist obj){
            _db.Waitinglists.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region GenerateOrderByCustomerId
        public async Task<Order> GenerateOrder(Order obj){
            await _db.Orders.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
    #endregion

    #region GetTableByTableId
        public async Task<Stable> GetTableByTableId(int TableId){
            return await _db.Stables.FirstOrDefaultAsync(t => t.Tableid == TableId && t.Isdeleted != true);
        }
    #endregion

    #region UpdateTableStatus
        public async Task UpdateTableStatus(Stable obj){
            _db.Stables.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion
}
