using ProductMicroservice.BLL.Services.Interfaces;
using MassTransit;
using Microservice.Messages.Messages.Test;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.Messages.Infrastructure.OperationResult;
using AutoMapper;
using ProductMicroservice.BLL.Models.DTO;
using ProductMicroservice.DAL.Repositories.SQLServer.Interfaces;
using ProductMicroservice.DAL.Models.SQLServer;
using Microservice.Messages.Infrastructure.UnitofWork.SQL;
using Microservice.Messages.Infrastructure.UnitofWork.Mongo;
using ProductMicroservice.DAL.Repositories.Mongo.Interfaces;

namespace ProductMicroservice.BLL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<TestMessageRequest> _client;
        private readonly ISQLUnitOfWork _sqlUnitOfWork;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IPublishEndpoint publishEndpoint, IRequestClient<TestMessageRequest> client, ISQLUnitOfWork unitOfWork, IMapper mapper, IMongoUnitOfWork mongoUnitOfWork)
        {
            _publishEndpoint = publishEndpoint;
            _client = client;
            _sqlUnitOfWork = unitOfWork;
            _mapper = mapper;
            _mongoUnitOfWork = mongoUnitOfWork;
        }

        public OperationResult<ProductDTO> Add(ProductDTO newProduct)
        {
            newProduct.Id = Guid.NewGuid();
            var sqlProductRepository = _sqlUnitOfWork.GetRepository<IProductSQLServerRepository>();
            var mongoProductRepository = _mongoUnitOfWork.GetRepository<IProductMongoRepository>();
            var testMongo = mongoProductRepository.Get(item => true);

            var product = _mapper.Map<Product>(newProduct);

            var dataResult = sqlProductRepository.Add(product);
            _sqlUnitOfWork.Save();

            var result = new OperationResult<ProductDTO>()
            {
                Type = dataResult.Type,
                Errors = dataResult.Errors,
                Data = _mapper.Map<ProductDTO>(dataResult.Data)
            };

            return result;
        }

        async public Task<OperationResult<List<ProductDTO>>> GetAll()
        {
            var productRepository = _sqlUnitOfWork.GetRepository<IProductSQLServerRepository>();
            List<TestMessageResponse> list = new List<TestMessageResponse>();

            for (var index = 0; index < 1; index++)
            {
                var response = await _client.GetResponse<OperationResult<TestMessageResponse>>(new TestMessageRequest { Text = index.ToString() });
                list.Add(response.Message.Data);
            }
     
            Parallel.For(1, 1000, index =>
            {
                _publishEndpoint.Publish(new TestMessagePublish
                {
                    Id = Guid.NewGuid(),
                    Text = "TestText",
                    Value = index
                });
            });

            var productsResult = productRepository.Get();
            var resultData = new OperationResult<List<ProductDTO>>
            {
                Type = productsResult.Type,
                Errors = productsResult.Errors,
                Data = _mapper.Map<List<ProductDTO>>(productsResult.Data)
            };

            return resultData;
        }
    }
}
