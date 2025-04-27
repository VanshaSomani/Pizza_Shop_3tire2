using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("user_role")]
public partial class UserRole
{
    [Key]
    [Column("userroleid")]
    public int Userroleid { get; set; }

    [Column("userid")]
    public int Userid { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Roleid")]
    [InverseProperty("UserRoles")]
    public virtual Role Role { get; set; }

    [ForeignKey("Userid")]
    [InverseProperty("UserRoles")]
    public virtual Profile User { get; set; }
}
