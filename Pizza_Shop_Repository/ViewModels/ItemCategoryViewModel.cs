using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.ViewModels;

public class ItemCategoryViewModel
{
    public IEnumerable<Category> CategoryList { get; set; }
    public IEnumerable<Item> ItemList { get; set; }
    public IEnumerable<Modifiergroup> ModifiergroupList { get; set; }
    public IEnumerable<Modifier> ModifiersList { get; set; }
    public IEnumerable<ItemModifierGroup> itemModifierGroupsList { get; set; }
    public List<ItemModifierGroupMapViewModel> ItemModifierGroupMapList { get; set; }
    public PagginationViewModel ItemPaggination { get; set; }
    public PagginationViewModel ModifierPaggination { get; set; }
    [Required]
    public string Categoryname { get; set; }
    [Required]
    public string CategoryDesc { get; set; }
    [Required (ErrorMessage = "Name is required")]
    public string ModifierGroupName { get; set; }
    [Required (ErrorMessage = "Description is required")]
    public string ModifierGroupDesc { get; set; }
    public int Categoryid { get; set; }
    public int Itemid { get; set; }
    [Required]
    // [Range(1,int.MaxValue,ErrorMessage ="Select Modifier Group")]    
    public int Modifiergroupid { get; set; }
    [Required]
    public string Itemname { get; set; }
    [Required]
    [Range(1,int.MaxValue , ErrorMessage = "Enter Range")]
    public int Rate { get; set; }
    [Required]
    public string ItemType { get; set; }
    [Required]
    [Range(1,int.MaxValue , ErrorMessage = "Enter Quantity")]
    public int Quantity { get; set; }
    [Required]
    public string Unit { get; set; }
    [Required]
    public bool Availlable { get; set; }
    [Required]
    public bool Defaulttax { get; set; }
    [Required]
    [Range(1,double.MaxValue , ErrorMessage = "Enter Tax Percentage")]
    public double Taxpercentage { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    public string Itemdesc { get; set; }
    public IFormFile Imgfile { get; set; }
    public string ExistingFile { get; set; }
    [Required]
    public string Modifiername { get; set; }
    [Required]
    public string Modifierdesc { get; set; }
    public int ModifierId { get; set; }
}
