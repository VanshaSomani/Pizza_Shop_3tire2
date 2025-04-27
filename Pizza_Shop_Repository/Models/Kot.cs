using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("kot")]
public partial class Kot
{
    [Key]
    [Column("kotid")]
    public int Kotid { get; set; }

    [Column("categoryid")]
    public int Categoryid { get; set; }

    [Column("orderid")]
    public int Orderid { get; set; }

    [Column("inprogress")]
    public bool Inprogress { get; set; }

    [Column("ready")]
    public bool Ready { get; set; }

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

    [ForeignKey("Categoryid")]
    [InverseProperty("Kots")]
    public virtual Category Category { get; set; }

    [ForeignKey("Orderid")]
    [InverseProperty("Kots")]
    public virtual Order Order { get; set; }
}
