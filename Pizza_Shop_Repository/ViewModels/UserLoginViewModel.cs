using System.ComponentModel.DataAnnotations;

namespace Pizza_Shop_Repository.ViewModels;

public class UserLoginViewModel
{
    [Required(ErrorMessage = "Enter Email")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Enter Password")]
    public string Passwordhashed { get; set; }
    public Boolean RemeberMe { get; set; }
}
