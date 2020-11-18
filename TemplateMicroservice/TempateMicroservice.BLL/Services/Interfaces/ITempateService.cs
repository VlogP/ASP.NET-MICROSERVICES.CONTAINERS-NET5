using Microservice.Messages.Infrastructure.OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;
using TempateMicroservice.DAL.Models.SQLServer;

namespace TempateMicroservice.BLL.Services.Interfaces
{
    public interface ITempateService
    {
        Task<OperationResult<List<TemplateModel>>> GetAll();

        OperationResult<object> Add(TemplateModel product);
    }
}
