namespace Pizza_Shop_Repository.ViewModels;

public class CustomerHistoryTable
{
    public DateTime OrderDate { get; set; }
    public string OrderType { get; set; }
    public string Payment { get; set; }
    public int Items { get; set; }
    public decimal Amount { get; set; }
}
