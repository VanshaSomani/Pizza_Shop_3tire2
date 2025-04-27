using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("permissions")]
public partial class Permission
{
    [Key]
    [Column("permissionid")]
    public int Permissionid { get; set; }

    [Required]
    [Column("pemission")]
    [StringLength(50)]
    public string Pemission { get; set; }

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

    [InverseProperty("Permission")]
    public virtual ICollection<Roleandpermission> Roleandpermissions { get; set; } = new List<Roleandpermission>();
}
