using System.ComponentModel.DataAnnotations;
using Pizza_Shop_Repository.Models;

namespace Pizza_Shop_Repository.ViewModels;

public class TableSectionViewModel
{
    public List<Section> SectionList { get; set; }

    public List<Stable> TableList { get; set; }

    public PagginationViewModel TablePagination { get; set; }

    [Required (ErrorMessage = "Enter Section Name")]
    public string SectionName { get; set; }

    [Required (ErrorMessage = "Enter Description")]
    public string SectionDesc { get; set; }

    [Required (ErrorMessage = "Select Section")]
    [Range(1 , int.MaxValue , ErrorMessage ="Select Section")]
    public int SectionId { get; set; }

    [Required(ErrorMessage = "Enter Table Name")]
    public string TableName { get; set; }

    [Required (ErrorMessage = "Enter Capacity")]
    [Range(1,int.MaxValue , ErrorMessage = "Table Capacity Not Valid")]
    public int TableCapacity { get; set; }

    [Required (ErrorMessage = "Select Status")]
    public string TableStatus { get; set; }

    public int TableId { get; set; }
}
