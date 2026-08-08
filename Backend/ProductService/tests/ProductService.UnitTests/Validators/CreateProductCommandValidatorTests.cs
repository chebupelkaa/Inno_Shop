using FluentValidation.TestHelper;
using ProductService.Application.Features.Products.Commands.CreateProduct;

namespace ProductService.UnitTests.Validators
{
    public class CreateProductCommandValidatorTests
    {
        [Fact]
        public void Invalid_price_and_empty_name_should_fail()
        {
            var validator = new CreateProductCommandValidator();
            var result = validator.TestValidate(new CreateProductCommand
            {
                Name = "",
                Description = "desc",
                Price = 0
            });

            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Valid_command_should_pass()
        {
            var validator = new CreateProductCommandValidator();
            var result = validator.TestValidate(new CreateProductCommand
            {
                Name = "Phone",
                Description = "Nice phone",
                Price = 10.5m
            });

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
