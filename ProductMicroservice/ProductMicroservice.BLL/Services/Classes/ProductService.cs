using ProductMicroservice.BLL.Services.Interfaces;
using MassTransit;
using Microservice.Core.Messages.Test;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.Core.Infrastructure.OperationResult;
using AutoMapper;
using ProductMicroservice.DAL.Repositories.SQLServer.Interfaces;
using ProductMicroservice.DAL.Models.SQLServer;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using Microservice.Core.Infrastructure.UnitofWork.Mongo;
using ProductMicroservice.DAL.Repositories.Mongo.Interfaces;
using ProductMicroservice.BLL.Models.DTO.Product;
using MongoDB.Bson;
using System.Linq;
using ProductMicroservice.BLL.Models.Product;
using ProductMicroservice.DAL.Repositories.ElasticSearch.Interfaces;

namespace ProductMicroservice.BLL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<TestMessageRequest> _client;
        private readonly ISQLUnitOfWork _sqlUnitOfWork;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly IProductElasticSearchRepository _productElasticSearchRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IPublishEndpoint publishEndpoint, 
            IRequestClient<TestMessageRequest> client, 
            ISQLUnitOfWork unitOfWork, IMapper mapper, 
            IMongoUnitOfWork mongoUnitOfWork,
            IProductElasticSearchRepository productElasticSearchRepository)
        {
            _publishEndpoint = publishEndpoint;
            _client = client;
            _sqlUnitOfWork = unitOfWork;
            _mapper = mapper;
            _mongoUnitOfWork = mongoUnitOfWork;
            _productElasticSearchRepository = productElasticSearchRepository;
        }

        public OperationResult<ProductPostDTO> Add(ProductPost newProduct)
        {
            var sqlServerProductRepository = _sqlUnitOfWork.GetRepository<IProductSQLServerRepository>();
            var sqlServerClientRepository = _sqlUnitOfWork.GetRepository<IClientSQLServerRepository>();
            var mongoProductRepository = _mongoUnitOfWork.GetRepository<IProductMongoRepository>();

            var clientResult = sqlServerClientRepository.Get(client => client.Id.Equals(newProduct.ClientId));
            var client = clientResult.Data.FirstOrDefault();
            var isClientExist = client != null;
            var result = new OperationResult<ProductPostDTO>();

            if (isClientExist)
            {
                newProduct.Tags?.Add(client.Name);
                var bsonId = ObjectId.GenerateNewId();
                var sqlServerProduct = _mapper.Map<Product>(newProduct);
                var mongoProduct = _mapper.Map<DAL.Models.Mongo.Product>(newProduct);
                var elasticProduct = _mapper.Map<DAL.Models.ElasticSearch.Product>(newProduct);

                mongoProduct.ClientName = client.Name;
                mongoProduct.ClientId = client.Id;
                mongoProduct.Id = bsonId;

                var mongoResult = mongoProductRepository.Add(mongoProduct);
                result.Errors = mongoResult.Errors;
                result.Type = mongoResult.Type;

                if (mongoResult.IsSuccess)
                {
                    sqlServerProduct.Id = Guid.NewGuid();
                    sqlServerProduct.BsonId = bsonId.ToString();

                    var sqlServerResult = sqlServerProductRepository.Add(sqlServerProduct);
                    result.Errors = sqlServerResult.Errors;
                    result.Type = sqlServerResult.Type;

                    if (sqlServerResult.IsSuccess)
                    {
                        elasticProduct.Id = bsonId.ToString();

                        var elasticResult = _productElasticSearchRepository.Add(elasticProduct);
                        result.Errors = elasticResult.Errors;
                        result.Type = elasticResult.Type;

                        if (elasticResult.IsSuccess)
                        {
                            result.Type = ResultType.Success;
                            result.Data = new ProductPostDTO
                            {
                                Id = bsonId.ToString(),
                                Name = mongoProduct.Name
                            };
                            _sqlUnitOfWork.Save();
                        }
                    }
                }             
            }
            else
            {
                result.Type = ResultType.BadRequest;
                result.Errors.Add("Not valid client id");
            }

            return result;
        }

        public async Task<OperationResult<List<ProductGetDTO>>> GetAll()
        {
            var mongoProductRepository = _mongoUnitOfWork.GetRepository<IProductMongoRepository>();

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

            var productsResult = mongoProductRepository.Get(_ => true);
            var resultData = new OperationResult<List<ProductGetDTO>>
            {
                Type = productsResult.Type,
                Errors = productsResult.Errors,
                Data = _mapper.Map<List<ProductGetDTO>>(productsResult.Data)
            };

            return resultData;
        }
    }
}
