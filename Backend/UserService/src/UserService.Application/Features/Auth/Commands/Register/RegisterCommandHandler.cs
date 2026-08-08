using MediatR;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler(IUserRepository userRepository, IEmailService emailService, ITokenService tokenService)
        : IRequestHandler<RegisterCommand, Unit>
    {
        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new AlreadyExists($"User with email {request.Email} already exists");
            }

            var confirmationToken = tokenService.GenerateSecureToken();

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = UserRole.User,
                IsEmailConfirmed = false,
                IsActive = true,
                EmailConfirmationToken = confirmationToken,
                EmailConfirmationTokenExpiry = DateTime.UtcNow.AddDays(3)
            };

            await userRepository.CreateAsync(user);
            await userRepository.SaveAsync();

            await emailService.SendEmailConfirmation(user.Email, user.Name, confirmationToken);

            return Unit.Value;
        }
    }
}
