using MediatR;
using TheProject.Application.DTOs;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;

namespace TheProject.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Response<List<CategoryDto>>>
    {
        private readonly ICategoriesInterface _categoriesService;

        public GetAllCategoriesHandler(ICategoriesInterface categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<Response<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            // É aqui que eu uso o paliativo. Ainda estou usando meu service.
            return await _categoriesService.GetAll();
        }
    }
}
