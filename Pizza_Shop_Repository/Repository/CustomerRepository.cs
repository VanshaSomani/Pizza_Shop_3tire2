using Microsoft.EntityFrameworkCore;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly RmsdemoContext _db;
    public CustomerRepository(RmsdemoContext db){
        _db = db;
    }

    #region GetCustomerList
        public async Task<(List<Customer> CustomerList , int TotalRecord)> GetCustomerList(int page , int pageSize){
            IQueryable<Customer> CustomerList = _db.Customers
            .Include(c => c.Orders)
            .Where(c => c.Isdeleted != true)
            .OrderBy(c => c.Customername);

            int totalrecord = await CustomerList.CountAsync();

            List<Customer> customerData = await CustomerList.Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

            return (customerData , totalrecord);
        }
    #endregion

    #region GetCustomerListForPartialView
        public async Task<(List<Customer> CustomerList , int TotalRecord)> GetCustomerListForPartialView(string SortBy , bool Desc ,int page , int pageSize , string timeDropDown , string FromDate , string ToDate , string SearchCriteria){
            IQueryable<Customer> CustomerList = _db.Customers
            .Include(c => c.Orders)
            .Where(c => c.Isdeleted != true)
            .OrderBy(c => c.Customername);

            if(!string.IsNullOrEmpty(SearchCriteria)){
                CustomerList = CustomerList.Where(c =>  c.Customername.ToLower().Contains(SearchCriteria.ToLower()));
            }

            if(!string.IsNullOrEmpty(timeDropDown)){
                CustomerList = timeDropDown switch{
                    "7" => CustomerList
                            .Where(c => c.Orders.Select(o => o.OrderDate).Max() >= DateTime.Now.AddDays(-7) && 
                            c.Orders.Select(o => o.OrderDate).Max() <= DateTime.Now),

                    "30" => CustomerList
                            .Where(c => c.Orders.Select(o => o.OrderDate).Max() >= DateTime.Now.AddDays(-30) && 
                            c.Orders.Select(o => o.OrderDate).Max() <= DateTime.Now),

                    // "Month" => CustomerList
                    //             .Where(c => Int32.Parse(c.Orders.Select(o => o.OrderDate).Max().ToString("yyyy")) == DateTime.Now.Year && 
                    //             Int32.Parse(c.Orders.Select(o => o.OrderDate).Max().ToString("MM")) == DateTime.Now.Month),

                    "Month" => CustomerList
                                .Where(c => c.Orders.Select(o => o.OrderDate).Max() >= new DateTime(DateTime.Now.Year , DateTime.Now.Month , 1) &&
                                    c.Orders.Select(o => o.OrderDate).Max() <= DateTime.Now),

                    _ => CustomerList
                };
                // CustomerList = await GetCustomerByTimeDropDown(timeDropDown , CustomerList);
            }

            if(!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate)){
                CustomerList = CustomerList
                .Where(c => c.Orders.Select(o => o.OrderDate.Date).Max() >= DateTime.Parse(FromDate) && 
                c.Orders.Select(o => o.OrderDate.Date).Max() <= DateTime.Parse(ToDate));
                // CustomerList = await CustomerDateInterval(FromDate , ToDate , CustomerList);
            }

            if(!string.IsNullOrEmpty(SortBy)){
                CustomerList = SortBy switch{
                        "Name" => Desc ? 
                                    CustomerList
                                    .OrderByDescending(c => c.Customername) : 
                                    CustomerList
                                    .OrderBy(c => c.Customername),
                        "Date" => Desc ? 
                                    CustomerList
                                    .OrderByDescending(c => c.Orders.Select(o => o.OrderDate).Max()) : 
                                    CustomerList
                                    .OrderBy(c => c.Orders.Select(o => o.OrderDate).Max()),
                        "Total Order" => Desc ? 
                                    CustomerList
                                    .OrderByDescending(c => c.Orders.Count()): 
                                    CustomerList
                                    .OrderBy(c => c.Orders.Count()),
                        _ => CustomerList
                };
            }

            int totalrecord = await CustomerList.CountAsync();

            List<Customer> Customers = await CustomerList
            .Skip((page-1)*pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (Customers , totalrecord);
        }
    #endregion

    #region SortCustomerList
        public async Task<List<Customer>> SortCustomerList(string SortBy , bool Desc , List<Customer> CustomerList){
           CustomerList = SortBy switch{
                "Name" => Desc ? 
                            CustomerList
                            .OrderByDescending(c => c.Customername)
                            .ToList() : 
                            CustomerList
                            .OrderBy(c => c.Customername)
                            .ToList(),
                "Date" => Desc ? 
                            CustomerList
                            .OrderByDescending(c => c.Orders.Select(o => o.OrderDate).Max())
                            .ToList() : 
                            CustomerList
                            .OrderBy(c => c.Orders.Select(o => o.OrderDate).Max())
                            .ToList(),
                "Total Order" => Desc ? 
                            CustomerList
                            .OrderByDescending(c => c.Orders.Count())
                            .ToList() : 
                            CustomerList
                            .OrderBy(c => c.Orders.Count())
                            .ToList(),
                _ => CustomerList
           };
           return CustomerList;
        }
    #endregion

    #region GetCustomerByTimeDropDown
        public async Task<List<Customer>> GetCustomerByTimeDropDown(string timeDropDown , List<Customer> CustomerList){
            CustomerList = timeDropDown switch{
                "7" => CustomerList
                        .Where(c => c.Orders.Select(o => o.OrderDate).Max() >= DateTime.Now.AddDays(-7) && c.Orders.Select(o => o.OrderDate).Max() <= DateTime.Now)
                        .ToList(),

                "30" => CustomerList
                        .Where(c => c.Orders.Select(o => o.OrderDate).Max() >= DateTime.Now.AddDays(-30) && c.Orders.Select(o => o.OrderDate).Max() <= DateTime.Now)
                        .ToList(),

                "Month" => CustomerList
                            .Where(c => Int32.Parse(c.Orders.Select(o => o.OrderDate).Max().ToString("yyyy")) == DateTime.Now.Year && Int32.Parse(c.Orders.Select(o => o.OrderDate).Max().ToString("MM")) == DateTime.Now.Month)
                            .ToList(),

                _ => CustomerList
            };
            return CustomerList;
        }
    #endregion

    #region CustomerDateInterval
        public async Task<List<Customer>> CustomerDateInterval(string FromDate , string ToDate , List<Customer> CustomerList){
            CustomerList = CustomerList.Where(c => c.Orders.Select(o => o.OrderDate.Date).Max() >= DateTime.Parse(FromDate) && c.Orders.Select(o => o.OrderDate.Date).Max() <= DateTime.Parse(ToDate)).ToList();
            return CustomerList;
        }
    #endregion

    #region GetCustomerDetailsByCustomerId
        public async Task<Customer> GetCustomerDetailsByCustomerId(int CustomerId){
            return await _db.Customers
            .Include(c => c.Orders).ThenInclude(o => o.OrderPayment)
            .Include(c => c.Orders).ThenInclude(o => o.OrderItems.Where(oi => oi.Isdeleted != true))
            .FirstOrDefaultAsync(c => c.Customerid == CustomerId);
        }
    #endregion

    #region GetDataForExcel
        public async Task<(List<Customer> CustomerList , int TotalRecord)> GetDataForExcel(string SearchCriteria , string FromDate , string ToDate , string timeDropDown){
            IQueryable<Customer> CustomerList = _db.Customers
            .Include(c => c.Orders)
            .Where(c => c.Isdeleted != true);

            if(!string.IsNullOrEmpty(timeDropDown)){
                CustomerList = timeDropDown switch{
                    "7" => CustomerList
                            .Where(c => c.Orders.Select(o => o.OrderDate).Max() >= DateTime.Now.AddDays(-7) && 
                            c.Orders.Select(o => o.OrderDate).Max() <= DateTime.Now),

                    "30" => CustomerList
                            .Where(c => c.Orders.Select(o => o.OrderDate).Max() >= DateTime.Now.AddDays(-30) && 
                            c.Orders.Select(o => o.OrderDate).Max() <= DateTime.Now),

                    "Month" => CustomerList
                                .Where(c => Int32.Parse(c.Orders.Select(o => o.OrderDate).Max().ToString("yyyy")) == DateTime.Now.Year && 
                                Int32.Parse(c.Orders.Select(o => o.OrderDate).Max().ToString("MM")) == DateTime.Now.Month),

                    _ => CustomerList
                };
            }

            if(!string.IsNullOrEmpty(SearchCriteria)){
                CustomerList = CustomerList
                .Where(c => c.Customername.ToLower().Contains(SearchCriteria.ToLower()));
            }

            if(!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate)){
                CustomerList = CustomerList
                .Where(c => c.Orders.Select(o => o.OrderDate.Date).Max() >= DateTime.Parse(FromDate) && 
                c.Orders.Select(o => o.OrderDate.Date).Max() <= DateTime.Parse(ToDate));
            }

            List<Customer> Customers = await CustomerList.ToListAsync();

            return (Customers , Customers.Count());
        }
    #endregion

}
