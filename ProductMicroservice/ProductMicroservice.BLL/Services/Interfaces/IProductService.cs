using Microservice.Messages.Infrastructure.OperationResult;
using ProductMicroservice.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMicroservice.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<OperationResult<List<Product>>> GetAll();

        OperationResult<object> Add(Product product);
    }
}
