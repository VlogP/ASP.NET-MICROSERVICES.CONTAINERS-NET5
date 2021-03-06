using Microservice.Core.Infrastructure.OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileMicroservice.DAL.Models.SQLServer;
using FileMicroservice.BLL.Models.File;
using FileMicroservice.BLL.Models.DTO;

namespace FileMicroservice.BLL.Services.Interfaces
{
    public interface IFileService
    {
        Task<OperationResult<FileDTO>> Upload(FilePost file);

        Task<OperationResult<FileDTO>> Download(FileGet file);
    }
}
