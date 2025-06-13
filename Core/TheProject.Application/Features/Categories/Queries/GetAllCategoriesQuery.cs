using MediatR;
using TheProject.Application.DTOs;
using TheProject.Domain.Entities;

namespace TheProject.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<Response<List<CategoryDto>>>
    {
        // Query vazia - Aqui vou buscar todas as categorias
    }
}