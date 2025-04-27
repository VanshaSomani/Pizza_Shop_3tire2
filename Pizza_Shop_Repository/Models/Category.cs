using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("category")]
public partial class Category
{
    [Key]
    [Column("categoryid")]
    public int Categoryid { get; set; }

    [Required]
    [Column("categoryname")]
    [StringLength(50)]
    public string Categoryname { get; set; }

    [Required]
    [Column("categorydesc")]
    [StringLength(500)]
    public string Categorydesc { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat")]
    public DateTime? Updatedat { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<FavroiteItem> FavroiteItems { get; set; } = new List<FavroiteItem>();

    [InverseProperty("Category")]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    [InverseProperty("Category")]
    public virtual ICollection<Kot> Kots { get; set; } = new List<Kot>();
}
