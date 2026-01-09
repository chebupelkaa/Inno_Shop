using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler(IUserRepository userRepository, IAuthenticationService authService)
        : IRequestHandler<RegisterCommand, TokenResponseDTO>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAuthenticationService _authService = authService;
        public async Task<TokenResponseDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new AlreadyExists($"User with email {request.Email} already exists");
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = UserRole.User,
                IsEmailConfirmed = false,
                IsActive = true
            };

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();

            return await _authService.GenerateAuthenticationAsync(user);
        }
    }
}
