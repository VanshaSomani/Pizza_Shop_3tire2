using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("modifiergroup")]
public partial class Modifiergroup
{
    [Key]
    [Column("modifiergroupid")]
    public int Modifiergroupid { get; set; }

    [Required]
    [Column("mgname")]
    [StringLength(50)]
    public string Mgname { get; set; }

    [Column("mgdesc")]
    [StringLength(500)]
    public string Mgdesc { get; set; }

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

    [InverseProperty("ModifierGroup")]
    public virtual ICollection<ItemModifierGroup> ItemModifierGroups { get; set; } = new List<ItemModifierGroup>();

    [InverseProperty("ModifierGroup")]
    public virtual ICollection<ModifierModifierGroupMapping> ModifierModifierGroupMappings { get; set; } = new List<ModifierModifierGroupMapping>();

    [InverseProperty("ModifierGroup")]
    public virtual ICollection<OrderItemModifier> OrderItemModifiers { get; set; } = new List<OrderItemModifier>();
}
