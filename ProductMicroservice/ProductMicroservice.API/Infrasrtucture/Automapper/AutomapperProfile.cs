using AutoMapper;
using ProductMicroservice.BLL.Models.DTO;
using ProductMicroservice.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
