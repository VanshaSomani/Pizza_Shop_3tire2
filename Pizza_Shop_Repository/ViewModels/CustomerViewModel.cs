namespace Pizza_Shop_Repository.ViewModels;

public class CustomerViewModel
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string Email { get; set; }
    public double PhoneNo { get; set; }
    public DateTime Date { get; set; }
    public int TotalOrder { get; set; }
}
