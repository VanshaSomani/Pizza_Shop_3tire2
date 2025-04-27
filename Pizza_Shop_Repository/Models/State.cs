using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("states")]
public partial class State
{
    [Key]
    [Column("stateid")]
    public int Stateid { get; set; }

    [Required]
    [Column("state_name")]
    [StringLength(100)]
    public string StateName { get; set; }

    [Column("countryid")]
    public int Countryid { get; set; }

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

    [InverseProperty("State")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [ForeignKey("Countryid")]
    [InverseProperty("States")]
    public virtual Country Country { get; set; }

    [InverseProperty("State")]
    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
