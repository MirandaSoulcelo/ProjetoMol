using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TheProject.Domain.Entities;

public class Categories
{
    public Categories()
    {
        Products = new HashSet<Products>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;


    [JsonIgnore]
    [InverseProperty(nameof(Entities.Products.Category))]
    public virtual ICollection<Products> Products { get; set; }
}
