using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("modifier")]
public partial class Modifier
{
    [Key]
    [Column("modifierid")]
    public int Modifierid { get; set; }

    [Required]
    [Column("modifiername")]
    [StringLength(50)]
    public string Modifiername { get; set; }

    [Column("rate")]
    public int Rate { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("unit")]
    [StringLength(50)]
    public string Unit { get; set; }

    [Column("modifierdesc")]
    [StringLength(500)]
    public string Modifierdesc { get; set; }

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

    [InverseProperty("Modifier")]
    public virtual ICollection<ModifierModifierGroupMapping> ModifierModifierGroupMappings { get; set; } = new List<ModifierModifierGroupMapping>();

    [InverseProperty("Modifier")]
    public virtual ICollection<OrderItemModifier> OrderItemModifiers { get; set; } = new List<OrderItemModifier>();
}
