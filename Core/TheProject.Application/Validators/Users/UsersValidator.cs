
using FluentValidation;
using TheProject.Application.DTOs.UsersDTO;

namespace TheProject.Application.Validators.Users
{
    public class UsersValidator : AbstractValidator<UsersDTO>
    {
            public UsersValidator(bool isUpdate = false)
        {
        
            
            if (isUpdate)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Coloque um Id válido para o Usuário.");
            }

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome obrigatório.")
                .MaximumLength(100).WithMessage("Nome deve ter até 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email obrigatório.")
                .MaximumLength(100).WithMessage("Email deve ter até 100 caracteres.");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha obrigatória.")
                .MaximumLength(12).WithMessage("Senha deve ter até 12 caracteres");
        }
    }
}