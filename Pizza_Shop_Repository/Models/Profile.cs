using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("profile")]
public partial class Profile
{
    [Key]
    [Column("userid")]
    public int Userid { get; set; }

    [Required]
    [Column("firstname")]
    [StringLength(50)]
    public string Firstname { get; set; }

    [Column("lastname")]
    [StringLength(50)]
    public string Lastname { get; set; }

    [Required]
    [Column("username")]
    [StringLength(50)]
    public string Username { get; set; }

    [Column("phoneno")]
    [Precision(15, 0)]
    public decimal Phoneno { get; set; }

    [Required]
    [Column("address")]
    [StringLength(500)]
    public string Address { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Required]
    [Column("zipcode", TypeName = "character varying")]
    public string Zipcode { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Column("countryid")]
    public int Countryid { get; set; }

    [Column("stateid")]
    public int Stateid { get; set; }

    [Column("cityid")]
    public int Cityid { get; set; }

    [Required]
    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; }

    [Column("profileimg")]
    [StringLength(500)]
    public string Profileimg { get; set; }

    [Column("status")]
    public bool? Status { get; set; }

    [ForeignKey("Cityid")]
    [InverseProperty("Profiles")]
    public virtual City City { get; set; }

    [ForeignKey("Countryid")]
    [InverseProperty("Profiles")]
    public virtual Country Country { get; set; }

    [ForeignKey("Stateid")]
    [InverseProperty("Profiles")]
    public virtual State State { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
