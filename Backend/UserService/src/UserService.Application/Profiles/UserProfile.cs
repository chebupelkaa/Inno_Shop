
using AutoMapper;
using UserService.Application.DTOs;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role)));
        }

    }
}
