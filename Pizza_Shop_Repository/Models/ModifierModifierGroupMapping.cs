using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("modifier_modifier_group_mapping")]
public partial class ModifierModifierGroupMapping
{
    [Key]
    [Column("Modifier_Modifier_Group_id")]
    public int ModifierModifierGroupId { get; set; }

    public int ModifierId { get; set; }

    public int ModifierGroupId { get; set; }

    [Column("isDeleted")]
    public bool? IsDeleted { get; set; }

    [ForeignKey("ModifierId")]
    [InverseProperty("ModifierModifierGroupMappings")]
    public virtual Modifier Modifier { get; set; }

    [ForeignKey("ModifierGroupId")]
    [InverseProperty("ModifierModifierGroupMappings")]
    public virtual Modifiergroup ModifierGroup { get; set; }
}
