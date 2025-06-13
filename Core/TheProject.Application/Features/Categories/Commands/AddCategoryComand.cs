using MediatR;
using TheProject.Application.DTOs;
using TheProject.Domain.Entities;

namespace TheProject.Application.Features.Categories.Commands.AddCategory
{
    public class AddCategoryCommand : IRequest<Response<CategoryDto>>
    {
        public string Name { get; set; } = string.Empty;
    }
}