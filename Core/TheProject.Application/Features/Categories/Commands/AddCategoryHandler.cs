using MediatR;
using TheProject.Application.DTOs;
using TheProject.Application.Interfaces;
using TheProject.Domain.Entities;

namespace TheProject.Application.Features.Categories.Commands.AddCategory
{
    public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, Response<CategoryDto>>
    {
        private readonly ICategoriesInterface _categoriesService;

        public AddCategoryHandler(ICategoriesInterface categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<Response<CategoryDto>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            // Vou ter que usar meu service pra não dar referência circular :(. Como construi o projeto pensando em infrastructure 
            //usando aplication, meio que com o mediatr eu quebraria isso nessa altura do desenvolvimento, mas estou usando um paliativo que é suar o sreviço para lógica e o handler como ponte
            return await _categoriesService.Add(request.Name);
        }
    }
}