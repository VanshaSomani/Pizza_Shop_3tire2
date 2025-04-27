using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("customer")]
public partial class Customer
{
    [Key]
    [Column("customerid")]
    public int Customerid { get; set; }

    [Required]
    [Column("customername")]
    [StringLength(50)]
    public string Customername { get; set; }

    [Required]
    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; }

    [Column("phoneno")]
    [Precision(10, 0)]
    public decimal Phoneno { get; set; }

    [Column("cust_date", TypeName = "timestamp without time zone")]
    public DateTime CustDate { get; set; }

    [Column("totalorder")]
    public int Totalorder { get; set; }

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

    [InverseProperty("Customer")]
    public virtual ICollection<Customerhistory> Customerhistories { get; set; } = new List<Customerhistory>();

    [InverseProperty("Customer")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Customer")]
    public virtual ICollection<OrderTableMapping> OrderTableMappings { get; set; } = new List<OrderTableMapping>();

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Customer")]
    public virtual ICollection<Waitinglist> Waitinglists { get; set; } = new List<Waitinglist>();
}
