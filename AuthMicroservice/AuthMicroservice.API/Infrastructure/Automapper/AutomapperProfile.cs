using AuthMicroservice.BLL.Models.DTO.User;
using AuthMicroservice.BLL.Models.User;
using AuthMicroservice.DAL.Models;
using AuthMicroservice.DAL.Models.SQLServer;
using AutoMapper;

namespace AuthMicroservice.API.Infrasrtucture.Automapper
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Role, source => source.MapFrom(source => source.Role.Name))
                .ReverseMap();

            CreateMap<User, UserRegister>()
                .ReverseMap();
        }
    }
}
