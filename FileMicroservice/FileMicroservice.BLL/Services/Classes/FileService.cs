using FileMicroservice.BLL.Services.Interfaces;
using MassTransit;
using Microservice.Core.Messages.Test;
using System.Threading.Tasks;
using Microservice.Core.Infrastructure.OperationResult;
using FileMicroservice.DAL.Models.SQLServer;
using FileMicroservice.DAL.Repositories.SQLServer.Interfaces;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using FileMicroservice.BLL.Models.DTO;
using Microsoft.Extensions.Configuration;
using FileMicroservice.BLL.Models.File;
using System.Linq;
using FileMicroservice.BLL.Infrastructure.GoogleHelper;
using Microservice.Core.Messages.FileReport;
using System.IO;

namespace FileMicroservice.BLL.Services.Classes
{
    public class FileService : IFileService
    {
        private readonly IRequestClient<FileReportUploadRequest> _clientUpload;
        private readonly IRequestClient<FileReportDownloadRequest> _clientDownload;
        private readonly IConfiguration _configuration;

        public FileService(
            IRequestClient<FileReportUploadRequest> clientUpload,
            IRequestClient<FileReportDownloadRequest> clientDownload,
            IConfiguration configuration)
        {
            _clientUpload = clientUpload;
            _clientDownload = clientDownload;
            _configuration = configuration;
        }

        public async Task<OperationResult<FileDTO>> Upload(FilePost file)
        {
            var googleService = await GoogleHelper.AuthGoogle(_configuration);

            using (var stream = file.File.OpenReadStream())
            {
                var uploadResponse = await GoogleHelper.Upload(_configuration, googleService, stream, file.File);

                var result = new OperationResult<FileDTO>
                {
                    Type = uploadResponse.Type,
                    Errors = uploadResponse.Errors
                };

                if (uploadResponse.IsSuccess)
                {
                    var reportResponse = await _clientUpload.GetResponse<OperationResult<FileReportUploadResponse>>(new FileReportUploadRequest
                    {
                        Description = file.Description,
                        Name = file.File.FileName,
                        GoogleId = uploadResponse.Data.Id,
                        Mime = file.File.ContentType,
                        ReportId = file.ReportId
                    });

                    result.Type = reportResponse.Message.Type;
                    result.Errors.AddRange(reportResponse.Message.Errors);

                    if (reportResponse.Message.IsSuccess)
                    {
                        result.Data = new FileDTO
                        {
                            FileId = reportResponse.Message.Data.FileId
                        };
                    }
                }

                return result;
            }
        }

        public async Task<OperationResult<FileDTO>> Download(FileGet file)
        {
            var result = new OperationResult<FileDTO>();
            using (var googleService = await GoogleHelper.AuthGoogle(_configuration))
            {
                var reportResponse = await _clientDownload.GetResponse<OperationResult<FileReportDownloadResponse>>(new FileReportDownloadRequest
                {
                    FileId = file.Id
                });

                result.Type = reportResponse.Message.Type;
                result.Errors = reportResponse.Message.Errors;

                var googleId = reportResponse.Message.Data.GoogleId;
                var getRequest = googleService.Files.Get(googleId);

                if (reportResponse.Message.IsSuccess)
                {
                    var filestream = new MemoryStream();
                    await getRequest.DownloadAsync(filestream);
                    filestream.Seek(0, SeekOrigin.Begin);

                    result.Data = new FileDTO
                    {
                        FileStream = filestream,
                        Mime = reportResponse.Message.Data.Mime,
                        Name = reportResponse.Message.Data.Name
                    };
                }
            }

            return result;
        }
    }
}
