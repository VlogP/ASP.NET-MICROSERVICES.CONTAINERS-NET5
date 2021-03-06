using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Microservice.Core.Constants.ConfigurationVariables;
using Microservice.Core.Infrastructure.OperationResult;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileMicroservice.BLL.Infrastructure.GoogleHelper
{
    class GoogleHelper
    {
        public static async Task<OperationResult<GoogleUploadResponse>> Upload(IConfiguration configuration, DriveService googleService, Stream fileStream, IFormFile file)
        {
            var parentFolderId = configuration[MicroserviceConfigurationVariables.Google.PARRENT_FOLDER_ID];
            var genIdRequest = googleService.Files.GenerateIds();
            genIdRequest.Count = 1;

            var id = (await genIdRequest.ExecuteAsync()).Ids.First();
            var googleFile = new Google.Apis.Drive.v3.Data.File
            {
                Name = file.FileName,
                Id = id,
                Parents = new List<string> { parentFolderId }
            };

            var uploadRequest = googleService.Files.Create(googleFile, fileStream, file.ContentType);
            var uploadResponse = await uploadRequest.UploadAsync();
            var result = new OperationResult<GoogleUploadResponse>
            {
                Data = new GoogleUploadResponse 
                {
                    Id = id
                }
            };

            if(uploadResponse.Status != UploadStatus.Completed)
            {
                result.Errors.Add(uploadResponse.Exception.Message);
                result.Type = ResultType.Invalid;
            }
            else
            {
                result.Type = ResultType.Success;
            }

            return result;
        }

        public static async Task<DriveService> AuthGoogle(IConfiguration configuration)
        {
            var Scopes = new List<string> { DriveService.Scope.Drive };

            var googleCredential = await GoogleCredential.FromFileAsync(
                configuration[MicroserviceConfigurationVariables.Google.CREDENTIALS],
                CancellationToken.None);
            
            googleCredential = googleCredential.CreateScoped(Scopes);
            googleCredential = googleCredential.CreateWithUser(configuration[MicroserviceConfigurationVariables.Google.USERNAME]);

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = configuration[MicroserviceConfigurationVariables.Google.APPLICATION]               
            });

            return service;
        }
    }
}
