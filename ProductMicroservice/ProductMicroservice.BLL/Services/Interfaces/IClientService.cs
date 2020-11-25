using Microservice.Core.Infrastructure.OperationResult;
using ProductMicroservice.BLL.Models.Client;
using ProductMicroservice.BLL.Models.DTO.Client;

namespace ProductMicroservice.BLL.Services.Interfaces
{
    public interface IClientService
    {
        public OperationResult<ClientPostDTO> Add(ClientPost product);
    }
}
