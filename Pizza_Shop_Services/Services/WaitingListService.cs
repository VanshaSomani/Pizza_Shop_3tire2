
using Microsoft.AspNetCore.Http;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class WaitingListService : IWaitingListService
{
    private readonly IWaitingListRepository _waitingRepository;
    private readonly IOrderAppTableRepository _tableRepository;
    private readonly IHttpContextAccessor _httpaccessor;
    public WaitingListService(IWaitingListRepository waitingRepository , IHttpContextAccessor httpaccessor , IOrderAppTableRepository tableRepository){
        _waitingRepository = waitingRepository;
        _httpaccessor = httpaccessor;
        _tableRepository = tableRepository;
    }

    #region GetDataForWaitingList
        public async Task<WaitingListViewModel> GetDataForWaitingList(int SectionId , int TotalPage , int page = 1 , int PageSize = 5){
            List<Section> sections = await _waitingRepository.GetAllSection();
            WaitingListViewModel obj = new WaitingListViewModel();
            List<WaitingListSection> section_list = new List<WaitingListSection>();
            int totalCount = 0;
            foreach(Section s in sections){
                WaitingListSection ws = new WaitingListSection();
                ws.SectionId = s.Sectionid;
                ws.SectionName = s.Sectionname;
                ws.WaitingCount = s.Waitinglists.Count();
                totalCount += s.Waitinglists.Count();
                section_list.Add(ws);
            }
            obj.SectionList = section_list;
            obj.TotalWaitingCount = totalCount;

            (List<Waitinglist> waitings , int totalrecord) = await _waitingRepository.GetWaitingListBySection(TotalPage , page , PageSize , SectionId);
            List<WaitingData> waiting_list = new List<WaitingData>();
            foreach(Waitinglist w in waitings){
                WaitingData wd = new WaitingData();
                wd.WaitingListId = w.Waitinglistid;
                wd.SectionId = w.Sectionid;
                wd.CreatedAt = (DateTime)w.Createdat;
                wd.CustomerName = w.Customer.Customername;
                wd.NoOfPerson = w.Peoples;
                wd.PhoneNo = (double)w.Customer.Phoneno;
                wd.Email = w.Customer.Email;
                waiting_list.Add(wd);
            }
            obj.WaitingDataList = waiting_list;

            obj.WaitingListPaggination = new PagginationViewModel{
                CurrentPage = page,
                PageSize = PageSize,
                TotalRecord = totalrecord,
                TotalPage = (int)Math.Ceiling((double)totalrecord / PageSize),
                MinRow = ((page - 1) * PageSize) + 1,
                MaxRow = ((page - 1) * PageSize) + obj.WaitingDataList.Count()
            };

            return obj;
        }
    #endregion

    #region GetDataForWaitingListPartialView
        public async Task<WaitingListViewModel> GetDataForWaitingListPartialView(int SectionId  , int TotalPage , int page = 1 , int PageSize = 5){
            WaitingListViewModel obj = new WaitingListViewModel();
            List<Section> sections = await _waitingRepository.GetAllSection();
            List<WaitingListSection> section_list = new List<WaitingListSection>();
            int totalCount = 0;
            foreach(Section s in sections){
                WaitingListSection ws = new WaitingListSection();
                ws.SectionId = s.Sectionid;
                ws.SectionName = s.Sectionname;
                ws.WaitingCount = s.Waitinglists.Count();
                totalCount += s.Waitinglists.Count();
                section_list.Add(ws);
            }
            obj.SectionList = section_list;
            obj.TotalWaitingCount = totalCount;

            (List<Waitinglist> waitings , int totalrecord) = await _waitingRepository.GetWaitingListBySection(TotalPage , page , PageSize , SectionId); 
            List<WaitingData> waiting_list = new List<WaitingData>();
            foreach(Waitinglist w in waitings){
                WaitingData wd = new WaitingData();
                wd.WaitingListId = w.Waitinglistid;
                wd.SectionId = w.Sectionid;
                wd.CreatedAt = (DateTime)w.Createdat;
                wd.CustomerName = w.Customer.Customername;
                wd.NoOfPerson = w.Peoples;
                wd.PhoneNo = (double)w.Customer.Phoneno;
                wd.Email = w.Customer.Email;
                waiting_list.Add(wd);
            }
            obj.WaitingDataList = waiting_list;

            obj.WaitingListPaggination = new PagginationViewModel{
                CurrentPage = page,
                PageSize = PageSize,
                TotalRecord = totalrecord,
                TotalPage = (int)Math.Ceiling((double)totalrecord / PageSize),
                MinRow = ((page - 1) * PageSize) + 1,
                MaxRow = ((page - 1) * PageSize) + obj.WaitingDataList.Count()
            };

            return obj;
        }
    #endregion

    #region GetDataForAssignWaitingToken
        public async Task<AssignWaitingTokenViewModel> GetDataForAssignWaitingToken(int SectionId ,  int WaitingId){
            List<Section> sections = await _waitingRepository.GetAllSection();
            Waitinglist waiting = await _waitingRepository.GetWaitingDataById(WaitingId);
            AssignWaitingTokenViewModel obj = new(){
                WaitingListID = WaitingId,
                WaitingNoOfPerson = waiting.Peoples,
                SectionId = SectionId,
                CustomerId = waiting.Customerid,
                SectionList = sections.Select(s => new SectionData{
                    SectionId = s.Sectionid,
                    SectionName = s.Sectionname
                }).ToList()
            };      
            if(SectionId != -1 && SectionId != 0){
                List<Stable> table = await _waitingRepository.GetTableBySectionId(SectionId);
                obj.TableList = table.Select(t => new TableData{
                    TableId = t.Tableid,
                    TableName = t.Tablename,
                    TableCapacity = t.Capacity
                }).ToList();
            }
            return obj;
        }
    #endregion

    #region AssignTableToCustomer
        public async Task<bool> AssignTableToCustomer(AssignWaitingTokenViewModel obj , List<int> TableIds){
            try
            {
                Order order = new(){
                    Customerid = obj.CustomerId,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    NoOfPerson = obj.WaitingNoOfPerson,
                    Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")),
                    Createdat = DateTime.Now
                };
                order = await _tableRepository.GenerateOrder(order);
                for (int i = 0; i < TableIds.Count; i++)
                {
                    OrderTableMapping OT = new OrderTableMapping();
                    OT.OrderId = order.Orderid;
                    OT.TableId = TableIds[i];
                    OT.Customerid = obj.CustomerId;
                    OT.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                    OT.Createdat = DateTime.Now;
                    OT.Isdeleted = false;
                    OT = await _tableRepository.AddDataToOrderTableMapping(OT);

                    Stable table = await _tableRepository.GetTableByTableId(TableIds[i]);
                    table.TableStatus = "Assigned";
                    table.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                    table.Updatedat = DateTime.Now;
                    await _tableRepository.UpdateTableStatus(table);
                }
                Waitinglist w = await _waitingRepository.GetWaitingDataById(obj.WaitingListID);
                if (w != null)
                {
                    w.Isassigned = true;
                    w.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                    w.Sectionid = obj.SectionId;
                    w.Updatedat = DateTime.Now;
                    await _tableRepository.UpdateWaitingList(w);
                }
                return true;
            }
            catch (Exception)
            {
                return false;  
            }
        }
    #endregion

    #region DeleteWaitingAsync
        public async Task<bool> DeleteWaitingAsync(int WaitingTokenId){
            try
            {
                Waitinglist obj = await _waitingRepository.GetWaitingDataById(WaitingTokenId);
                obj.Isdeleted = true;
                obj.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                obj.Updatedat = DateTime.Now;
                await _waitingRepository.DeleteWaitingList(obj);
                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

    #region GetWaitingListForEdit
        public async Task<OffCanvasCustomerDetailViewModel> GetWaitingListForEdit(int WaitingListId){
            List<Section> sections = await _tableRepository.GetSections();
            OffCanvasCustomerDetailViewModel obj = new OffCanvasCustomerDetailViewModel();
            List<OffCanvasCustomerDetailSectionList> sectionDropDown = new List<OffCanvasCustomerDetailSectionList>();
            foreach(Section s in sections){
                OffCanvasCustomerDetailSectionList sd = new OffCanvasCustomerDetailSectionList();
                sd.SectionId = s.Sectionid;
                sd.SectionName = s.Sectionname; 
                sectionDropDown.Add(sd);
            }
            obj.SectionList = sectionDropDown;
            Waitinglist waiting = await _waitingRepository.GetWaitingDataById(WaitingListId);
            obj.WaitingTokenId = waiting.Waitinglistid;
            obj.SectionId = waiting.Sectionid;
            obj.CustomerId = waiting.Customerid;
            obj.CustomerName = waiting.Customer.Customername;
            obj.NoOfPerson = waiting.Peoples;
            obj.Email = waiting.Customer.Email;
            obj.Phoneno = (double)waiting.Customer.Phoneno;

            return obj;
        }
    #endregion

    #region EditWaitingTokenAsync
        public async Task<bool> EditWaitingTokenAsync(OffCanvasCustomerDetailViewModel obj){
            try
            {
                Waitinglist w = await _waitingRepository.GetWaitingDataById(obj.WaitingTokenId);
                w.Peoples = obj.NoOfPerson;
                w.Sectionid = obj.SectionId;
                w.Customer.Customername = obj.CustomerName;
                w.Updatedat = DateTime.Now;
                w.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                await _waitingRepository.EditWaitingToken(w);

                Customer c = await _waitingRepository.CustomerByCustomerId(obj.CustomerId);
                if(c.Customername.ToLower() != obj.CustomerName.ToLower() && c.Phoneno != (decimal)obj.Phoneno){
                    c.Customername = obj.CustomerName;
                    c.Phoneno = (decimal)obj.Phoneno;
                    c.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                    await _waitingRepository.EditCustomer(c);
                }

                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(e);
                return false;
            }
        }
    #endregion

}
