using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("stables")]
public partial class Stable
{
    [Key]
    [Column("tableid")]
    public int Tableid { get; set; }

    [Column("sectionid")]
    public int Sectionid { get; set; }

    [Column("capacity")]
    public int Capacity { get; set; }

    [Column("tablename")]
    [StringLength(50)]
    public string Tablename { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat")]
    public TimeOnly? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Required]
    [Column("table_status")]
    [StringLength(50)]
    public string TableStatus { get; set; }

    [InverseProperty("Table")]
    public virtual ICollection<OrderTableMapping> OrderTableMappings { get; set; } = new List<OrderTableMapping>();

    [ForeignKey("Sectionid")]
    [InverseProperty("Stables")]
    public virtual Section Section { get; set; }

    [InverseProperty("Table")]
    public virtual ICollection<Waitinglist> Waitinglists { get; set; } = new List<Waitinglist>();
}
