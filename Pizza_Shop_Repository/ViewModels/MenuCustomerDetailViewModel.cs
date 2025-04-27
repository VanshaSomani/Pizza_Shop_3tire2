using System.ComponentModel.DataAnnotations;

namespace Pizza_Shop_Repository.ViewModels;

public class MenuCustomerDetailViewModel
{
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    [Required (ErrorMessage ="Enter Customername")]
    public string CustomerName { get; set; }
    [Required (ErrorMessage = "Enter phoneno")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone no format is not valid.")]
    public double PhoneNo { get; set; }
    [Required (ErrorMessage ="Enter no of persons")]
    [Range(1,int.MaxValue , ErrorMessage = "Enter No Of Person")]
    public int NoOfPerson { get; set; }
    public string Email { get; set; }
}
