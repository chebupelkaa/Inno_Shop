using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.SendEmailConfirmation
{
    public class SendEmailConfirmationCommandHandler(IUserRepository userRepository, IEmailService emailService, ITokenService tokenService)
        : IRequestHandler<SendEmailConfirmationCommand, Unit>
    {
        public async Task<Unit> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException(typeof(UserDTO));
            }

            if (user.IsEmailConfirmed)
            {
                throw new BaseException("Email is already confirmed");
            }

            if (!string.IsNullOrEmpty(user.EmailConfirmationToken)
                && user.EmailConfirmationTokenExpiry.HasValue
                && user.EmailConfirmationTokenExpiry > DateTime.UtcNow)
            {
                await emailService.SendEmailConfirmation(user.Email, user.Name, user.EmailConfirmationToken);
                return Unit.Value;
            }

            var confirmationToken = tokenService.GenerateSecureToken();
            user.EmailConfirmationToken = confirmationToken;
            user.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddDays(3);

            await userRepository.UpdateAsync(user);
            await userRepository.SaveAsync();

            await emailService.SendEmailConfirmation(user.Email, user.Name, confirmationToken);

            return Unit.Value;
        }
    }
}
