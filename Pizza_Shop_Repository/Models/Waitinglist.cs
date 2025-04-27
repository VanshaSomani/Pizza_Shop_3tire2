using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("waitinglist")]
public partial class Waitinglist
{
    [Key]
    [Column("waitinglistid")]
    public int Waitinglistid { get; set; }

    [Column("peoples")]
    public int Peoples { get; set; }

    [Column("sectionid")]
    public int Sectionid { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("isassigned")]
    public bool Isassigned { get; set; }

    [Column("tableid")]
    public int? Tableid { get; set; }

    [Column("customerid")]
    public int Customerid { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Customerid")]
    [InverseProperty("Waitinglists")]
    public virtual Customer Customer { get; set; }

    [ForeignKey("Sectionid")]
    [InverseProperty("Waitinglists")]
    public virtual Section Section { get; set; }

    [ForeignKey("Tableid")]
    [InverseProperty("Waitinglists")]
    public virtual Stable Table { get; set; }
}
