using System.Security.Cryptography;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class KOTService : IKOTService
{
    private readonly IKOTRepository _kotRepository;
    public KOTService(IKOTRepository kotRepository){
        _kotRepository = kotRepository;
    }

    #region GetDataForKOT
        public async Task<KOTViewModel> GetDataForKOT(){
            KOTViewModel obj = new KOTViewModel();

            List<KOTViewModel.KOTCategory> categories = new List<KOTViewModel.KOTCategory>();
            List<Category> categoryList = await _kotRepository.GetCategoriesForKot();
            foreach(Category c in categoryList){
                KOTViewModel.KOTCategory kc = new KOTViewModel.KOTCategory();
                kc.CategoryID = c.Categoryid;
                kc.CategoryName = c.Categoryname;
                categories.Add(kc);
            }
            obj.KOTCategorieList = categories;
            return obj;
        }
    #endregion

    //changes
    #region GetKOTCardsDetails
        public async Task<List<KOTCardDetailsViewModel>> GetKOTCardsDetails(int CategoryID = -1 , string status = "In Progress"){
            List<Order> orderList = await _kotRepository.GetDataForKOTCards(CategoryID , status);
            List<KOTCardDetailsViewModel> KOTCardList = new List<KOTCardDetailsViewModel>();
            
            foreach(Order o in orderList){
                KOTCardDetailsViewModel KOTCD = new KOTCardDetailsViewModel();
                KOTCD.OrderId = o.Orderid;
                // KOTCD.SectionName = o.Table.Section.Sectionname;
                // KOTCD.TableName = o.Table.Tablename;
                KOTCD.OrderStatus = o.Status;
                KOTCD.OrderDate = o.OrderDate;
                KOTCD.OrderInstruction = o.OrderInstruction;

                List<TableSectionListOrderDetails> ordertables = new List<TableSectionListOrderDetails>();
                foreach(OrderTableMapping ot in o.OrderTableMappings){
                    TableSectionListOrderDetails ts = new TableSectionListOrderDetails();
                    ts.TableName = ot.Table.Tablename;
                    bool check = ordertables.Any(o => o.SectionName == ot.Table.Section.Sectionname);
                    if(!check){
                        ts.SectionName = ot.Table.Section.Sectionname;
                    }
                    ordertables.Add(ts);
                }
                KOTCD.TableSectionList = ordertables;

                List<KOTCardData> KOTCardDetailsTable = new List<KOTCardData>();
                foreach(OrderItem oi in o.OrderItems){
                    KOTCardData od = new KOTCardData();
                    od.ItemName = oi.Item.Itemname;
                    od.ItemQuantity = oi.Quantity;
                    od.OrderItemInstruction = oi.OrderItemInstruction;
                    List<KOTCardModifierData> OrderItemModifierList = new List<KOTCardModifierData>();
                    foreach(OrderItemModifier oim in oi.OrderItemModifiers){
                        KOTCardModifierData om = new KOTCardModifierData();
                        om.ModifierName = oim.Modifier.Modifiername;
                        om.ModifierQuantity = (int)oim.Quantity;
                        OrderItemModifierList.Add(om);
                    }
                    od.KOTItemModifierList = OrderItemModifierList;
                    KOTCardDetailsTable.Add(od);
                }
                KOTCD.KOTCardDataList = KOTCardDetailsTable;
                KOTCardList.Add(KOTCD);
            }
            return KOTCardList;
        }
    #endregion

    #region GetKotDataByOrderId
        public async Task<KOTModalInfoViewModel> GetKotDataByOrderId(int OrderId , string Status , int CategoryID){
            Order o = await _kotRepository.GetOrderItemDetailByOrderId(OrderId , Status , CategoryID);
            KOTModalInfoViewModel obj = new KOTModalInfoViewModel();
            obj.OrderId = o.Orderid;
            obj.Status = Status;
            List<KOTModalTTableData> KotTableList = new List<KOTModalTTableData>();
            foreach(OrderItem oi in o.OrderItems){
                KOTModalTTableData k = new KOTModalTTableData();
                k.ItemId = oi.Itemid;
                k.ItemName = oi.Item.Itemname;
                k.ItemQuantity = oi.Quantity;
                k.ReadyItem = (int)oi.ReadyItem;
                List<string> modifierList = new List<string>();
                foreach(OrderItemModifier oim in oi.OrderItemModifiers){
                    modifierList.Add(oim.Modifier.Modifiername);
                }
                k.ModifierName = modifierList;
                KotTableList.Add(k);
            }
            obj.KotTableData = KotTableList;
            return obj;
        }
    #endregion

    #region UpdateKotData
        public async Task<bool> UpdateKotData(int OrderId , string Status , List<UpdateItem> UpdatedItem){
            try
            {
                for(int i = 0 ; i < UpdatedItem.Count() ; i++){
                    await _kotRepository.UpdateKotDataByOrderId(OrderId , Status , UpdatedItem[i].itemId , UpdatedItem[i].itemQuantity);
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
