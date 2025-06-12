using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheProject.Application.DTOs;
using TheProject.Domain.Entities;

namespace TheProject.Application.Interfaces
{
    public interface IProductsInterface
    {
        //metodo assincrono, que retorna um task

        Task<Response<List<ProductsDTO>>> GetAll(string? search = null, int page = 1, int pageSize = 10);
        Task<Response<Products>> GetById(int id);
        Task<Response<Products>> GetByCategoryId(int CategoryId);

        Task<Response<Products>> Update(ProductUptadeDTO dto);
       
        Task<Response<Products>> Add(ProductUptadeDTO dto);

    }
}