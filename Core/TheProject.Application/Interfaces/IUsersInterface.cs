using TheProject.Application.DTOs.UsersDTO;
using TheProject.Application.DTOs.UsersUpdateDTO;
using TheProject.Domain.Entities;

namespace TheProject.Application.Interfaces
{
    public interface IUsersInterface
    {
        Task<Response<List<UsersDTO>>> GetAll();
        Task<Response<UsersDTO>> Add(UsersDTO dto);

        Task<Response<Users>> Update(UsersUpdateDTO dto);

        Task<Response<bool>> Delete(UserDeleteDTO dto);

        Task<Users?> GetUserByEmailAndPasswordAsync(string email, string password);
    }
}