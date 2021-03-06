using MassTransit;
using Microservice.Core.Infrastructure.OperationResult;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using Microservice.Core.Messages.FileReport;
using ReportMicroservice.DAL.Repositories.Interfaces.SQLServer;
using ReportMicroservice.DAL.Repositories.SQLServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroservice.BLL.ResponseConsumers.FileReport
{
    public class FileReportDownloadResponseConsumer : IConsumer<FileReportDownloadRequest>
    {
        private readonly ISQLUnitOfWork _sqlUnitOfWork;

        public FileReportDownloadResponseConsumer(ISQLUnitOfWork sqlUnitOfWork)
        {
            _sqlUnitOfWork = sqlUnitOfWork;
        }

        public async Task Consume(ConsumeContext<FileReportDownloadRequest> context)
        {
            var fileReportSQLRepository = _sqlUnitOfWork.GetRepository<IFileReportSQLServerRepository>();
            var fileReportResult = await fileReportSQLRepository.GetAsync(item => item.Id.Equals(context.Message.FileId));

            var respond = new OperationResult<FileReportDownloadResponse>
            {
                Type = fileReportResult.Type,
                Errors = fileReportResult.Errors
            };

            if (fileReportResult.IsSuccess)
            {
                var fileReport = fileReportResult.Data.FirstOrDefault();
                if (fileReport != null)
                {
                    respond.Data = new FileReportDownloadResponse
                    {
                        GoogleId = fileReport.GoogleId,
                        Mime = fileReport.Mime,
                        Name = fileReport.Name
                    };
                }
            }

            await context.RespondAsync<OperationResult<FileReportDownloadResponse>>(respond);
        }
    }
}
