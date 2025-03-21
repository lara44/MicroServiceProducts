
using FluentValidation;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required..")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                    .WithMessage("Price must be greater than zero..")
                .Must(price => HasExactlyTwoDecimals(price))
                    .WithMessage("Price must have exactly 2 decimal places.")
                .PrecisionScale(18, 2, true)
                    .WithMessage("Price must not have more than 2 decimal places and up to 18 digits in total..");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative..");
        }

        private bool HasExactlyTwoDecimals(decimal price)
        {
            // Convierte a string y cuenta los dígitos después del punto decimal
            var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(price)[3])[2];
            return decimalPlaces == 2;
        }
    }
}