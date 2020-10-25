using ProductMicroservice.BLL.Services.Interfaces;
using MassTransit;
using MassTransit.Audit;
using Microservice.Messages.Messages.Test;
using ProductMicroservice.DAL.Models;
using ProductMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using Microservice.Messages.Infrastructure.OperationResult;

namespace ProductMicroservice.BLL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<TestMessageRequest> _client;

        public ProductService(IProductRepository productRepository, IPublishEndpoint publishEndpoint, IRequestClient<TestMessageRequest> client)
        {
            _productRepository = productRepository;
            _publishEndpoint = publishEndpoint;
            _client = client;

        }

        public OperationResult<object> Add(Product product)
        {
            var dataResult = _productRepository.Add(product);

            var result = new OperationResult<object>()
            {
                Type = ResultType.Success
            };

            return result;
        }

        async public Task<OperationResult<List<Product>>> GetAll()
        {
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

            return _productRepository.Get();
        }
    }
}
