namespace Pizza_Shop_Repository.ViewModels;

public class OffCanvasWaitingListViewModel
{
    public List<OffCanvasWaitingData> WaitingList { get; set; }    
}

public class OffCanvasWaitingData{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int NoOfPerson { get; set; }
}
