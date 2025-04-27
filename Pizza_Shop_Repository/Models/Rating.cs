using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("rating")]
public partial class Rating
{
    [Key]
    [Column("ratindid")]
    public int Ratindid { get; set; }

    [Column("food")]
    public int Food { get; set; }

    [Column("ambiance")]
    public int Ambiance { get; set; }

    [Column("service")]
    public int Service { get; set; }

    [Column("avgratting")]
    public int Avgratting { get; set; }

    [InverseProperty("Rating")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
