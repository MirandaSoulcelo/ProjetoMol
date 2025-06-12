using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheProject.Domain.Entities;

namespace TheProject.Application.Interfaces
{
    public interface IProductsInterface
    {
        //metodo assincrono, que retorna um task
        Task<Response<List<Products>>> GetAll();
        Task<Response<Products>> GetById(int id);
        Task<Response<Products>> GetByCategoryId(int CategoryId);
        
        Task<Response<Products>> Update(long id, int categoryId, string name, decimal unitPrice, int stockQuantity, bool status);
    }
}