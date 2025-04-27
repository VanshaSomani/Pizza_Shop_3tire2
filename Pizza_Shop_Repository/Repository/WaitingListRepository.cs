using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class WaitingListRepository : IWaitingListRepository
{
    private readonly RmsdemoContext _db;
    public WaitingListRepository(RmsdemoContext db){
        _db = db;
    }

    #region GetAllSection
        public async Task<List<Section>> GetAllSection(){
            return await _db.Sections
            .Include(s => s.Waitinglists.Where(w => w.Isassigned == false && w.Isdeleted != true))
            .Where(s => s.Isdeleted != true)
            .ToListAsync();
        }
    #endregion

    #region GetWaitingListByCategory
        public async Task<(List<Waitinglist> waiting_list , int totalrecord)> GetWaitingListBySection(int TotalPage , int page , int PageSize , int SectionId = -1)
        {
            IQueryable<Waitinglist> waiting_list = _db.Waitinglists
            .Include(c => c.Customer)
            .Where(w => w.Isassigned == false && w.Isdeleted != true);

            if(SectionId != -1){
                waiting_list = waiting_list
                .Where(w => w.Sectionid == SectionId);
            }

            int WaitingListTotalRecord = await waiting_list.CountAsync();

            List<Waitinglist> Reduced_Waiting_list = await waiting_list.Skip((page - 1) * PageSize).Take(PageSize).OrderBy(w => w.Waitinglistid).ToListAsync();

            return (Reduced_Waiting_list , WaitingListTotalRecord);
        } 
    #endregion

    #region GetWaitingDataById
        public async Task<Waitinglist> GetWaitingDataById(int WaitingTokenId){
            return await _db.Waitinglists
            .Include(w => w.Customer)
            .FirstOrDefaultAsync(w => w.Waitinglistid == WaitingTokenId);
        }
    #endregion

    #region DeleteWaitingList
        public async Task DeleteWaitingList(Waitinglist obj){
            _db.Waitinglists.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region GetWaitingTokenByCustomerId
        public async Task<Customer> CustomerByCustomerId(int CustomerId){
            return await _db.Customers
            .FirstOrDefaultAsync(w => w.Customerid == CustomerId && w.Isdeleted != true);
        }
    #endregion

    #region EditWaitingToken
        public async Task EditWaitingToken(Waitinglist obj){
            _db.Waitinglists.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region EditCustomer
        public async Task EditCustomer(Customer obj){
            _db.Customers.Update(obj);
            await _db.SaveChangesAsync();
        }
    #endregion

    #region Name
        public async Task<List<Stable>> GetTableBySectionId(int SectionId){
            return await _db.Stables.Where(t => t.Sectionid == SectionId && t.Isdeleted != true && t.TableStatus!="Assigned" && t.TableStatus!="Running").ToListAsync();
        }
    #endregion
}
