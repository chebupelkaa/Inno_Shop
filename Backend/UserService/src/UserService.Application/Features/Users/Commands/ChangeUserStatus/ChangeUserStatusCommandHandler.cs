using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Users.Commands.ChangeUserStatus
{
    internal class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserStatusCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
                throw new NotFoundException(typeof(UserDTO), request.UserId);

            user.IsActive = request.IsActive;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();

            return true;
        }
    }
}
