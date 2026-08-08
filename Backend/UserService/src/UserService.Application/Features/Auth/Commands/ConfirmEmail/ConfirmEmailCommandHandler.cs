using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler(IUserRepository userRepository)
        : IRequestHandler<ConfirmEmailCommand, Unit>
    {
        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
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

            if (string.IsNullOrEmpty(user.EmailConfirmationToken) || user.EmailConfirmationToken != request.Token)
            {
                throw new BaseException("Invalid or expired confirmation token");
            }

            if (user.EmailConfirmationTokenExpiry == null || user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
            {
                throw new BaseException("Invalid or expired confirmation token");
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpiry = null;

            await userRepository.UpdateAsync(user);
            await userRepository.SaveAsync();

            return Unit.Value;
        }
    }
}
