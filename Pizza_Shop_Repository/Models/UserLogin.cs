using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("user_login")]
public partial class UserLogin
{
    [Key]
    [Column("userloginid")]
    public int Userloginid { get; set; }

    [Required]
    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [Column("passwordhashed")]
    [StringLength(600)]
    public string Passwordhashed { get; set; }

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

    [Column("userid")]
    public int Userid { get; set; }

    [ForeignKey("Userid")]
    [InverseProperty("UserLogins")]
    public virtual Profile User { get; set; }
}
