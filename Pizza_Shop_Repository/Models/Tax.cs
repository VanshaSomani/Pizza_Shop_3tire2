using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("tax")]
public partial class Tax
{
    [Key]
    [Column("taxid")]
    public int Taxid { get; set; }

    [Required]
    [Column("taxname")]
    [StringLength(50)]
    public string Taxname { get; set; }

    [Column("isenable")]
    public bool Isenable { get; set; }

    [Column("defaulttax")]
    public bool Defaulttax { get; set; }

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

    [Column("taxamount")]
    [Precision(5, 2)]
    public decimal? Taxamount { get; set; }

    [Column("taxtype")]
    [StringLength(100)]
    public string Taxtype { get; set; }

    [InverseProperty("Tax")]
    public virtual ICollection<InvoceTax> InvoceTaxes { get; set; } = new List<InvoceTax>();
}
