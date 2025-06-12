using Microsoft.EntityFrameworkCore;
using TheProject.Application.DTOs;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;
using TheProject.Infrastructure.Data;

namespace TheProject.Infrastructure.Services.Categories
{
    public class CategoriesService : ICategoriesInterface
    {
        private readonly AppDbContext _context;

        public CategoriesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<CategoryDto>>> GetAll()
        {
            Response<List<CategoryDto>> response = new Response<List<CategoryDto>>();
            try
            {
                // Recuperar todas as categorias e mapear para CategoryDto (apenas Id e Name)
                var categories = await _context.Categories
                    .Select(c => new CategoryDto 
                    { 
                        Id = (int)c.Id, 
                        Name = c.Name 
                    })
                    .ToListAsync();

                response.Data = categories;
                response.Message = "Categorias coletadas";
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

        public async Task<Response<CategoryDto>> Add(string name)
        {
            Response<CategoryDto> response = new Response<CategoryDto>();
            try
            {
                // Validação: Nome não informado ou inválido
                if (string.IsNullOrWhiteSpace(name))
                {
                    response.Message = "Nome não informado ou inválido";
                    response.Status = false;
                    return response;
                }

                // Validação: Verificar duplicidade
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());

                if (existingCategory != null)
                {
                    response.Message = "Já existe uma categoria para o nome informado";
                    response.Status = false;
                    return response;
                }

                // Criar nova categoria
                var newCategory = new Domain.Entities.Categories
                {
                    Name = name.Trim(),
                   
                };

                // Salvar no banco
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();

                // Retornar a categoria criada (apenas Id e Name)
                response.Data = new CategoryDto 
                { 
                    Id = (int)newCategory.Id, 
                    Name = newCategory.Name 
                };
                response.Message = "Categoria criada com sucesso";
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