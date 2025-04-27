using System.ComponentModel.DataAnnotations;

namespace Pizza_Shop_Repository.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Enter email")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }
}
