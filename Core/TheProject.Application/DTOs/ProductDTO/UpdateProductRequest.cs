
namespace TheProject.Application.DTOs
{
   
    // Classe auxiliar para receber o request do Update
    public class UpdateProductRequest
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool Status { get; set; }
    }
}
