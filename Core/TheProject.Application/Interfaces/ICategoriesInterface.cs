using TheProject.Application.DTOs;
using TheProject.Domain.Entities;

namespace TheProject.Application.Interfaces
{
    public interface ICategoriesInterface
    {
        Task<Response<List<CategoryDto>>> GetAll();
        Task<Response<CategoryDto>> Add(string name);
    }
}