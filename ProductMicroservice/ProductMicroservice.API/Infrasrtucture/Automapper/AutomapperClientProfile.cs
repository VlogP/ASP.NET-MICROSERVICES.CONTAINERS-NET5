using AutoMapper;
using ProductMicroservice.API.Models.Client;
using ProductMicroservice.BLL.Models.Client;
using ProductMicroservice.BLL.Models.DTO.Client;
using ProductMicroservice.DAL.Models.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.API.Infrasrtucture.Automapper
{
    public class AutomapperClientProfile : Profile
    {
        public AutomapperClientProfile()
        {
            CreateMap<Client, ClientPostDTO>()
                .ReverseMap();

            CreateMap<Client, ClientPost>()
                .ReverseMap();

            CreateMap<ClientPost, ClientPostAPI>()
                .ReverseMap();
        }
    }
}
