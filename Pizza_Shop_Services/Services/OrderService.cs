using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    private readonly IOrderExcelService _orderExcel;
    public OrderService(IOrderRepository orderRepository , IOrderExcelService orderExcel){
        _orderRepository = orderRepository;
        _orderExcel = orderExcel;
    }

    #region GetOrderListViewModel
        public async Task<OrderListViewModel> GetOrderListViewModel(int page = 1 , int pageSize = 5){
            (List<Order> OrderList , int totalrecord) = await _orderRepository.GetAllOrders(page , pageSize);
            
            List<OrderViewModel> order_list = new List<OrderViewModel>();
            for(int i = 0 ; i < OrderList.Count ; i++){
                OrderViewModel o = new OrderViewModel();
                o.OrderId = OrderList[i].Orderid;
                o.OrderDate = OrderList[i].OrderDate;
                o.CustomerName = OrderList[i].Customer.Customername;
                o.Status = OrderList[i].Status;
                if(OrderList[i].OrderPayment != null){
                    o.PaymentMode = OrderList[i].OrderPayment.Paymenttype;
                }
                else{
                    o.PaymentMode = "Pending";
                }
                if(OrderList[i].Rating != null){
                    o.Rating = OrderList[i].Rating.Avgratting;
                }
                else{
                    o.Rating = 0;
                }
                if(OrderList[i].OrderPayment != null){
                    o.TotalAmount = (double)OrderList[i].OrderPayment.TotalAmount;
                }
                else{
                    o.TotalAmount = 0;
                }
                order_list.Add(o);
            }

            OrderListViewModel obj = new OrderListViewModel{
                Order_list = order_list,   
            };
            obj.Order_Paggination = new PagginationViewModel{
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecord = totalrecord,
                TotalPage = (int)Math.Ceiling((double)totalrecord/pageSize),
                MinRow = ((page - 1)*pageSize)+1,
                MaxRow = ((page - 1)*pageSize) + obj.Order_list.Count(),
            };
            return obj;
        }
    #endregion

    #region GetOrderListViewModelForPartialView
        public async Task<OrderListViewModel> GetOrderListViewModelForPartialView(int page , int pageSize , string timeDropDown , string SearchCriteria , string Status , string SortBy , bool Desc , string FromDate , string ToDate){
            (List<Order> OrderList , int totalrecord) = await _orderRepository.GetAllOrderForPartialView(SearchCriteria , page , pageSize , Status , SortBy , Desc ,timeDropDown , FromDate , ToDate);
            //OrderList = await _orderRepository.SortOrderList(SortBy , Desc , OrderList);
            List<OrderViewModel> order_list = new List<OrderViewModel>();
            for(int i = 0 ; i < OrderList.Count ; i++){
                OrderViewModel o = new OrderViewModel();
                o.OrderId = OrderList[i].Orderid;
                o.OrderDate = OrderList[i].OrderDate;
                o.CustomerName = OrderList[i].Customer.Customername;
                o.Status = OrderList[i].Status;
                if(OrderList[i].OrderPayment != null){
                    o.PaymentMode = OrderList[i].OrderPayment.Paymenttype;
                }
                else{
                    o.PaymentMode = "Pending";
                }
                if(OrderList[i].Rating != null){
                    o.Rating = OrderList[i].Rating.Avgratting;
                }
                else{
                    o.Rating = 0;
                }
                if(OrderList[i].OrderPayment != null){
                    o.TotalAmount = (double)OrderList[i].OrderPayment.TotalAmount;
                }
                else{
                    o.TotalAmount = 0;
                }
                order_list.Add(o);
            }

            OrderListViewModel obj = new OrderListViewModel{
                Order_list = order_list,   
            };
            obj.Order_Paggination = new PagginationViewModel{
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecord = totalrecord,
                TotalPage = (int)Math.Ceiling((double)totalrecord/pageSize),
                MinRow = ((page - 1)*pageSize)+1,
                MaxRow = ((page - 1)*pageSize) + obj.Order_list.Count(),
            };
            return obj;
        }
    #endregion

    #region ExportAsync
        public async Task<byte[]> ExportAsync(string SearchCriteria , string timeDropDown , string Status)
        {
            (List<Order> OrderList , int totalrecord) = await _orderRepository.GetDataForExcel(SearchCriteria , timeDropDown , Status);

            List<OrderViewModel> order_list = new List<OrderViewModel>();
            for(int i = 0 ; i < OrderList.Count ; i++){
                OrderViewModel o = new OrderViewModel();
                o.OrderId = OrderList[i].Orderid;
                o.OrderDate = OrderList[i].OrderDate;
                o.CustomerName = OrderList[i].Customer.Customername;
                o.Status = OrderList[i].Status;
                if(OrderList[i].OrderPayment != null){
                    o.PaymentMode = OrderList[i].OrderPayment.Paymenttype;
                }
                else{
                    o.PaymentMode = "Pending";
                }
                if(OrderList[i].Rating != null){
                    o.Rating = OrderList[i].Rating.Avgratting;
                }
                else{
                    o.Rating = 0;
                }
                if(OrderList[i].OrderPayment != null){
                    o.TotalAmount = (double)OrderList[i].OrderPayment.TotalAmount;
                }
                else{
                    o.TotalAmount = 0;
                }
                order_list.Add(o);
            }

            OrderListViewModel obj = new OrderListViewModel{
                Order_list = order_list,   
            };
            obj.Order_Paggination = new PagginationViewModel{
                TotalRecord = totalrecord,
            };
            
            return await _orderExcel.ExportDataToExcel(obj , SearchCriteria , timeDropDown , Status); 
        }
    #endregion

    //change
    #region GetOrderDetailsAsync
        public async Task<OrderDetailViewModel> GetOrderDetailsAsync(int OrderId){
            Order order = await _orderRepository.GetEntireOrderDetails(OrderId);
            OrderDetailViewModel obj = new OrderDetailViewModel();

            obj.OrderId = order.Orderid;
            if(order.Invoices.Count() != 0){
                obj.InvoceNo = order.Invoices.First().Invoiceid.ToString();
            }
            obj.CustomerName = order.Customer.Customername;
            obj.CustomerPhoneNo = order.Customer.Phoneno;
            obj.OrderStatus = order.Status;
            int capacity = 0;
            foreach(OrderTableMapping ot in order.OrderTableMappings){
                capacity += ot.Table.Capacity;
            }
            obj.NoOfPerson = capacity;
            obj.CustomerEmail = order.Customer.Email;
            // obj.TableName = order.Table.Tablename;
            // obj.SectionName = order.Table.Section.Sectionname;
            if(order.OrderPayment != null){
                obj.PaidOn = (DateTime)order.OrderPayment.Paidon;
            }
            obj.PlaceOn = order.OrderDate;
            if(order.Updatedat != null){
                obj.ModifiedOn = (DateTime)order.Updatedat;
            }
            obj.ModifiedOn = order.OrderDate;
            if(order.OrderPayment != null){
                obj.PaymentMethod = order.OrderPayment.Paymenttype;
            }
            else{
                obj.PaymentMethod = "Pending";
            }

            List<TableSectionListOrderDetails> ordertables = new List<TableSectionListOrderDetails>();
            foreach(OrderTableMapping ot in order.OrderTableMappings){
                TableSectionListOrderDetails ts = new TableSectionListOrderDetails();
                ts.TableName = ot.Table.Tablename;
                bool check = ordertables.Any(o => o.SectionName == ot.Table.Section.Sectionname);
                if(!check){
                    ts.SectionName = ot.Table.Section.Sectionname;
                }
                ordertables.Add(ts);
            }

            obj.TableSectionList = ordertables;

            List<OrderDetailTableViewModel> OrderDetailsTable = new List<OrderDetailTableViewModel>();
            decimal SubTotal = 0;
            decimal OtherTax = 0;
            foreach(OrderItem o in order.OrderItems){
                OrderDetailTableViewModel od = new OrderDetailTableViewModel();
                od.ItemName = o.Item.Itemname;
                od.ItemQuantity = o.Quantity;
                od.ItemAmount = (decimal)o.Amount;
                od.TotalAmount = (decimal)(o.Quantity * o.Amount);
                OtherTax = OtherTax + (od.TotalAmount * (decimal)o.Itemtaxpercentage/100);
                List<OrderItemTableModifierViewModel> OrderItemModifierList = new List<OrderItemTableModifierViewModel>();
                decimal modifierSubTotal = 0;
                foreach(OrderItemModifier oim in o.OrderItemModifiers){
                    OrderItemTableModifierViewModel om = new OrderItemTableModifierViewModel();
                    om.ModifierName = oim.Modifier.Modifiername;
                    om.Quantity = o.Quantity;
                    om.ModifierPrice = (decimal)oim.ModifierAmount;
                    om.ModifierTotalAmount = (decimal)(o.Quantity * oim.ModifierAmount);
                    modifierSubTotal += om.ModifierTotalAmount;
                    OrderItemModifierList.Add(om);
                }
                SubTotal = SubTotal + od.TotalAmount + modifierSubTotal;
                od.ItemModifierList = OrderItemModifierList;
                OrderDetailsTable.Add(od);
            }
            obj.OrderDetailsList = OrderDetailsTable;
            obj.SubTotal = SubTotal;
            obj.OtherTax = OtherTax;
            decimal TotalTaxAmount = 0;
            if(order.Invoices.Count != 0){
                List<OrderDetailTaxViewModel> OrderTaxList = new List<OrderDetailTaxViewModel>();
                foreach(InvoceTax it in order.InvoceTaxes){
                    OrderDetailTaxViewModel odt = new OrderDetailTaxViewModel();
                    odt.TaxName = it.Tax.Taxname;
                    odt.TaxAmount = it.Tax.Taxtype switch
                    {
                        "percentage" => (decimal)(SubTotal * it.TaxAmount / 100),
                        "fixed amount" => (decimal)it.TaxAmount,
                        _ => 0
                    };
                    TotalTaxAmount += odt.TaxAmount;
                    OrderTaxList.Add(odt);
                }
                obj.OrderDetailTax = OrderTaxList;
            }
            obj.TotalAmount = SubTotal + TotalTaxAmount + OtherTax;
            return obj;
        }

    #endregion

}
