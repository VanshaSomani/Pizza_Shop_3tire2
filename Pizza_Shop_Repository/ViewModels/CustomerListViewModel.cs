namespace Pizza_Shop_Repository.ViewModels;

public class CustomerListViewModel
{
    public List<CustomerViewModel> CustomerList { get; set; }
    public CustomerHistoryViewModel CustomerHistory { get; set; }
    public PagginationViewModel CustomerPaggination { get; set; }
}
