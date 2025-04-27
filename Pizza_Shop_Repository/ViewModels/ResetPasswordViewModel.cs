using System.ComponentModel.DataAnnotations;

namespace Pizza_Shop_Repository.ViewModels;

public class ResetPasswordViewModel
{
    [Required]
    public string Email { get; set; }

    [Required(ErrorMessage = "Enter Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Enter Password"), Compare("Password" , ErrorMessage = "Confirm password and new password does'nt match.")]
    public string ConfirmePassword { get; set; }
}
