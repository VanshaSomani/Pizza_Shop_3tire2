namespace Pizza_Shop_Repository.ViewModels;

public class OrderAppTableInfo
{
    public int TableId { get; set; }
    public string TableName { get; set; }
    public int TableCapacity { get; set; }
    public DateTime AssignTime { get; set; }
    public string TableStatus { get; set; }
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    
}
