namespace Pizza_Shop_Repository.ViewModels;

public class OrderListViewModel
{
    public List<OrderViewModel> Order_list { get; set; }
    public PagginationViewModel Order_Paggination { get; set; }
}
