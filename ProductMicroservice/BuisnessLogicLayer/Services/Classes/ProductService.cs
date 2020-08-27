using BuisnessLogicLayer.Services.Interfaces;
using MassTransit;
using MassTransit.Audit;
using Microservice.Messages.Messages.Test;
using ProductDataLayer.Models;
using ProductDataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogicLayer.Services.Classes
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private IPublishEndpoint _publishEndpoint;

        public ProductService(IProductRepository productRepository, IPublishEndpoint publishEndpoint)
        {
            _productRepository = productRepository;
            _publishEndpoint = publishEndpoint;
        }

        public void Add(Product product)
        {
            _productRepository.Add(product);
        }

        public List<Product> GetAll()
        {
            for(var index = 0; index <= 100000; index++)
            {
                _publishEndpoint.Publish<TestMessage>(new TestMessage { 
                    Id = Guid.NewGuid(),
                    Text = "TestText",
                    Value = 5                             
                });
            }

            return _productRepository.Get();
        }
    }
}
