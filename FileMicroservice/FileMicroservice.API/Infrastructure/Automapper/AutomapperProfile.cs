using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileMicroservice.BLL.Models.DTO;
using FileMicroservice.DAL.Models;
using FileMicroservice.DAL.Models.SQLServer;
using FileMicroservice.API.Models;
using FileMicroservice.BLL.Models.File;

namespace FileMicroservice.API.Infrastructure.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<FileModel, FileDTO>()
                .ReverseMap();

            CreateMap<FileUploadAPI, FileDTO>()
                .ReverseMap();

            CreateMap<FileUploadAPI, FilePost>()
                .ReverseMap();
        }
    }
}
