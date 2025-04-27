using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("Order_Item_Modifier")]
public partial class OrderItemModifier
{
    [Key]
    public int OrderItemModifierId { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    public int ModifierId { get; set; }

    public int ModifierGroupId { get; set; }

    public int OrderItemId { get; set; }

    public int? Quantity { get; set; }

    [Column("modifierAmount", TypeName = "money")]
    public decimal? ModifierAmount { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("ModifierId")]
    [InverseProperty("OrderItemModifiers")]
    public virtual Modifier Modifier { get; set; }

    [ForeignKey("ModifierGroupId")]
    [InverseProperty("OrderItemModifiers")]
    public virtual Modifiergroup ModifierGroup { get; set; }

    [ForeignKey("OrderItemId")]
    [InverseProperty("OrderItemModifiers")]
    public virtual OrderItem OrderItem { get; set; }
}
