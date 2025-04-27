using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("favroite_item")]
public partial class FavroiteItem
{
    [Column("itemid")]
    public int Itemid { get; set; }

    [Column("categoryid")]
    public int Categoryid { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("updatedby")]
    public int? Updatedby { get; set; }

    [Column("createdat")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Key]
    [Column("faveroititemid")]
    public int Faveroititemid { get; set; }

    [ForeignKey("Categoryid")]
    [InverseProperty("FavroiteItems")]
    public virtual Category Category { get; set; }

    [ForeignKey("Itemid")]
    [InverseProperty("FavroiteItems")]
    public virtual Item Item { get; set; }
}
