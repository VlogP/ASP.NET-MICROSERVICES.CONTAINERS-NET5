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
using ProductMicroservice.DAL.Repositories.Classes;
using Microservice.Messages.Infrastructure.UnitofWork;
using AutoMapper;
using ProductMicroservice.BLL.Models.DTO;

namespace ProductMicroservice.BLL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<TestMessageRequest> _client;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IPublishEndpoint publishEndpoint, IRequestClient<TestMessageRequest> client, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _publishEndpoint = publishEndpoint;
            _client = client;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public OperationResult<ProductDTO> Add(ProductDTO newProduct)
        {
            newProduct.Id = Guid.NewGuid();
            var productRepository = _unitOfWork.GetRepository<IProductRepository>();
            var product = _mapper.Map<Product>(newProduct);

            var dataResult = productRepository.Add(product);
            _unitOfWork.Save();

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
            var productRepository = _unitOfWork.GetRepository<IProductRepository>();
            List<TestMessageResponse> list = new List<TestMessageResponse>();

            for (var index = 0; index < 1; index++)
            {
                var response = await _client.GetResponse<OperationResult<TestMessageResponse>>(new TestMessageRequest { Text = index.ToString() });

                list.Add(response.Message.Data);
            }
     
            Parallel.For(1, 1000, async index =>
            {
                await _publishEndpoint.Publish(new TestMessagePublish
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
