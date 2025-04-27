using System.ComponentModel.DataAnnotations;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.ViewModels;

public class TaxesAndFeesViewModel
{
    public IEnumerable<Tax> TaxList { get; set; }

    public PagginationViewModel TaxPaggination { get; set; }

    public int TaxId { get; set; }

    [Required]
    public string TaxName { get; set; }

    [Required]
    public string TaxType { get; set; }

    [Required]
    [Range(1,int.MaxValue , ErrorMessage = "Tax amount should not be zero")]
    public int TaxAmount { get; set; }

    [Required]
    public bool IsEnabled { get; set; }

    [Required]
    public bool DefaultTax { get; set; }
}
