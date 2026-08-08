using Moq;
using UserService.Application.Exceptions;
using UserService.Application.Features.Auth.Commands.ConfirmEmail;
using UserService.Application.Features.Auth.Commands.ForgotPassword;
using UserService.Application.Features.Auth.Commands.ResetPassword;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.UnitTests.Handlers
{
    public class AuthHandlersTests
    {
        [Fact]
        public async Task ConfirmEmail_valid_token_should_confirm_user()
        {
            var user = new User
            {
                Email = "user@example.com",
                IsEmailConfirmed = false,
                EmailConfirmationToken = "token-1",
                EmailConfirmationTokenExpiry = DateTime.UtcNow.AddDays(1)
            };

            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            var handler = new ConfirmEmailCommandHandler(repository.Object);
            await handler.Handle(new ConfirmEmailCommand
            {
                Email = user.Email,
                Token = "token-1"
            }, CancellationToken.None);

            Assert.True(user.IsEmailConfirmed);
            Assert.Null(user.EmailConfirmationToken);
            repository.Verify(r => r.UpdateAsync(user), Times.Once);
            repository.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task ForgotPassword_unknown_email_should_return_without_error()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            var emailService = new Mock<IEmailService>();
            var tokenService = new Mock<ITokenService>();

            var handler = new ForgotPasswordCommandHandler(repository.Object, emailService.Object, tokenService.Object);
            await handler.Handle(new ForgotPasswordCommand { Email = "missing@example.com" }, CancellationToken.None);

            emailService.Verify(e => e.SendPasswordRecovery(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ResetPassword_mismatched_passwords_should_throw()
        {
            var handler = new ResetPasswordCommandHandler(Mock.Of<IUserRepository>());

            await Assert.ThrowsAsync<PasswordValidationException>(() => handler.Handle(new ResetPasswordCommand
            {
                Email = "user@example.com",
                Token = "token",
                NewPassword = "Password1!",
                ConfirmPassword = "Password2!"
            }, CancellationToken.None));
        }
    }
}
