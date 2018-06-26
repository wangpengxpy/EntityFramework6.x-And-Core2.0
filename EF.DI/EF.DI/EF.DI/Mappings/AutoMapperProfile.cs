using AutoMapper;
using EF.Core.Data;
using EF.DI.Models;

namespace EFDI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(d => d.FirstName, m => m.MapFrom(f => f.UserProfile.FirstName))
                .ForMember(d => d.LastName, m => m.MapFrom(f => f.UserProfile.LastName))
                .ForMember(d => d.Address, m => m.MapFrom(f => f.UserProfile.Address));

            CreateMap<UserDTO, User>()
                .ForMember(d => d.UserProfile, m => m.MapFrom(f => f));

            CreateMap<UserDTO, UserProfile>()
                .ForMember(d => d.FirstName, m => m.MapFrom(f => f.FirstName))
                .ForMember(d => d.LastName, m => m.MapFrom(f => f.LastName))
                .ForMember(d => d.Address, m => m.MapFrom(f => f.Address));
        }
    }
}