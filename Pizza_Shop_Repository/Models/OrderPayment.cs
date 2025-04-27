using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("order_payment")]
public partial class OrderPayment
{
    [Key]
    public int PaymentId { get; set; }

    [Column(TypeName = "money")]
    public decimal TotalAmount { get; set; }

    public int? CardDetails { get; set; }

    [Column(TypeName = "character varying")]
    public string UpiPin { get; set; }

    [Column("paymenttype")]
    [StringLength(30)]
    public string Paymenttype { get; set; }

    [Column("paidon", TypeName = "timestamp without time zone")]
    public DateTime? Paidon { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [InverseProperty("OrderPayment")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
