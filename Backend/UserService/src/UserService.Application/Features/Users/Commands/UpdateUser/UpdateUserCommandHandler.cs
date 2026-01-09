using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<UpdateUserCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var checkUser = await _userRepository.GetByIdAsync(request.newUser.Id);

            if (checkUser == null)
                throw new NotFoundException(typeof(UserDTO), request.newUser.Id);

            var user =_mapper.Map<User>(request.newUser);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();

            return _mapper.Map<UserDTO>(user);
        }
    }
}
