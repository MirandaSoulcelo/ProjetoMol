
namespace TheProject.Application.DTOs.UsersUpdateDTO
{
    public class UsersUpdateDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true; 

    }
}