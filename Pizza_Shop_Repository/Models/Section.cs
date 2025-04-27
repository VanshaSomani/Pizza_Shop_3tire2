using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("sections")]
public partial class Section
{
    [Key]
    [Column("sectionid")]
    public int Sectionid { get; set; }

    [Required]
    [Column("sectionname")]
    [StringLength(50)]
    public string Sectionname { get; set; }

    [Column("sectiondesc")]
    [StringLength(500)]
    public string Sectiondesc { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat")]
    public TimeOnly? Updatedat { get; set; }

    [InverseProperty("Section")]
    public virtual ICollection<Stable> Stables { get; set; } = new List<Stable>();

    [InverseProperty("Section")]
    public virtual ICollection<Waitinglist> Waitinglists { get; set; } = new List<Waitinglist>();
}
