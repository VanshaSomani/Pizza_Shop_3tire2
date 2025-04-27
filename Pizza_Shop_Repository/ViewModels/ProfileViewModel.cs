using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Pizza_Shop_Repository.ViewModels;

public class ProfileViewModel
{
    [Required]
    public int Userid { get; set; }
    [Required]
    public string Firstname { get; set; }
    [Required]
    public string Lastname { get; set; }
    [Required]
    public string Username { get; set; }
    [Required , Phone]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
    public decimal Phoneno { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Zipcode { get; set; }
    [Required]
    public int Countryid { get; set; }
    // [Required]
    public string CountryName { get; set; }
    [Required]
    public int Stateid { get; set; }
    // [Required]
    public string StateName { get; set; }
    [Required]
    public int Cityid { get; set; }
    // [Required]
    public string CityName { get; set; }
    [Required , EmailAddress]
    public string Email { get; set; }

    public string ExistingProfileImg { get; set; }

    public IFormFile ProfileImg { get; set; }
    public string RoleName { get; set; }
}
