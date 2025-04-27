using System.ComponentModel.DataAnnotations;

namespace Pizza_Shop_Repository.ViewModels;

public class OffCanvasCustomerDetailViewModel
{
    public List<OffCanvasCustomerDetailSectionList> SectionList { get; set; }
    [Required(ErrorMessage = "Select Section")]
    public int SectionId { get; set; }
    public int CustomerId { get; set; }
    [Required(ErrorMessage ="Enter Customer Name")]
    public string CustomerName { get; set; }
    [Required(ErrorMessage ="Enter No Of Person")]
    [Range(1,int.MaxValue , ErrorMessage = "Enter No Of Person")]
    public int NoOfPerson { get; set; }
    [Required(ErrorMessage ="Enter Email")]
    public string Email { get; set; }
    [Required(ErrorMessage ="Enter Phone No")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone no format is not valid.")]
    public double Phoneno { get; set; }
    public int WaitingTokenId { get; set; }
}

public class OffCanvasCustomerDetailSectionList{
    public int SectionId { get; set; }
    public string SectionName { get; set; }
}
