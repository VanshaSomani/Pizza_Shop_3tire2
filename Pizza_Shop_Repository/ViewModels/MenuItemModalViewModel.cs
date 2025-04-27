namespace Pizza_Shop_Repository.ViewModels;

public class MenuItemModalViewModel
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal ItemPrice { get; set; }
    public double ItemTax { get; set; }
    public List<MenuItemVmModifierGroup> ModifierGroupList { get; set; }
}
public class MenuItemVmModifierGroup
{
    public int ModifierGroupId { get; set; }
    public string ModifierGroupName { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public List<MenuItemVmModifier> ModifierList { get; set; }
}
public class MenuItemVmModifier{
    public int ModifierId { get; set; }
    public string ModifierName { get; set; }
    public double Price { get; set; }
}