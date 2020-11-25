using AutoMapper;
using Microservice.Core.Infrastructure.OperationResult;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using ProductMicroservice.BLL.Models.Client;
using ProductMicroservice.BLL.Models.DTO.Client;
using ProductMicroservice.BLL.Services.Interfaces;
using ProductMicroservice.DAL.Models.SQLServer;
using ProductMicroservice.DAL.Repositories.SQLServer.Interfaces;
using System;

namespace ProductMicroservice.BLL.Services.Classes
{
    public class ClientService : IClientService
    {
        private readonly ISQLUnitOfWork _sqlUnitOfWork;
        private readonly IMapper _mapper;

        public ClientService(ISQLUnitOfWork sqlUnitOfWork, IMapper mapper)
        {
            _sqlUnitOfWork = sqlUnitOfWork;
            _mapper = mapper;
        }

        public OperationResult<ClientPostDTO> Add(ClientPost newClient)
        {
            var sqlServerClientRepository = _sqlUnitOfWork.GetRepository<IClientSQLServerRepository>();
            var client = _mapper.Map<Client>(newClient);
            client.Id = Guid.NewGuid();

            var dataResult = sqlServerClientRepository.Add(client);
            _sqlUnitOfWork.Save();

            var result = new OperationResult<ClientPostDTO>
            {
                Type = dataResult.Type,
                Errors = dataResult.Errors,
                Data = _mapper.Map<ClientPostDTO>(dataResult.Data)
            };

            return result;
        }
    }
}
