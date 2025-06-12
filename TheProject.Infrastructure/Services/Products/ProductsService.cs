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
        public async Task<Response<List<ProductsDTO>>> GetAll(string? search = null, int page = 1, int pageSize = 10)
        {
            Response<List<ProductsDTO>> response = new Response<List<ProductsDTO>>();

            try
            {
                var query = _context.Products.AsQueryable();

                // Filtro de busca no Name
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.Name.Contains(search));
                }

                // Paginação
                var products = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new ProductsDTO
                    {
                        Id = (int)p.Id,
                        Name = p.Name,
                        StockQuantity = p.StockQuantity
                    })
            .ToListAsync();

                response.Data = products;
                response.Message = "Produtos coletados com filtro e paginação";

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

        

        public async Task<Response<Products>> Add(ProductUptadeDTO dto)
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


            // Validações

            /*
            if (request.CategoryId <= 0)
            {
                response.Status = false;
                response.Message = "Código da categoria não informado ou inválido";
                return response;
            }

            */

            /*
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                response.Status = false;
                response.Message = "Nome não informado ou inválido";
                return response;
            }

            */

            /*
            if (request.UnitPrice <= 0)
            {
                response.Status = false;
                response.Message = "Preço Unitário não informado ou inválido";
                return response;
            }

            */

            /*
            if (request.StockQuantity < 0)
            {
                response.Status = false;
                response.Message = "Quantidade em estoque não informada ou inválida";
                return response;
            }

            */

            // Verifica se a categoria existe
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                response.Status = false;
                response.Message = "Categoria informada não encontrada";
                return response;
            }

            // Verifica duplicidade pelo nome
            var productExists = await _context.Products.AnyAsync(p => p.Name == dto.Name);
            if (productExists)
            {
                response.Status = false;
                response.Message = "Já existe um produto para o nome informado";
                return response;
            }

            // Criar o produto
            var product = new Products
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                UnitPrice = (double)dto.UnitPrice,
                StockQuantity = dto.StockQuantity,
                Status = dto.Status
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            response.Data = product;
            response.Message = "Produto adicionado com sucesso";
            return response;
        }

    
       
    }
}
    
