using MassTransit;
using Microservice.Core.Infrastructure.OperationResult;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using Microservice.Core.Messages.FileReport;
using ReportMicroservice.DAL.Models.SQLServer;
using ReportMicroservice.DAL.Repositories.Interfaces.SQLServer;
using ReportMicroservice.DAL.Repositories.SQLServer.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ReportMicroservice.BLL.ResponseConsumers.FileReport
{
    public class FileReportUploadResponseConsumer: IConsumer<FileReportUploadRequest>
    {
        private readonly ISQLUnitOfWork _sqlUnitOfWork;

        public FileReportUploadResponseConsumer(ISQLUnitOfWork sqlUnitOfWork)
        {
            _sqlUnitOfWork = sqlUnitOfWork;
        }

        public async Task Consume(ConsumeContext<FileReportUploadRequest> context)
        {
            var fileReportSQLRepository = _sqlUnitOfWork.GetRepository<IFileReportSQLServerRepository>();
            var reportSQLRepository = _sqlUnitOfWork.GetRepository<IReportSQLServerRepository>();

            var reportResult = await reportSQLRepository.GetAsync(item => item.Id.Equals(context.Message.ReportId));

            var reportExist = reportResult.Data.FirstOrDefault() != null;

            if (reportExist)
            {
                var fileReportResult = await fileReportSQLRepository.AddAsync(new DAL.Models.SQLServer.FileReport
                {
                    Description = context.Message.Description,
                    GoogleId = context.Message.GoogleId,
                    Mime = context.Message.Mime,
                    ReportId = context.Message.ReportId,
                    Name = context.Message.Name
                });
               
                var respond = new OperationResult<FileReportUploadResponse>
                {
                    Type = fileReportResult.Type,
                    Errors = fileReportResult.Errors,
                };

                if (fileReportResult.IsSuccess)
                {
                    await _sqlUnitOfWork.SaveAsync();
                    respond.Data = new FileReportUploadResponse
                    {
                        FileId = fileReportResult.Data.Id
                    };
                }

                await context.RespondAsync<OperationResult<FileReportUploadResponse>>(respond);
            }
        }
    }
}
