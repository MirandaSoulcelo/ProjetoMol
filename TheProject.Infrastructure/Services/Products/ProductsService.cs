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








         public async Task<Response<Products>> Update(long id, int categoryId, string name, decimal unitPrice, int stockQuantity, bool status)
        {
            Response<Products> response = new Response<Products>();
            try
            {
                // VALIDAÇÃO 1: ID do produto
                if (id <= 0)
                {
                    response.Message = "Código do produto não informado ou inválido";
                    response.Status = false;
                    return response;
                }

                // VALIDAÇÃO 2: CategoryId
                if (categoryId <= 0)
                {
                    response.Message = "Código da categoria não informado ou inválido";
                    response.Status = false;
                    return response;
                }

                // VALIDAÇÃO 3: Nome preenchido
                if (string.IsNullOrWhiteSpace(name))
                {
                    response.Message = "Nome não informado ou inválido";
                    response.Status = false;
                    return response;
                }

                // VALIDAÇÃO 4: UnitPrice maior que 0
                if (unitPrice <= 0)
                {
                    response.Message = "Preço Unitário não informado ou inválido";
                    response.Status = false;
                    return response;
                }

                // VALIDAÇÃO 5: StockQuantity maior ou igual a 0
                if (stockQuantity < 0)
                {
                    response.Message = "Quantidade em estoque não informada ou inválida";
                    response.Status = false;
                    return response;
                }

                // VALIDAÇÃO 6: Verificar se o produto existe
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    response.Message = "Produto informado não encontrado";
                    response.Status = false;
                    return response;
                }

                // VALIDAÇÃO 7: Verificar se a categoria existe
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == categoryId);
                if (!categoryExists)
                {
                    response.Message = "Categoria informada não encontrada";
                    response.Status = false;
                    return response;
                }

                // VALIDAÇÃO 8: Verificar duplicidade de nome (excluindo o produto atual)
                var duplicateProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower() && p.Id != id);
                
                if (duplicateProduct != null)
                {
                    response.Message = "Já existe um produto para o nome informado";
                    response.Status = false;
                    return response;
                }

                // ATUALIZAR O PRODUTO
                existingProduct.CategoryId = categoryId;
                existingProduct.Name = name.Trim();
                existingProduct.UnitPrice = (double)unitPrice;
                existingProduct.StockQuantity = stockQuantity;
                existingProduct.Status = status;
                //existingProduct.UpdatedAt = DateTime.UtcNow;

                // Salvar no banco
                await _context.SaveChangesAsync();

                // Retornar o produto atualizado
                response.Data = existingProduct;
                response.Message = "Produto atualizado com sucesso";
                response.Status = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }
    }
}
    
