using AutoMapper;
using ReportMicroservice.BLL.Models.DTO;
using ReportMicroservice.DAL.Models;
using ReportMicroservice.DAL.Models.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportMicroservice.API.Infrasrtucture.Automapper
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Report, ReportDTO>()
                .ReverseMap();
        }
    }
}
