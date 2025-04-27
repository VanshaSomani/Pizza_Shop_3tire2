namespace Pizza_Shop_Repository.ViewModels;

public class KOTCardDetailsViewModel
{
    public int OrderId { get; set; }
    public string SectionName { get; set; }
    public string TableName { get; set; }
    public List<TableSectionListOrderDetails> TableSectionList { get; set; }
    public string OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderInstruction { get; set; }
    public List<KOTCardData> KOTCardDataList { get; set; }
}
