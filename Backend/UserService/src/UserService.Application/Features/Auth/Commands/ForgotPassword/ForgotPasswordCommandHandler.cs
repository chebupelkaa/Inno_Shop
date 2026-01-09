using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService,
    ITokenService tokenService) : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(typeof(UserDTO));
            }

            if (user.PasswordResetTokenExpiry > DateTime.UtcNow)
            {
                var timeLeft = (user.PasswordResetTokenExpiry.Value - DateTime.UtcNow).TotalMinutes;
                throw new TokenAlreadyRequestedException(user.PasswordResetTokenExpiry.Value);
            }

            var resetToken = _tokenService.GenerateSecureToken();
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();

            await _emailService.SendPasswordRecovery(user.Email, resetToken);

            return Unit.Value;
        }

    }
}
