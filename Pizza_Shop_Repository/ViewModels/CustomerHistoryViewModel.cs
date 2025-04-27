namespace Pizza_Shop_Repository.ViewModels;

public class CustomerHistoryViewModel
{
    public string Name { get; set; }
    public decimal PhoneNo { get; set; }
    public decimal MaxOrder { get; set; }
    public decimal AverageBill { get; set; }
    public DateTime ComingSince { get; set; }
    public int Visits { get; set; }
    public List<CustomerHistoryTable> CustomerHistoryData { get; set; }
}
