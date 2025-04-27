using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("city")]
public partial class City
{
    [Key]
    [Column("cityid")]
    public int Cityid { get; set; }

    [Required]
    [Column("city_name")]
    [StringLength(100)]
    public string CityName { get; set; }

    [Column("stateid")]
    public int Stateid { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat")]
    public DateTime? Updatedat { get; set; }

    [InverseProperty("City")]
    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    [ForeignKey("Stateid")]
    [InverseProperty("Cities")]
    public virtual State State { get; set; }
}
