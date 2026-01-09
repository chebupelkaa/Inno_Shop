using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        public async Task<Unit> Handle(ResetPasswordCommand request,CancellationToken cancellationToken)
        {
            if (request.NewPassword==request.ConfirmPassword)
            {
                throw new PasswordValidationException();
            }

            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new NotFoundException(typeof(UserDTO));
            }

            if (string.IsNullOrEmpty(user.PasswordResetToken) || user.PasswordResetToken != request.Token)
            {
                throw new BaseException("Invalid or expired reset token");
            }

            if (user.PasswordResetTokenExpiry == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                throw new BaseException("Invalid or expired reset token");
            }

            if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
            {
                throw new BaseException("New password cannot be the same as the old password");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();

            return Unit.Value;

        }
    }
}
