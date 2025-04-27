using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("customerhistory")]
public partial class Customerhistory
{
    [Column("customerid")]
    public int? Customerid { get; set; }

    [Column("customername")]
    [StringLength(50)]
    public string Customername { get; set; }

    [Column("phoneno")]
    [Precision(15, 0)]
    public decimal? Phoneno { get; set; }

    [Column("maxorder")]
    public int? Maxorder { get; set; }

    [Column("avgbill")]
    public int? Avgbill { get; set; }

    [Column("comingsince", TypeName = "timestamp without time zone")]
    public DateTime? Comingsince { get; set; }

    [Column("visits")]
    public int? Visits { get; set; }

    [Column("orderdate", TypeName = "timestamp without time zone")]
    public DateTime? Orderdate { get; set; }

    [Column("items")]
    public int? Items { get; set; }

    [Column("amount")]
    public int? Amount { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Key]
    [Column("cutomerhistoryid")]
    public int Cutomerhistoryid { get; set; }

    [ForeignKey("Customerid")]
    [InverseProperty("Customerhistories")]
    public virtual Customer Customer { get; set; }
}
