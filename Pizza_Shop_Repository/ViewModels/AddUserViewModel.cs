using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Pizza_Shop_Repository.ViewModels;

public class AddUserViewModel
{
    [Required(ErrorMessage ="Enter First Name"), MaxLength(50 , ErrorMessage ="Length exceed")]
    public string Firstname { get; set; }

    [Required(ErrorMessage ="Enter Last Name"), MaxLength(50 , ErrorMessage ="Length exceed")]
    public string Lastname { get; set; }

    [Required(ErrorMessage ="Enter User Name"), MaxLength(50 , ErrorMessage ="Length exceed")]
    public string Username { get; set; }

    [Required]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
    public decimal Phoneno { get; set; }

    [Required(ErrorMessage ="Enter Address"), MaxLength(500 , ErrorMessage ="Length exceed")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Country is Required")]
    public int Countryid { get; set; }

    [Required(ErrorMessage = "State is Required")]
    public int Stateid { get; set; }

    [Required(ErrorMessage = "City is Required")]
    public int Cityid { get; set; }

    [Required]
    public string Passwordhashed { get; set; }

    [Required]
    public IFormFile profileimg { get; set; }

    [Required(ErrorMessage ="Enter Zipcode"), MaxLength(10 , ErrorMessage ="Length exceed")]
    public string Zipcode { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public int Roleid { get; set; }

    [Required , MaxLength(100 , ErrorMessage ="Length exceed")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }

    [Required]
    public int Createdby { get; set; }
}
