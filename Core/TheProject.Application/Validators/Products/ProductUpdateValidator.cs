using FluentValidation;
using TheProject.Application.DTOs;



  public class ProductUpdateValidator : AbstractValidator<ProductUptadeDTO>
{   
    public ProductUpdateValidator(bool isUpdate = false)
    {
        

         
        if (isUpdate)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Coloque um Id válido para o produto.");
        }

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Código da categoria inválido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter até 100 caracteres.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantidade em estoque inválida.");
    }
}

   
