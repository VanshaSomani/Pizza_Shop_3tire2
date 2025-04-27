using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("item")]
public partial class Item
{
    [Key]
    [Column("itemid")]
    public int Itemid { get; set; }

    [Column("categoryid")]
    public int Categoryid { get; set; }

    [Required]
    [Column("itemname")]
    [StringLength(50)]
    public string Itemname { get; set; }

    [Column("rate")]
    public int Rate { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("unit")]
    [StringLength(50)]
    public string Unit { get; set; }

    [Column("availlable")]
    public bool Availlable { get; set; }

    [Column("defaulttax")]
    public bool Defaulttax { get; set; }

    [Column("taxpercentage")]
    public double? Taxpercentage { get; set; }

    [Required]
    [Column("code")]
    [StringLength(10)]
    public string Code { get; set; }

    [Column("itemdesc")]
    [StringLength(500)]
    public string Itemdesc { get; set; }

    [Column("imgfile")]
    [StringLength(500)]
    public string Imgfile { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Required]
    [Column("item_type", TypeName = "character varying")]
    public string ItemType { get; set; }

    [Column("isfavourite")]
    public bool Isfavourite { get; set; }

    [ForeignKey("Categoryid")]
    [InverseProperty("Items")]
    public virtual Category Category { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<FavroiteItem> FavroiteItems { get; set; } = new List<FavroiteItem>();

    [InverseProperty("Item")]
    public virtual ICollection<ItemModifierGroup> ItemModifierGroups { get; set; } = new List<ItemModifierGroup>();

    [InverseProperty("Item")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
