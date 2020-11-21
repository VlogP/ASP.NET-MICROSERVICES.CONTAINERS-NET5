using TempateMicroservice.BLL.Services.Interfaces;
using MassTransit;
using MassTransit.Audit;
using Microservice.Core.Messages.Test;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using Microservice.Core.Infrastructure.OperationResult;
using Microservice.Core.Infrastructure.UnitofWork;
using TempateMicroservice.DAL.Models.SQLServer;
using TempateMicroservice.DAL.Repositories.SQLServer.Interfaces;
using Microservice.Core.Infrastructure.UnitofWork.SQL;

namespace TempateMicroservice.BLL.Services.Classes
{
    public class TempateService : ITempateService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<TestMessageRequest> _client;
        private readonly ISQLUnitOfWork _sqlUnitOfWork;

        public TempateService(IPublishEndpoint publishEndpoint, IRequestClient<TestMessageRequest> client, ISQLUnitOfWork sqlUnitOfWork)
        {
            _publishEndpoint = publishEndpoint;
            _client = client;
            _sqlUnitOfWork = sqlUnitOfWork;
        }

        public OperationResult<object> Add(TemplateModel product)
        {
            var productRepository = _sqlUnitOfWork.GetRepository<ITempateSQLServerRepository>();

            var dataResult = productRepository.Add(product);
            _sqlUnitOfWork.Save();

            var result = new OperationResult<object>()
            {
                Type = dataResult.Type,
                Errors = dataResult.Errors
            };

            return result;
        }

        async public Task<OperationResult<List<TemplateModel>>> GetAll()
        {
            var productRepository = _sqlUnitOfWork.GetRepository<ITempateSQLServerRepository>();
            List<TestMessageResponse> list = new List<TestMessageResponse>();

            for(var index = 0; index <= 10; index++)
            {
                await _publishEndpoint.Publish(new TestMessagePublish
                { 
                    Id = Guid.NewGuid(),
                    Text = "TestText",
                    Value = 5                             
                });
            }

            
            for (var index = 0; index <= 10; index++)
            {
                var result = await _client.GetResponse<OperationResult<TestMessageResponse>>(new TestMessageRequest { Text = index.ToString() });

                list.Add(result.Message.Data);
            }

            return productRepository.Get();
        }
    }
}
