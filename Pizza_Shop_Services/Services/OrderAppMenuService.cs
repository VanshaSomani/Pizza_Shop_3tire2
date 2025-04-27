using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.ObjectPool;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;
using static Pizza_Shop_Repository.ViewModels.KOTViewModel;

namespace Pizza_Shop_Services.Services;

public class OrderAppMenuService : IOrderAppMenuService
{
    private readonly IOrderAppMenuRepository _orderMenuRepository;
    private readonly IHttpContextAccessor _httpaccessor;
    public OrderAppMenuService(IOrderAppMenuRepository orderMenuRepository, IHttpContextAccessor httpaccessor)
    {
        _orderMenuRepository = orderMenuRepository;
        _httpaccessor = httpaccessor;
    }

    #region GetDataForMenu
    public async Task<OrderAppMenuViewModel> GetDataForMenu(int CategoryID, int customerId, int OrderId)
    {
        OrderAppMenuViewModel obj = new();

        List<Category> categories = await _orderMenuRepository.GetAllCategories();
        List<KOTCategory> category_list = new List<KOTCategory>();
        foreach (Category c in categories)
        {
            KOTCategory cat = new KOTCategory();
            cat.CategoryID = c.Categoryid;
            cat.CategoryName = c.Categoryname;
            category_list.Add(cat);
        }
        obj.OrderMenuCategorieList = category_list;

        List<Item> items = await _orderMenuRepository.GetAllItemByCategoryId(CategoryID);

        obj.OrderMenuItemList = items.Select(i => new OrderMenuItem
        {
            ItemId = i.Itemid,
            ItemName = i.Itemname,
            ItemPrice = i.Rate,
            ItemTax = i.Taxpercentage ?? 0,
            ImgPath = i.Imgfile,
            ItemType = i.ItemType,
            Favorite = i.Isfavourite
        }).ToList();

        if (OrderId != 0)
        {
            Order order = await _orderMenuRepository.OrderByOrderId(OrderId);
            if (order != null)
            {
                if (customerId != 0)
                {
                    obj.CustomerId = customerId;
                }
                obj.OrderId = OrderId;
                obj.OrderStatus = order.Status;
                obj.OrderData = new OrderMenuBillViewModel
                {
                    TableSectionDataList = order.OrderTableMappings.Select(ot => new TableSectionData
                    {
                        SectionId = ot.Table.Section.Sectionid,
                        SectionName = ot.Table.Section.Sectionname,
                        TableId = ot.Table.Tableid,
                        TableName = ot.Table.Tablename
                    }).ToList()
                };

                obj.OrderedItem = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ItemId = oi.Itemid,
                    OrderItemId = oi.Orderitemid,
                    ItemName = oi.Item.Itemname,
                    ItemTax = oi.Item.Taxpercentage ?? 0,
                    ItemQuantity = oi.Quantity,
                    ItemPrice = oi.Item.Rate,
                    TotalPrice = oi.Quantity * oi.Item.Rate,
                    ReadyItemsCount = oi.ReadyItem ?? 0,
                    Modifiers = oi.OrderItemModifiers.Select(oim => new OrderItemModifierViewModel
                    {
                        ModifierId = oim.ModifierId,
                        ModifierGroupId = oim.Modifier.Modifierid,
                        ModifierName = oim.Modifier.Modifiername,
                        ModifierQuantity = (int)oim.Quantity,
                        ModifierPrice = oim.Modifier.Rate,
                        TotalPrice = (int)oim.Quantity * oim.Modifier.Rate
                    }).ToList(),
                    TotalModifierPrice = (decimal)oi.OrderItemModifiers.Sum(oim => oim.Modifier.Rate * oi.Quantity)
                }).ToList();

                if (order.Status == "In Progress")
                {
                    Order taxes = await _orderMenuRepository.GetTaxByOrderId(order.Orderid);
                    obj.TaxList = taxes.InvoceTaxes.Select(it => new MenuTaxDataViewModel
                    {
                        TaxId = it.TaxId,
                        TaxName = it.Tax.Taxname,
                        TaxType = it.Taxtype,
                        TaxRate = it.TaxAmount ?? 0
                    }).ToList();
                }
                else
                {
                    List<Tax> taxes = await _orderMenuRepository.GetAllEnabledTax();

                    obj.TaxList = taxes.Select(t => new MenuTaxDataViewModel
                    {
                        TaxId = t.Taxid,
                        TaxName = t.Taxname,
                        TaxType = t.Taxtype,
                        TaxRate = t.Taxamount ?? 0
                    }).ToList();
                }
            }

        }

        return obj;
    }
    #endregion

    #region GetOrderStatusByOrderId
    public async Task<bool> GetOrderStatusByOrderId(int OrderId)
    {
        try
        {
            Order order = await _orderMenuRepository.OrderStatusByOrderId(OrderId);
            if (order != null)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region GetItemList
    public async Task<OrderAppMenuViewModel> GetItemList(int CategoryID, int customerId, string SearchData)
    {
        OrderAppMenuViewModel obj = new();
        List<Item> items = await _orderMenuRepository.GetAllItemByCategoryId(CategoryID, SearchData);
        obj.CustomerId = customerId;
        obj.OrderMenuItemList = items.Select(i => new OrderMenuItem
        {
            ItemId = i.Itemid,
            ItemName = i.Itemname,
            ItemPrice = i.Rate,
            ItemTax = i.Taxpercentage ?? 0,
            ImgPath = i.Imgfile,
            ItemType = i.ItemType,
            Favorite = i.Isfavourite
        }).ToList();

        return obj;
    }
    #endregion

    #region EditItemFavroite
    public async Task<bool> EditItemFavroite(int CategoryID, int ItemId, bool Isfavourite)
    {
        try
        {
            Item item = await _orderMenuRepository.GetItemByItemId(ItemId);
            item.Isfavourite = !Isfavourite;
            item.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            item.Updatedat = DateTime.Now;
            await _orderMenuRepository.EditItem(item);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region GetDataForModifierGroupModal
    public async Task<MenuItemModalViewModel> GetDataForModifierGroupModal(int ItemId)
    {
        Item item = await _orderMenuRepository.GetItemModifierGroupMappingByItemId(ItemId);
        MenuItemModalViewModel obj = new();
        if (item != null)
        {
            obj.ItemId = item.Itemid;
            obj.ItemName = item.Itemname;
            obj.ItemPrice = item.Rate;
            obj.ItemTax = item.Taxpercentage ?? 0;
            obj.ModifierGroupList = item.ItemModifierGroups.Select(i => new MenuItemVmModifierGroup
            {
                ModifierGroupId = i.ModifierGroupId,
                ModifierGroupName = i.ModifierGroup.Mgname,
                Min = i.Min,
                Max = i.Max,
                ModifierList = i.ModifierGroup.ModifierModifierGroupMappings.Select(im => new MenuItemVmModifier
                {
                    ModifierId = im.ModifierId,
                    ModifierName = im.Modifier.Modifiername,
                    Price = im.Modifier.Rate
                }).ToList()
            }).ToList();
        }
        return obj;
    }
    #endregion

    #region GetCustomerDetail
    public async Task<MenuCustomerDetailViewModel> GetCustomerDetail(int CustomerId, int OrderId)
    {
        Order order = await _orderMenuRepository.GetCustomerByOrderId(OrderId);
        MenuCustomerDetailViewModel obj = new()
        {
            CustomerId = order.Customerid,
            OrderId = order.Orderid,
            CustomerName = order.Customer.Customername,
            PhoneNo = (double)order.Customer.Phoneno,
            NoOfPerson = order.NoOfPerson ?? 0,
            Email = order.Customer.Email
        };
        return obj;
    }
    #endregion

    #region UpdateCustomerInfo
    public async Task<bool> UpdateCustomerInfo(MenuCustomerDetailViewModel obj)
    {
        try
        {
            Customer cust = await _orderMenuRepository.GetCustomerByCustomerId(obj.CustomerId);
            cust.Customername = obj.CustomerName;
            cust.Phoneno = (decimal)obj.PhoneNo;
            cust.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            cust.Updatedat = DateTime.Now;
            await _orderMenuRepository.UpdateCustomer(cust);
            Order order = await _orderMenuRepository.GetCustomerByOrderId(obj.OrderId);
            order.NoOfPerson = obj.NoOfPerson;
            order.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            order.Updatedat = DateTime.Now;
            await _orderMenuRepository.UpdateOrder(order);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region GetOrderInstruction
    public async Task<OrderCommentViewModel> GetOrderInstruction(int OrderId)
    {
        Order order = await _orderMenuRepository.GetCustomerByOrderId(OrderId);
        OrderCommentViewModel obj = new()
        {
            OrderId = order.Orderid,
            OrderInstruction = order.OrderInstruction
        };
        return obj;
    }
    #endregion

    #region AddOrderCommentToOrder
    public async Task<bool> AddOrderCommentToOrder(OrderCommentViewModel obj)
    {
        try
        {
            Order order = await _orderMenuRepository.GetCustomerByOrderId(obj.OrderId);
            order.OrderInstruction = obj.OrderInstruction;
            order.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
            order.Updatedat = DateTime.Now;
            await _orderMenuRepository.UpdateOrder(order);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region GetOrderItemReadyQuantity
        public async Task<int> GetOrderItemReadyQuantity(int OrderItemId){
            OrderItem orderItem = await _orderMenuRepository.GetOrderItemByOrderItemId(OrderItemId);
            return orderItem.ReadyItem ?? 0;
        }
    #endregion

    #region SaveOrder
    public async Task<bool> SaveOrder(int orderid, string orderstatus, List<OrderItemViewModel> save_item, List<int> delete_item, List<MenuTaxDataViewModel> tax_list)
    {
       try
       {
            Order order = await _orderMenuRepository.OrderStatusByOrderId(orderid);
            for(int i = 0 ; i < delete_item.Count ; i++){
                OrderItem orderItem = await _orderMenuRepository.GetOrderItemByOrderItemId(delete_item[i]);
                if(orderItem != null){
                    orderItem.Isdeleted = true;
                    orderItem.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                    orderItem.Updatedat = DateTime.Now;
                    await _orderMenuRepository.UpdateOrderItem(orderItem);
                }
            }
            double total = 0;
            double exclusivetax = 0;
            double subtotal = 0;
            for(int i = 0 ; i < save_item.Count ; i++)
            {
                double modifier_sum = 0;
                if(save_item[i].OrderItemId != 0){
                    OrderItem orderItem = await _orderMenuRepository.GetOrderItemByOrderItemId(save_item[i].OrderItemId);
                    if(orderItem.Quantity != save_item[i].ItemQuantity)
                    {
                        orderItem.Quantity = save_item[i].ItemQuantity;
                        orderItem.Status = "In Progress";
                        orderItem.Updatedby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId"));
                        orderItem.Updatedat = DateTime.Now;
                        await _orderMenuRepository.UpdateOrderItem(orderItem);
                    }
                    if(orderItem.OrderItemModifiers.Count != 0){
                        modifier_sum += (double) orderItem.OrderItemModifiers.Sum(oim => oim.ModifierAmount);
                    }
                }
                else{
                    OrderItem orderItem = new(){
                        Orderid = order.Orderid,
                        Itemid = save_item[i].ItemId,
                        Quantity = save_item[i].ItemQuantity,
                        Amount = save_item[i].ItemPrice,
                        Itemtaxpercentage = (decimal?)save_item[i].ItemTax,
                        Status = "In Progress",
                        ReadyItem = 0,
                        Isdeleted = false,
                        Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")),
                        Createdat = DateTime.Now
                    };
                    orderItem = await _orderMenuRepository.AddOrderItem(orderItem);
                    for(int j = 0 ; j < save_item[i].Modifiers.Count ; j++){
                        OrderItemModifier orderItemModifier = new(){
                            OrderItemId = orderItem.Orderitemid,
                            ModifierId = save_item[i].Modifiers[j].ModifierId,
                            ModifierGroupId = save_item[i].Modifiers[j].ModifierGroupId,
                            ModifierAmount = save_item[i].Modifiers[j].ModifierPrice,
                            Quantity = save_item[i].Modifiers[j].ModifierQuantity   
                        };
                        await _orderMenuRepository.AddOrderItemModifier(orderItemModifier);
                        modifier_sum += (double)save_item[i].Modifiers[j].ModifierPrice;
                    }
                }
                double item_sum = (double)(save_item[i].ItemPrice*save_item[i].ItemQuantity);
                subtotal += item_sum + (modifier_sum*save_item[i].ItemQuantity);
                exclusivetax += item_sum * save_item[i].ItemTax / 100;
            }
            double totalTax = 0;
            for(int i = 0 ; i < tax_list.Count ; i++){
                if(order.Status == "Pending"){
                    InvoceTax invoceTax = new(){
                        OrderId = orderid,
                        TaxId = tax_list[i].TaxId,
                        Taxtype = tax_list[i].TaxType,
                        TaxAmount = tax_list[i].TaxRate
                    };
                    await _orderMenuRepository.AddOrderTax(invoceTax);
                }
                if(tax_list[i].TaxType == "percentage"){
                    totalTax += subtotal*(double)tax_list[i].TaxRate/100;
                }
                else{
                    totalTax += (double)tax_list[i].TaxRate;
                }
            }
            total = subtotal + totalTax + exclusivetax;
            if(order.OrderPaymentId != null){
                OrderPayment orderPayment = await _orderMenuRepository.GetOrderPaymentById((int)order.OrderPaymentId);
                orderPayment.TotalAmount = (decimal)total;
                await _orderMenuRepository.UpdateOrderPayment(orderPayment);
            }
            else{
                OrderPayment orderPayment = new(){
                    TotalAmount = (decimal)total,
                    // Paymenttype = 
                    Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")),
                    Createdat = DateTime.Now
                };
                orderPayment = await _orderMenuRepository.AddOrderPayment(orderPayment);
                order.OrderPaymentId = orderPayment.PaymentId;
            }
            if(order.Status == "Pending"){
                Invoice invoice = new(){
                    Orderid = order.Orderid,
                    Createdby = Int32.Parse(_httpaccessor.HttpContext.Session.GetString("UserId")),
                    Createdat = DateTime.Now
                };
                invoice = await _orderMenuRepository.AddInvoice(invoice);
            }
            await _orderMenuRepository.UpdateOrder(order);
            return true;
       }
       catch (Exception e)
       {
        return false;
       } 
    }
    #endregion
}
