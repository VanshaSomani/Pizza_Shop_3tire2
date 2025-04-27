using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pizza_Shop_Repository.Models;

[Table("Invoce_Tax")]
public partial class InvoceTax
{
    [Key]
    public int InvoiceTaxId { get; set; }

    public int TaxId { get; set; }

    [Column("taxtype")]
    [StringLength(20)]
    public string Taxtype { get; set; }

    [Column(TypeName = "money")]
    public decimal? TaxAmount { get; set; }

    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("InvoceTaxes")]
    public virtual Order Order { get; set; }

    [ForeignKey("TaxId")]
    [InverseProperty("InvoceTaxes")]
    public virtual Tax Tax { get; set; }
}
