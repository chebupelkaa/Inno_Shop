using FluentValidation.TestHelper;
using UserService.Application.Features.Auth.Commands.Login;
using UserService.Application.Features.Auth.Commands.Register;

namespace UserService.UnitTests.Validators
{
    public class AuthValidatorsTests
    {
        [Fact]
        public void Register_invalid_email_and_weak_password_should_fail()
        {
            var validator = new RegisterCommandValidator();
            var result = validator.TestValidate(new RegisterCommand
            {
                Name = "Test User",
                Email = "not-an-email",
                Password = "weak"
            });

            result.ShouldHaveValidationErrorFor(x => x.Email);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Register_valid_command_should_pass()
        {
            var validator = new RegisterCommandValidator();
            var result = validator.TestValidate(new RegisterCommand
            {
                Name = "Test User",
                Email = "user@example.com",
                Password = "Password1!"
            });

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Login_empty_fields_should_fail()
        {
            var validator = new LoginCommandValidator();
            var result = validator.TestValidate(new LoginCommand
            {
                Email = "",
                Password = ""
            });

            result.ShouldHaveValidationErrorFor(x => x.Email);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}
