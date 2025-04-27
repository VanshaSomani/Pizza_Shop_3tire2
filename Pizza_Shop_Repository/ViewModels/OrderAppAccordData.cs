namespace Pizza_Shop_Repository.ViewModels;

public class OrderAppAccordData
{
    public int SectionId { get; set; }
    public string SectionName { get; set; }
    public int Available { get; set; }
    public int Assigned { get; set; }
    public int Running { get; set; }
    public List<OrderAppTableInfo> TableList { get; set; }
}
