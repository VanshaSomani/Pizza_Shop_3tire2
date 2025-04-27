using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("Item_modifier_group")]
public partial class ItemModifierGroup
{
    [Key]
    [Column("itemMGid")]
    public int ItemMgid { get; set; }

    [Column("itemId")]
    public int ItemId { get; set; }

    [Column("modifierGroupId")]
    public int ModifierGroupId { get; set; }

    [Column("min")]
    public int Min { get; set; }

    [Column("max")]
    public int Max { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemModifierGroups")]
    public virtual Item Item { get; set; }

    [ForeignKey("ModifierGroupId")]
    [InverseProperty("ItemModifierGroups")]
    public virtual Modifiergroup ModifierGroup { get; set; }
}
