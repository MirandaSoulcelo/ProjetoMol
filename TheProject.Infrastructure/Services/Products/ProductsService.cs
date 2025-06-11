using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;
using TheProject.Infrastructure.Data;

namespace TheProject.Infrastructure.Services.Products
{
    public class ProductsService : IProductsInterface
    {

        private readonly AppDbContext _context;

        public ProductsService(AppDbContext context)
        {
            _context = context;
         }
        public Task<Response<List<Domain.Entities.Products>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Response<Domain.Entities.Products>> GetByCategoryId(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Domain.Entities.Products>> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}