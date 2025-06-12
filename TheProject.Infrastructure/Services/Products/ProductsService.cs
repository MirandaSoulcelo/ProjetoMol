using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TheProject.Application.DTOs;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;
using TheProject.Infrastructure.Data;

namespace TheProject.Infrastructure.Services.Product
{
    public class ProductsService : IProductsInterface
    {

        private readonly AppDbContext _context;
        private readonly IValidator<ProductUptadeDTO> _validator;

        public ProductsService(AppDbContext context, IValidator<ProductUptadeDTO> validator)
        {
            _context = context;
            _validator = validator;
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



        public async Task<Response<Products>> Update(ProductUptadeDTO dto)
        {
            var response = new Response<Products>();

            // Validação SINCRONA do DTO via FluentValidation (regras locais)
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                response.Status = false;
                response.Message = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                return response;
            }

            // Validação ASSÍNCRONA: Produto existe?
            var product = await _context.Products.FindAsync(dto.Id);
            if (product == null)
            {
                response.Status = false;
                response.Message = "Produto informado não encontrado.";
                return response;
            }

            // Validação ASSÍNCRONA: Categoria existe?
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                response.Status = false;
                response.Message = "Categoria informada não encontrada.";
                return response;
            }

            // Validação ASSÍNCRONA: Nome duplicado (exceto o próprio produto)
            var duplicateProduct = await _context.Products
                .AnyAsync(p => p.Name.ToLower() == dto.Name.ToLower() && p.Id != dto.Id);
            if (duplicateProduct)
            {
                response.Status = false;
                response.Message = "Já existe um produto com o nome informado.";
                return response;
            }

            // Atualizar produto
            product.CategoryId = dto.CategoryId;
            product.Name = dto.Name.Trim();
            product.UnitPrice = (double)dto.UnitPrice;
            product.StockQuantity = dto.StockQuantity;
            product.Status = dto.Status;

            await _context.SaveChangesAsync();

            response.Data = product;
            response.Message = "Produto atualizado com sucesso.";
            response.Status = true;
            return response;
        }

       
    }
}
    
