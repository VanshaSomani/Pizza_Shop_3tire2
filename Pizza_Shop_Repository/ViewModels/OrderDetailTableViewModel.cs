namespace Pizza_Shop_Repository.ViewModels;

public class OrderDetailTableViewModel
{
    public string ItemName { get; set; }
    public int ItemQuantity { get; set; }
    public decimal ItemAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemTableModifierViewModel> ItemModifierList { get; set; }
}
