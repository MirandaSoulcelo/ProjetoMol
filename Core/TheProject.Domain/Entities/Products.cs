using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheProject.Domain.Entities;

public class Products
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool Status { get; set; }

    [ForeignKey(nameof(CategoryId))]
    [InverseProperty(nameof(Categories.Products))]
    public virtual Categories Category { get; set; } = new();
}
