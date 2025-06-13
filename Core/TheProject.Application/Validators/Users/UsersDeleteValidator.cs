
using FluentValidation;
using TheProject.Application.DTOs.UsersDTO;

namespace TheProject.Application.Validators.Users
{
    public class UsersDeleteValidator : AbstractValidator<UserDeleteDTO>
    {
            public UsersDeleteValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Código do usuário inválido");
        }
    }
    
}