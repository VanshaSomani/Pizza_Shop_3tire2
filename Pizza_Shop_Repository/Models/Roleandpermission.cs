using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("roleandpermission")]
public partial class Roleandpermission
{
    [Key]
    [Column("rpid")]
    public int Rpid { get; set; }

    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("permissionid")]
    public int Permissionid { get; set; }

    [Required]
    [Column("viewpermission")]
    public bool? Viewpermission { get; set; }

    [Required]
    [Column("addandeditpermission")]
    public bool? Addandeditpermission { get; set; }

    [Required]
    [Column("isdeletepermission")]
    public bool? Isdeletepermission { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Column("isselected")]
    public bool? Isselected { get; set; }

    [ForeignKey("Permissionid")]
    [InverseProperty("Roleandpermissions")]
    public virtual Permission Permission { get; set; }

    [ForeignKey("Roleid")]
    [InverseProperty("Roleandpermissions")]
    public virtual Role Role { get; set; }
}
