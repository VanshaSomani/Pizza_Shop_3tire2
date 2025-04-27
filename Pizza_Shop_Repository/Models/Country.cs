using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("country")]
public partial class Country
{
    [Key]
    [Column("countryid")]
    public int Countryid { get; set; }

    [Required]
    [Column("country_name")]
    [StringLength(100)]
    public string CountryName { get; set; }

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

    [InverseProperty("Country")]
    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    [InverseProperty("Country")]
    public virtual ICollection<State> States { get; set; } = new List<State>();
}
