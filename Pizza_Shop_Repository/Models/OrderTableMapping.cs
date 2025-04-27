using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("order_table_mapping")]
public partial class OrderTableMapping
{
    [Key]
    [Column("order_table_id")]
    public int OrderTableId { get; set; }

    [Column("orderId")]
    public int? OrderId { get; set; }

    [Column("tableId")]
    public int TableId { get; set; }

    [Column("customerid")]
    public int Customerid { get; set; }

    [Column("isdeleted")]
    public bool Isdeleted { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Customerid")]
    [InverseProperty("OrderTableMappings")]
    public virtual Customer Customer { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderTableMappings")]
    public virtual Order Order { get; set; }

    [ForeignKey("TableId")]
    [InverseProperty("OrderTableMappings")]
    public virtual Stable Table { get; set; }
}
