using TheProject.Application.DTOs;
using TheProject.Domain.Entities;

namespace TheProject.Application.Interfaces
{
    public interface IProductsInterface
    {
        Task<Response<List<ProductsDTO>>> GetAll(string? search = null, int page = 1, int pageSize = 10);
        Task<Response<Products>> Update(ProductUptadeDTO dto);

        Task<Response<Products>> Add(ProductUptadeDTO dto);
        
        Task<Response<bool>> Delete(ProductDeleteDTO dto);

    }
}