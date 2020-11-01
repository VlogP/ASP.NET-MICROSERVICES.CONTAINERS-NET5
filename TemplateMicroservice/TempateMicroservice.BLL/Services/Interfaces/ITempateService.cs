using Microservice.Messages.Infrastructure.OperationResult;
using TempateMicroservice.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TempateMicroservice.BLL.Services.Interfaces
{
    public interface ITempateService
    {
        Task<OperationResult<List<TempateModel>>> GetAll();

        OperationResult<object> Add(TempateModel product);
    }
}
