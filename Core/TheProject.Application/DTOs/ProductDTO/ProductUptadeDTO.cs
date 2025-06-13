
using System.ComponentModel.DataAnnotations;

namespace TheProject.Application.DTOs
{
    public class ProductUptadeDTO
    {

        [Required]
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool Status { get; set; }
    }
}