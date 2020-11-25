using Microservice.Core.Infrastructure.OperationResult;
using ProductMicroservice.BLL.Models.DTO.Product;
using ProductMicroservice.BLL.Models.Product;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMicroservice.BLL.Services.Interfaces
{
    public interface IProductService
    {
        public Task<OperationResult<List<ProductGetDTO>>> GetAll();

        public OperationResult<ProductPostDTO> Add(ProductPost product);
    }
}
