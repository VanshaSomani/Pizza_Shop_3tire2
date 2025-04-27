namespace Pizza_Shop_Repository.ViewModels;

public class KOTCardData
{
    public string ItemName { get; set; }
    public int ItemQuantity { get; set; }
    public string Instructions { get; set; }
    public string OrderItemInstruction { get; set; }
    public List<KOTCardModifierData> KOTItemModifierList { get; set; }
}
