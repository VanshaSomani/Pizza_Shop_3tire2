using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("feedback")]
public partial class Feedback
{
    [Key]
    [Column("feedbackid")]
    public int Feedbackid { get; set; }

    [Column("orderid")]
    public int Orderid { get; set; }

    [Column("customerid")]
    public int Customerid { get; set; }

    [Column("food")]
    public int? Food { get; set; }

    [Column("ambiance")]
    public int? Ambiance { get; set; }

    [Column("service")]
    public int? Service { get; set; }

    [ForeignKey("Customerid")]
    [InverseProperty("Feedbacks")]
    public virtual Customer Customer { get; set; }

    [ForeignKey("Orderid")]
    [InverseProperty("Feedbacks")]
    public virtual Order Order { get; set; }
}
