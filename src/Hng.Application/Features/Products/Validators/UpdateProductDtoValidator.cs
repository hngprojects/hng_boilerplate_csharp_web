using FluentValidation;
using Hng.Application.Features.Products.Dtos;

namespace Hng.Application.Features.Products.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("product_name is required");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("price must be a positive number");
        }
    }
}
