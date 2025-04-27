using static Pizza_Shop_Repository.ViewModels.KOTViewModel;

namespace Pizza_Shop_Repository.ViewModels;

public class OrderAppMenuViewModel
{
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    public string OrderStatus { get; set; }
    public List<KOTCategory> OrderMenuCategorieList { get; set; }
    public List<OrderMenuItem> OrderMenuItemList { get; set; }
    public OrderMenuBillViewModel OrderData { get; set; }
    public List<OrderItemViewModel> OrderedItem { get; set; }
    public List<MenuTaxDataViewModel> TaxList { get; set; }
}

public class OrderMenuItem
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal ItemPrice { get; set; }
    public double ItemTax { get; set; }
    public string ItemType { get; set; }
    public string ImgPath { get; set; }
    public bool Favorite { get; set; }
}

public class OrderMenuBillViewModel
{
    public List<TableSectionData> TableSectionDataList { get; set; }
}

public class TableSectionData
{
    public int SectionId { get; set; }
    public string SectionName { get; set; }
    public int TableId { get; set; }
    public string TableName { get; set; }
}

public class OrderItemViewModel{
    public int ItemId { get; set; }
    public int OrderItemId { get; set; }
    public string ItemName { get; set; }
    public int ItemQuantity { get; set; }
    public decimal ItemPrice { get; set; }
    public double ItemTax { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalModifierPrice { get; set; }
    public int ReadyItemsCount { get; set; }
    public List<OrderItemModifierViewModel> Modifiers { get; set; }
}

public class OrderItemModifierViewModel{
    public int ModifierId { get; set; }
    public int ModifierGroupId { get; set; }
    public string ModifierName { get; set; }
    public int ModifierQuantity { get; set; }
    public decimal ModifierPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class MenuTaxDataViewModel{
    public int TaxId { get; set; }
    public string TaxName { get; set; }
    public string TaxType { get; set; }
    public decimal TaxRate { get; set; }
}