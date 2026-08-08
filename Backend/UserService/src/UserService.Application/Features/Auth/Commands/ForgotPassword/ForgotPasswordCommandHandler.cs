using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService, ITokenService tokenService)
        : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Unit.Value;
            }

            if (!string.IsNullOrEmpty(user.PasswordResetToken)
                && user.PasswordResetTokenExpiry.HasValue
                && user.PasswordResetTokenExpiry > DateTime.UtcNow)
            {
                await emailService.SendPasswordRecovery(user.Email, user.PasswordResetToken);
                return Unit.Value;
            }

            var resetToken = tokenService.GenerateSecureToken();
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            await userRepository.UpdateAsync(user);
            await userRepository.SaveAsync();

            await emailService.SendPasswordRecovery(user.Email, resetToken);

            return Unit.Value;
        }
    }
}
