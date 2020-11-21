using AutoMapper;
using ProductMicroservice.BLL.Models.DTO;
using ProductMicroservice.DAL.Models.SQLServer;

namespace ProductMicroservice.API.Infrasrtucture.Automapper
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ReverseMap();
        }
    }
}
