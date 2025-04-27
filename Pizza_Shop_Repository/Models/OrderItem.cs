using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("order_item")]
public partial class OrderItem
{
    [Key]
    [Column("orderitemid")]
    public int Orderitemid { get; set; }

    [Column("itemid")]
    public int Itemid { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("orderid")]
    public int Orderid { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Column("amount", TypeName = "money")]
    public decimal? Amount { get; set; }

    [Column("itemtaxpercentage", TypeName = "money")]
    public decimal? Itemtaxpercentage { get; set; }

    [StringLength(700)]
    public string OrderItemInstruction { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; }

    [Column("ready_item")]
    public int? ReadyItem { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Itemid")]
    [InverseProperty("OrderItems")]
    public virtual Item Item { get; set; }

    [ForeignKey("Orderid")]
    [InverseProperty("OrderItems")]
    public virtual Order Order { get; set; }

    [InverseProperty("OrderItem")]
    public virtual ICollection<OrderItemModifier> OrderItemModifiers { get; set; } = new List<OrderItemModifier>();
}
