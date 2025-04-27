using Microsoft.AspNetCore.Server.Kestrel.Core;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerExcelService _customerExcelService;
    public CustomerService(ICustomerRepository customerRepository , ICustomerExcelService customerExcelService){
        _customerRepository = customerRepository;
        _customerExcelService = customerExcelService;
    }

    #region GetCustomerDetailsList
        public async Task<CustomerListViewModel> GetCustomerDetailsList(int page = 1 , int pageSize = 5){
            (List<Customer> obj , int totalrecord) = await _customerRepository.GetCustomerList(page , pageSize);
            List<CustomerViewModel> ListOfCustomer = new List<CustomerViewModel>();
            foreach(Customer customer in obj){
                CustomerViewModel c = new CustomerViewModel();
                c.CustomerId = customer.Customerid;
                c.CustomerName = customer.Customername;
                c.Email = customer.Email;
                if(customer.Orders.Count() != 0){
                    c.Date = customer.Orders.Select(o => o.OrderDate).Max();
                }
                else{
                    c.Date = customer.CustDate;
                }
                c.PhoneNo = (double)customer.Phoneno;
                c.CustomerId = customer.Customerid;
                c.TotalOrder = customer.Orders.Count();
                ListOfCustomer.Add(c);
            }
            CustomerListViewModel CustomerData = new CustomerListViewModel{
                CustomerList = ListOfCustomer
            };
            CustomerData.CustomerPaggination = new PagginationViewModel{
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecord = totalrecord,
                TotalPage = (int)Math.Ceiling((double)totalrecord/pageSize),
                MinRow = ((page - 1)*pageSize)+1,
                MaxRow = ((page - 1)*pageSize) + CustomerData.CustomerList.Count()
            };
            return CustomerData;
        }
    #endregion

    #region GetCustomerDetailsListForPartialView
        public async Task<CustomerListViewModel> GetCustomerDetailsListForPartialView(string SortBy , bool Desc ,int page , int pageSize , string SearchCriteria , string FromDate , string ToDate , string timeDropDown){
            (List<Customer> obj , int totalrecord) = await _customerRepository.GetCustomerListForPartialView(SortBy , Desc , page , pageSize , timeDropDown , FromDate , ToDate , SearchCriteria);
            List<CustomerViewModel> ListOfCustomer = new List<CustomerViewModel>();
            foreach(Customer customer in obj){
                CustomerViewModel c = new CustomerViewModel();
                c.CustomerId = customer.Customerid;
                c.CustomerName = customer.Customername;
                c.Email = customer.Email;
                if(customer.Orders.Count() != 0){
                    c.Date = customer.Orders.Select(o => o.OrderDate).Max();
                }
                else{
                    c.Date = customer.CustDate;
                }
                c.PhoneNo = (double)customer.Phoneno;
                c.CustomerId = customer.Customerid;
                c.TotalOrder = customer.Orders.Count();
                ListOfCustomer.Add(c);
            }
            CustomerListViewModel CustomerData = new CustomerListViewModel{
                CustomerList = ListOfCustomer
            };
            CustomerData.CustomerPaggination = new PagginationViewModel{
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecord = totalrecord,
                TotalPage = (int)Math.Ceiling((double)totalrecord/pageSize),
                MinRow = ((page - 1)*pageSize)+1,
                MaxRow = ((page - 1)*pageSize) + CustomerData.CustomerList.Count()
            };
            return CustomerData;
        }
    #endregion

    #region GetCustomerHistory
        public async Task<CustomerListViewModel> GetCustomerHistory(int CustomerId){
            Customer c = await _customerRepository.GetCustomerDetailsByCustomerId(CustomerId);
            CustomerListViewModel obj = new CustomerListViewModel();
            CustomerHistoryViewModel custHistory = new CustomerHistoryViewModel();
            custHistory.Name = c.Customername;
            custHistory.PhoneNo = c.Phoneno;
            custHistory.AverageBill = c.Orders.Where(o => o.OrderPayment != null).Sum(o => o.OrderPayment.TotalAmount);
            if(c.Orders.Count() != 0){
                custHistory.ComingSince = c.Orders.Select(o => o.OrderDate).Min();
            }
            else{
                custHistory.ComingSince = c.CustDate;
            }
            custHistory.Visits = c.Orders.Count();
            if(c.Orders.Count() != 0){
                custHistory.MaxOrder = c.Orders.Where(o => o.OrderPayment != null).Select(o => o.OrderPayment.TotalAmount).Max();
            }

            List<CustomerHistoryTable> OrderList = new List<CustomerHistoryTable>();
            foreach(Order o in c.Orders){
                CustomerHistoryTable ch = new CustomerHistoryTable();
                ch.OrderDate = o.OrderDate.Date;
                ch.OrderType = "DineIn";
                ch.Payment = o.OrderPaymentId == null ? "Pending" : "Paid";
                ch.Items = o.OrderItems.Count();
                ch.Amount = o.OrderPayment != null ? o.OrderPayment.TotalAmount : 0;
                OrderList.Add(ch);
            }

            custHistory.CustomerHistoryData = OrderList;

            obj.CustomerHistory = custHistory;

            return obj;
        }
    #endregion

    #region ExportAsync
        public async Task<byte[]> ExportAsync(string SearchCriteria , string FromDate , string ToDate , string timeDropDown){
            (List<Customer> obj , int totalrecord) = await _customerRepository.GetDataForExcel(SearchCriteria , FromDate , ToDate , timeDropDown);
            List<CustomerViewModel> ListOfCustomer = new List<CustomerViewModel>();
            foreach(Customer customer in obj){
                CustomerViewModel c = new CustomerViewModel();
                c.CustomerId = customer.Customerid;
                c.CustomerName = customer.Customername;
                c.Email = customer.Email;
                if(customer.Orders.Count() != 0){
                    c.Date = customer.Orders.Select(o => o.OrderDate).Max();
                }
                else{
                    c.Date = customer.CustDate;
                }
                c.PhoneNo = (double)customer.Phoneno;
                c.CustomerId = customer.Customerid;
                c.TotalOrder = customer.Orders.Count();
                ListOfCustomer.Add(c);
            }
            CustomerListViewModel CustomerData = new CustomerListViewModel{
                CustomerList = ListOfCustomer
            };
            CustomerData.CustomerPaggination = new PagginationViewModel{
                TotalRecord = totalrecord
            };

            return await _customerExcelService.ExportDataToExcel(CustomerData , SearchCriteria , FromDate , ToDate , timeDropDown);
        }
    #endregion

}
