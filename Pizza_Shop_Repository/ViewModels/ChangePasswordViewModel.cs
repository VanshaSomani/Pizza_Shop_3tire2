using System.ComponentModel.DataAnnotations;

namespace Pizza_Shop_Repository.ViewModels;

public class ChangePasswordViewModel
{
    [Required (ErrorMessage ="Enter Password")]
    public string OldPassword { get; set; }

    [Required (ErrorMessage ="Enter Password")]
    public string NewPassword { get; set; }

    [Required (ErrorMessage ="Enter Password") , Compare("NewPassword" , ErrorMessage = "Confirm password and new does'nt match.")]
    public string ConfirmePassword { get; set; }
}
