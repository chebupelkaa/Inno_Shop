using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.id);

            if (user == null)
            {
                throw new NotFoundException(typeof(UserDTO), request.id);
            }

            return _mapper.Map<UserDTO>(user);
        }
    }
}
