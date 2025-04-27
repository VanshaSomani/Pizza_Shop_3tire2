namespace Pizza_Shop_Repository.ViewModels;

public class AssignWaitingTokenViewModel
{
    public int WaitingListID { get; set; }
    public int WaitingNoOfPerson { get; set; }
    public int SectionId { get; set; }
    public int CustomerId { get; set; }
    public List<SectionData> SectionList { get; set; }
    public List<TableData> TableList { get; set; }
}

public class SectionData{
    public int SectionId { get; set; }
    public string SectionName { get; set; }
}

public class TableData{
    public int TableId { get; set; }
    public string TableName { get; set; }
    public int TableCapacity { get; set; }
}
