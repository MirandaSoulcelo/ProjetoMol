using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;
using TheProject.Infrastructure.Data;

namespace TheProject.Infrastructure.Services.Product
{
    public class ProductsService : IProductsInterface
    {

        private readonly AppDbContext _context;

        public ProductsService(AppDbContext context)
        {
            _context = context;
         }

         //por quê async?
        public async Task<Response<List<Domain.Entities.Products>>> GetAll()
        {
            Response<List<Products>> response = new Response<List<Products>>();
            try
            {
                //transformando em lista todos os produtos do banco e esperando pegar tudo para depois continuar
                var products = await _context.Products.ToListAsync();


                response.Data = products;
                response.Message = "Produtos coletados";

                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
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