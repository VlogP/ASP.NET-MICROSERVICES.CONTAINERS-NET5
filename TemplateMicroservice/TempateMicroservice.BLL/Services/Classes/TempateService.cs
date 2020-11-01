using TempateMicroservice.BLL.Services.Interfaces;
using MassTransit;
using MassTransit.Audit;
using Microservice.Messages.Messages.Test;
using TempateMicroservice.DAL.Models;
using TempateMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using Microservice.Messages.Infrastructure.OperationResult;
using TempateMicroservice.DAL.Infrastructure.UnitofWork;
using TempateMicroservice.DAL.Repositories.Classes;

namespace TempateMicroservice.BLL.Services.Classes
{
    public class TempateService : ITempateService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<TestMessageRequest> _client;
        private readonly IUnitOfWork _unitOfWork;

        public TempateService(IPublishEndpoint publishEndpoint, IRequestClient<TestMessageRequest> client, IUnitOfWork unitOfWork)
        {
            _publishEndpoint = publishEndpoint;
            _client = client;
            _unitOfWork = unitOfWork;
        }

        public OperationResult<object> Add(TempateModel product)
        {
            var productRepository = _unitOfWork.GetRepository<TempateRepository>();

            var dataResult = productRepository.Add(product);
            _unitOfWork.Save();

            var result = new OperationResult<object>()
            {
                Type = dataResult.Type,
                Errors = dataResult.Errors
            };

            return result;
        }

        async public Task<OperationResult<List<TempateModel>>> GetAll()
        {
            var productRepository = _unitOfWork.GetRepository<TempateRepository>();
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
