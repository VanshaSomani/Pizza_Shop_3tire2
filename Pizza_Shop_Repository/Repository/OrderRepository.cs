using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly RmsdemoContext _db;
    public OrderRepository(RmsdemoContext db){
        _db = db;
    }

    #region GetAllOrders
        public async Task<(List<Order> order_list , int TotalRecord)> GetAllOrders(int page , int pageSize){
            IQueryable<Order> OrderList = _db.Orders
            .Include(o => o.Rating)
            .Include(o => o.Customer)
            .Include(o => o.OrderPayment)
            .Where(o => o.Isdeleted != true && o.Customer.Isdeleted != true)
            .OrderBy(o => o.Orderid);

            int totalrecord = await OrderList.CountAsync();
            List<Order> orders = await OrderList.Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

            return (orders , totalrecord);
        }
    #endregion

    #region GetAllOrderForPartialView
        public async Task<(List<Order> order_list , int TotalRecord)> GetAllOrderForPartialView(string SearchCriteria , int page , int pageSize , string Status , string SortBy , bool Desc , string timeDropDown , string FromDate , string ToDate){

            IQueryable<Order> OrderList = _db.Orders
            .Include(o => o.Rating)
            .Include(o => o.Customer)
            .Include(o => o.OrderPayment)
            .Where(o => o.Isdeleted != true && o.Customer.Isdeleted != true)
            .OrderBy(o => o.Orderid);

            if(!string.IsNullOrEmpty(SearchCriteria)){
                OrderList = OrderList
                .Where(o => o.Customer.Customername.ToLower().Contains(SearchCriteria.ToLower())
                || o.Orderid.ToString().Contains(SearchCriteria));
            }

            if(!string.IsNullOrEmpty(Status)){
                OrderList = OrderList
                            .Where(o => o.Status == Status);
            }

            if(!string.IsNullOrEmpty(timeDropDown)){
                OrderList = timeDropDown switch{
                    "7" => OrderList
                            .Where(o => o.OrderDate >= DateTime.Now.AddDays(-7) && o.OrderDate <= DateTime.Now),

                    "30" => OrderList
                            .Where(o => o.OrderDate >= DateTime.Now.AddDays(-30) && o.OrderDate <= DateTime.Now),

                    // "Month" => OrderList
                    //             .Where(o => Int32.Parse(o.OrderDate.ToString("yyyy")) == DateTime.Now.Year && 
                    //             Int32.Parse(o.OrderDate.ToString("MM")) == DateTime.Now.Month),

                    "Month" => OrderList
                                .Where(o => o.OrderDate >= new DateTime(DateTime.Now.Year , DateTime.Now.Month , 1) &&
                                o.OrderDate <= DateTime.Now),

                    _ => OrderList
                };
            }

            if(!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate)){
                OrderList = OrderList
                .Where(o => o.OrderDate.Date >= DateTime.Parse(FromDate) && 
                o.OrderDate.Date <= DateTime.Parse(ToDate));
            }

            if(!string.IsNullOrEmpty(SortBy)){
                OrderList = SortBy switch
                {
                    "Order" => Desc ? 
                                    OrderList
                                    .OrderByDescending(o => o.Orderid) : 
                                    OrderList
                                    .OrderBy(o => o.Orderid),

                    "Date" => Desc ? 
                                    OrderList
                                    .OrderByDescending(o => o.OrderDate) : 
                                    OrderList
                                    .OrderBy(o => o.OrderDate),

                    "Customer" => Desc ? 
                                    OrderList
                                    .OrderByDescending(o => o.Customer.Customername) : 
                                    OrderList
                                    .OrderBy(o => o.Customer.Customername),

                    "Total Amount" => Desc ? 
                                        OrderList
                                        .OrderByDescending(o => o.OrderPayment.TotalAmount) : 
                                        OrderList
                                        .OrderBy(o => o.OrderPayment.TotalAmount),

                    _ => OrderList
                };
            }

            int totalrecord = await OrderList.CountAsync();

            List<Order> orders = await OrderList.Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

            return (orders , totalrecord);
        }
    #endregion

    #region SortOrderList
        public async Task<List<Order>> SortOrderList(string SortBy , bool Desc , List<Order> Order_List){
            Order_List = SortBy switch
            {
                "Order" => Desc ? 
                                Order_List
                                .OrderByDescending(o => o.Orderid)
                                .ToList() : 
                                Order_List
                                .OrderBy(o => o.Orderid)
                                .ToList(),

                "Date" => Desc ? 
                                Order_List
                                .OrderByDescending(o => o.OrderDate)
                                .ToList() : 
                                Order_List
                                .OrderBy(o => o.OrderDate)
                                .ToList(),

                "Customer" => Desc ? 
                                Order_List
                                .OrderByDescending(o => o.Customer.Customername)
                                .ToList() : 
                                Order_List
                                .OrderBy(o => o.Customer.Customername)
                                .ToList(),

                "Total Amount" => Desc ? 
                                    Order_List
                                    .OrderByDescending(o => o.OrderPayment.TotalAmount)
                                    .ToList() : 
                                    Order_List
                                    .OrderBy(o => o.OrderPayment.TotalAmount)
                                    .ToList(),

                _ => Order_List
            };
            return Order_List;
        }
    #endregion

    #region GetOrderByTimeDropDowm
        public async Task<List<Order>> GetOrderByTimeDropDowm(string timeDropDown , List<Order> Order_List){
            Order_List = timeDropDown switch{
                "7" => Order_List
                        .Where(o => o.OrderDate >= DateTime.Now.AddDays(-7) && o.OrderDate <= DateTime.Now)
                        .ToList(),

                "30" => Order_List
                        .Where(o => o.OrderDate >= DateTime.Now.AddDays(-30) && o.OrderDate <= DateTime.Now)
                        .ToList(),

                "Month" => Order_List
                            .Where(o => Int32.Parse(o.OrderDate.ToString("yyyy")) == DateTime.Now.Year && Int32.Parse(o.OrderDate.ToString("MM")) == DateTime.Now.Month)
                            .ToList(),

                _ => Order_List
            };
            return Order_List;
        }
    #endregion

    #region DateInterval
        public async Task<List<Order>> DateInterval(string FromDate , string ToDate , List<Order> Order_List){
            Order_List = Order_List.Where(o => o.OrderDate.Date >= DateTime.Parse(FromDate) && o.OrderDate.Date <= DateTime.Parse(ToDate)).ToList();
            return Order_List;
        }
    #endregion

    #region GetDataForExcel
        public async Task<(List<Order> order_list , int TotalRecord)> GetDataForExcel(string SearchCriteria , string timeDropDown , string Status){
            IQueryable<Order> OrderList = _db.Orders
            .Include(o => o.Rating)
            .Include(o => o.Customer)
            .Include(o => o.OrderPayment)
            .Where(o => o.Isdeleted != true && o.Customer.Isdeleted != true)
            .OrderBy(o => o.Orderid);

            if(!string.IsNullOrEmpty(Status)){
                OrderList = OrderList
                            .Where(o => o.Status == Status);
            }

            if(!string.IsNullOrEmpty(timeDropDown)){
                OrderList = timeDropDown switch{
                    "7" => OrderList
                            .Where(o => o.OrderDate >= DateTime.Now.AddDays(-7) && o.OrderDate <= DateTime.Now),

                    "30" => OrderList
                            .Where(o => o.OrderDate >= DateTime.Now.AddDays(-30) && o.OrderDate <= DateTime.Now),

                    // "Month" => OrderList
                    //             .Where(o => Int32.Parse(o.OrderDate.ToString("yyyy")) == DateTime.Now.Year && 
                    //             Int32.Parse(o.OrderDate.ToString("MM")) == DateTime.Now.Month),

                    "Month" => OrderList
                                .Where(o => o.OrderDate >= new DateTime(DateTime.Now.Year , DateTime.Now.Month , 1) &&
                                o.OrderDate <= DateTime.Now),

                    _ => OrderList
                };
            }

            if(!string.IsNullOrEmpty(SearchCriteria)){
                OrderList = OrderList
                .Where(o => o.Customer.Customername.ToLower().Contains(SearchCriteria.ToLower())
                || o.Orderid.ToString().Contains(SearchCriteria));
            }

            List<Order> orders = await OrderList.ToListAsync();

            return (orders , orders.Count());
        }
    #endregion

    #region GetEntireOrderDetails
        public async Task<Order> GetEntireOrderDetails(int OrderId){
            Order obj = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderPayment)
            .Include(o => o.OrderTableMappings).ThenInclude(ot => ot.Table).ThenInclude(ott => ott.Section)
            .Include(o => o.Invoices)
            .Include(i => i.InvoceTaxes).ThenInclude(it => it.Tax)
            .Include(o => o.OrderItems.Where(oi => oi.Isdeleted != true)).ThenInclude(oi => oi.Item)
            .Include(o => o.OrderItems.Where(oi => oi.Isdeleted != true))
            .ThenInclude(oi => oi.OrderItemModifiers.Where(oim => oim.Isdeleted != true)).ThenInclude(oim => oim.Modifier)
            .FirstOrDefaultAsync(o => o.Orderid == OrderId);

            return obj;
        }
    #endregion

}
