namespace Pizza_Shop_Repository.ViewModels;

public class KOTModalInfoViewModel
{
    public int OrderId { get; set; }
    public string Status { get; set; }
    public List<KOTModalTTableData> KotTableData { get; set; }
}
