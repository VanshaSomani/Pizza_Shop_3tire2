using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("resetpassword")]
public partial class Resetpassword
{
    [Column("password")]
    [StringLength(600)]
    public string Password { get; set; }

    [Column("confirmepassword")]
    [StringLength(600)]
    public string Confirmepassword { get; set; }

    [Column("resettoken")]
    [StringLength(600)]
    public string Resettoken { get; set; }

    [Column("resettokenvalidity")]
    public DateTime? Resettokenvalidity { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; }

    [Key]
    [Column("resetpasswordid")]
    public int Resetpasswordid { get; set; }
}
