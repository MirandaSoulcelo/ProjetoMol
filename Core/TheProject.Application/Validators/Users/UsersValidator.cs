
using FluentValidation;
using TheProject.Application.DTOs.UsersDTO;

namespace TheProject.Application.Validators.Users
{
    public class UsersValidator : AbstractValidator<UsersDTO>
    {
            public UsersValidator()
        {
        
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome obrigatório.")
                .MaximumLength(100).WithMessage("Nome deve ter até 100 caracteres.");

             RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Formato de email inválido.")
                .NotEmpty().WithMessage("Email obrigatório.")
                .MaximumLength(100).WithMessage("Email deve ter até 100 caracteres.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")
                .WithMessage("O email deve conter um domínio válido (ex: exemplo@dominio.com).");
                

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha obrigatória.")
                .MaximumLength(12).WithMessage("Senha deve ter até 12 caracteres");
        }
    }
}