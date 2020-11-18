using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TempateMicroservice.BLL.Models.DTO;
using TempateMicroservice.DAL.Models;
using TempateMicroservice.DAL.Models.SQLServer;

namespace TempateMicroservice.API.Infrastructure.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<TemplateModel, TemplateDTO>()
                .ReverseMap();
        }
    }
}
