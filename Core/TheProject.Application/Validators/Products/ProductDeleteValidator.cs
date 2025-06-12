using FluentValidation;
using TheProject.Application.DTOs;

namespace TheProject.Application.Validators.Products
{
    public class ProductDeleteValidator : AbstractValidator<ProductDeleteDTO>
    {
         public ProductDeleteValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Código do produto não informado ou inválido");
        }
    }
}