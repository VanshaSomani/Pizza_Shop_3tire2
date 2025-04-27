
using Microsoft.AspNetCore.Http;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class OrderAppTableService : IOrderAppTableService
{
    private readonly IOrderAppTableRepository _tableRepository;
    private readonly IHttpContextAccessor _httpaccessor;
    public OrderAppTableService(IOrderAppTableRepository tableRepository, IHttpContextAccessor httpaccessor)
    {
        _tableRepository = tableRepository;
        _httpaccessor = httpaccessor;
    }

    #region GetDataForTableAccoordian
    public async Task<OrderAppTableList> GetDataForTableAccoordian()
    {
        OrderAppTableList obj = new OrderAppTableList();
        List<Section> sections = await _tableRepository.GetSectionAndTableList();
        List<OrderAppAccordData> AccordSectionList = new List<OrderAppAccordData>();
        foreach (Section s in sections)
        {
            OrderAppAccordData oa = new OrderAppAccordData();
            oa.SectionId = s.Sectionid;
            oa.SectionName = s.Sectionname;
            oa.Available = s.Stables.Count(t => t.TableStatus == "Available");
            oa.Assigned = s.Stables.Count(t => t.TableStatus == "Assigned");
            oa.Running = s.Stables.Count(t => t.TableStatus == "Running");
            List<OrderAppTableInfo> AccordTableList = new List<OrderAppTableInfo>();
            foreach (Stable t in s.Stables)
            {
                OrderAppTableInfo ot = new OrderAppTableInfo();
                ot.TableId = t.Tableid;
                ot.TableName = t.Tablename;
                ot.TableCapacity = t.Capacity;
                ot.TableStatus = t.TableStatus;
                if(ot.TableStatus != "Available"){
                    ot.AssignTime = (DateTime)t.Updatedat;
                }
                else{
                    ot.AssignTime = DateTime.Now;
                }
                if(ot.TableStatus == "Assigned"){
                    ot.CustomerId = t.OrderTableMappings.First().Customerid;
                    ot.OrderId = (int)t.OrderTableMappings.First().OrderId;
                }
                if(ot.TableStatus == "Running"){
                    ot.CustomerId = t.OrderTableMappings.First().Customerid;
                    ot.OrderId = (int)t.OrderTableMappings.First().OrderId;
                }
                AccordTableList.Add(ot);
            }
            oa.TableList = AccordTableList;
            AccordSectionList.Add(oa);
        }
        obj.TableSectionList = AccordSectionList;
        return obj;
    }
    #endregion

    #region GetWaitingListForOfCanvas
    public async Task<OffCanvasWaitingListViewModel> GetWaitingListForOfCanvas(int SectionId)
    {
        List<Waitinglist> waitinglist = await _tableRepository.GetWaitingListBySectionId(SectionId);
        OffCanvasWaitingListViewModel obj = new OffCanvasWaitingListViewModel();
        List<OffCanvasWaitingData> customerlist = new List<OffCanvasWaitingData>();
        foreach (Waitinglist c in waitinglist)
        {
            OffCanvasWaitingData wc = new OffCanvasWaitingData();
            wc.CustomerId = c.Customerid;
            wc.CustomerName = c.Customer.Customername;
            wc.NoOfPerson = c.Peoples;
            customerlist.Add(wc);
        }
        obj.WaitingList = customerlist;
        return obj;
    }
    #endregion

    #region GetCustomerDetailForOfCanvas
    public async Task<OffCanvasCustomerDetailViewModel> GetCustomerDetailForOfCanvas(int SectionId = -1, int CustomerId = -1, string Email = null)
    {
        List<Section> sections = await _tableRepository.GetSections();
        OffCanvasCustomerDetailViewModel obj = new OffCanvasCustomerDetailViewModel();
        obj.SectionId = SectionId;
        List<OffCanvasCustomerDetailSectionList> sectionDropDown = new List<OffCanvasCustomerDetailSectionList>();
        foreach (Section s in sections)
        {
            OffCanvasCustomerDetailSectionList sd = new OffCanvasCustomerDetailSectionList();
            sd.SectionId = s.Sectionid;
            sd.SectionName = s.Sectionname;
            sectionDropDown.Add(sd);
        }
        obj.SectionList = sectionDropDown;
        if (CustomerId != 0 && CustomerId != -1)
        {
            Customer c = await _tableRepository.GetCustomerAsync(CustomerId);
            obj.SectionId = c.Waitinglists.First().Sectionid;
            obj.CustomerId = c.Customerid;
            obj.CustomerName = c.Customername;
            obj.NoOfPerson = c.Waitinglists.First().Peoples;
            obj.Email = c.Email;
            obj.Phoneno = (double)c.Phoneno;
        }
        if (!string.IsNullOrEmpty(Email))
        {
            Customer c = await _tableRepository.GetCustomerByEmailAsync(Email);
            if (c != null)
            {
                obj.CustomerId = c.Customerid;
                obj.CustomerName = c.Customername;
                obj.Email = c.Email;
                obj.Phoneno = (double)c.Phoneno;
            }
        }
        return obj;
    }
    #endregion

    #region GetCustomerDetailByEmail
    public async Task<OffCanvasCustomerDetailViewModel> GetCustomerDetailByEmail(int SectionId, string Email)
    {
        List<Section> sections = await _tableRepository.GetSections();
        OffCanvasCustomerDetailViewModel obj = new OffCanvasCustomerDetailViewModel();
        obj.SectionId = SectionId;
        List<OffCanvasCustomerDetailSectionList> sectionDropDown = new List<OffCanvasCustomerDetailSectionList>();
        foreach (Section s in sections)
        {
            OffCanvasCustomerDetailSectionList sd = new OffCanvasCustomerDetailSectionList();
            sd.SectionId = s.Sectionid;
            sd.SectionName = s.Sectionname;
            sectionDropDown.Add(sd);
        }
        obj.SectionList = sectionDropDown;
        Customer c = await _tableRepository.GetCustomerWaitingListByEmail(Email);
        if (c != null)
        {
            obj.SectionId = SectionId;
            obj.CustomerId = c.Customerid;
            obj.CustomerName = c.Customername;
            if (c.Waitinglists.Count() != 0)
            {
                obj.NoOfPerson = c.Waitinglists.First().Peoples;
            }
            obj.Email = c.Email;
            obj.Phoneno = (double)c.Phoneno;
        }
        return obj;
    }

    #endregion

    #region AddWaitingList
    public async Task<bool> AddWaitingList(OffCanvasCustomerDetailViewModel obj)
    {
        try
        {
            if (obj.CustomerId == 0)
            {
                Customer newCustomer = new Customer();
                newCustomer.Customername = obj.CustomerName;
                newCustomer.Email = obj.Email;
                newCustomer.Phoneno = (decimal)obj.Phoneno;
                newCustomer.CustDate = DateTime.Now;
                newCustomer.Totalorder = 0;
                newCustomer.Isdeleted = false;
                newCustomer.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                newCustomer.Createdat = DateTime.Now;
                newCustomer = await _tableRepository.AddCustomer(newCustomer);

                Waitinglist w1 = new Waitinglist();
                w1.Customerid = newCustomer.Customerid;
                w1.Peoples = obj.NoOfPerson;
                w1.Sectionid = obj.SectionId;
                w1.Isdeleted = false;
                w1.Createdat = DateTime.Now;
                w1.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                await _tableRepository.AddWaitingList(w1);
            }
            else
            {
                Waitinglist w = new Waitinglist();
                w.Customerid = obj.CustomerId;
                w.Peoples = obj.NoOfPerson;
                w.Sectionid = obj.SectionId;
                w.Isdeleted = false;
                w.Createdat = DateTime.Now;
                w.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                await _tableRepository.AddWaitingList(w);
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

    #region AddTableToOrderTableMapping
    public async Task<(int OrderId, int CustomerId , bool status)> AssignTableToCustomer(OffCanvasCustomerDetailViewModel obj, List<int> TableIds)
    {
        List<int> OrderTableMappingIds = new List<int>();
        try
        {
            int customerId = obj.CustomerId;

            if (obj.CustomerId == 0)
            {
                Customer newCustomer = new Customer();
                newCustomer.Customername = obj.CustomerName;
                newCustomer.Email = obj.Email;
                newCustomer.Phoneno = (decimal)obj.Phoneno;
                newCustomer.CustDate = DateTime.Now;
                newCustomer.Totalorder = 0;
                newCustomer.Isdeleted = false;
                newCustomer.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                newCustomer.Createdat = DateTime.Now;
                newCustomer = await _tableRepository.AddCustomer(newCustomer);
                customerId = newCustomer.Customerid;
            }

            Order order = new(){
                Customerid = customerId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                NoOfPerson = obj.NoOfPerson,
                Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")),
                Createdat = DateTime.Now
            };
            order = await _tableRepository.GenerateOrder(order);

            for (int i = 0; i < TableIds.Count(); i++)
            {
                OrderTableMapping OT = new OrderTableMapping();
                OT.OrderId = order.Orderid;
                OT.TableId = TableIds[i];
                OT.Customerid = customerId;
                OT.Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                OT.Createdat = DateTime.Now;
                OT.Isdeleted = false;
                OT = await _tableRepository.AddDataToOrderTableMapping(OT);

                Stable table = await _tableRepository.GetTableByTableId(TableIds[i]);
                table.TableStatus = "Assigned";
                table.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                table.Updatedat = DateTime.Now;
                await _tableRepository.UpdateTableStatus(table);

                OrderTableMappingIds.Add(OT.OrderTableId);
            }

            Waitinglist w = await _tableRepository.GetWaitinglistByCustomerId(customerId);
            if (w != null)
            {
                w.Isassigned = true;
                w.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                w.Sectionid = obj.SectionId;
                w.Updatedat = DateTime.Now;
                await _tableRepository.UpdateWaitingList(w);
            }

            return (order.Orderid , customerId , true);
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return (-1 , -1 , false);
        }
    }
    #endregion
}
