namespace Pizza_Shop_Repository.ViewModels;

public class OrderDetailViewModel
{
    public int OrderId { get; set; }
    public string InvoceNo { get; set; }
    public DateTime? PaidOn { get; set; }
    public DateTime PlaceOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public TimeOnly OrderDuration { get; set; }
    public string OrderStatus { get; set; }
    public string CustomerName { get; set; }
    public decimal CustomerPhoneNo { get; set; }
    public int NoOfPerson { get; set; }
    public string CustomerEmail { get; set; }
    // public string TableName { get; set; }
    // public string SectionName { get; set; }
    public string PaymentMethod { get; set; }
    public List<TableSectionListOrderDetails> TableSectionList { get; set; }
    public List<OrderDetailTableViewModel> OrderDetailsList { get; set; }
    public List<OrderDetailTaxViewModel> OrderDetailTax { get; set; }
    public decimal SubTotal { get; set; }
    public decimal OtherTax { get; set; }
    public decimal TotalAmount { get; set; }
}
