using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.SendEmailConfirmation
{
    public class SendEmailConfirmationCommandHandler : IRequestHandler<SendEmailConfirmationCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public SendEmailConfirmationCommandHandler(
            IUserRepository userRepository,
            IEmailService emailService,
            ITokenService tokenService,
            ILogger<SendEmailConfirmationCommandHandler> logger)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<Unit> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException(typeof(UserDTO),user);
            }

            if (user.IsEmailConfirmed)
            {
                throw new BaseException("Email is already confirmed");
            }

            if (user.EmailConfirmationTokenExpiry.HasValue && user.EmailConfirmationTokenExpiry > DateTime.UtcNow)
            {
                var minutesLeft = (int)(user.EmailConfirmationTokenExpiry.Value - DateTime.UtcNow).TotalMinutes;
                throw new TokenAlreadyRequestedException(user.EmailConfirmationTokenExpiry.Value);
            }

            var confirmationToken = _tokenService.GenerateSecureToken();
            user.EmailConfirmationToken = confirmationToken;
            user.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddDays(3); 

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();

            // Отправляем email
            await _emailService.SendEmailConfirmationAsync(user.Email, user.Name, confirmationToken);

            return Unit.Value;
        }
    }
}
