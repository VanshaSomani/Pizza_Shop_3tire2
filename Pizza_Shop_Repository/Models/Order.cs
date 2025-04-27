using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("orders")]
public partial class Order
{
    [Key]
    [Column("orderid")]
    public int Orderid { get; set; }

    [Column("customerid")]
    public int Customerid { get; set; }

    [Column("order_date", TypeName = "timestamp without time zone")]
    public DateTime OrderDate { get; set; }

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
    [Column("status")]
    [StringLength(30)]
    public string Status { get; set; }

    public int? OrderPaymentId { get; set; }

    public int? RatingId { get; set; }

    [StringLength(700)]
    public string OrderInstruction { get; set; }

    public int? NoOfPerson { get; set; }

    [ForeignKey("Customerid")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Order")]
    public virtual ICollection<InvoceTax> InvoceTaxes { get; set; } = new List<InvoceTax>();

    [InverseProperty("Order")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [InverseProperty("Order")]
    public virtual ICollection<Kot> Kots { get; set; } = new List<Kot>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("OrderPaymentId")]
    [InverseProperty("Orders")]
    public virtual OrderPayment OrderPayment { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderTableMapping> OrderTableMappings { get; set; } = new List<OrderTableMapping>();

    [ForeignKey("RatingId")]
    [InverseProperty("Orders")]
    public virtual Rating Rating { get; set; }
}
