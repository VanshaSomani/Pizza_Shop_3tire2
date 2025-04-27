namespace Pizza_Shop_Repository.ViewModels;

public class WaitingListViewModel
{
    public List<WaitingListSection> SectionList { get; set; }
    public List<WaitingData> WaitingDataList { get; set; }
    public int TotalWaitingCount { get; set; }
    public PagginationViewModel WaitingListPaggination { get; set; }
}

public class WaitingListSection
{
    public int SectionId { get; set; }
    public string SectionName { get; set; }
    public int WaitingCount { get; set; }
}

public class WaitingData
{
    public int WaitingListId { get; set; }
    public int SectionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CustomerName { get; set; }
    public int NoOfPerson { get; set; }
    public double PhoneNo { get; set; }
    public string Email { get; set; }
}