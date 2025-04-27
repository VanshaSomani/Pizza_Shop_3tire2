namespace Pizza_Shop_Repository.ViewModels;

public class OrderItemTableModifierViewModel
{
    public string ModifierName { get; set; }
    public int Quantity { get; set; }
    public decimal ModifierPrice { get; set; }
    public decimal ModifierTotalAmount { get; set; }
}
