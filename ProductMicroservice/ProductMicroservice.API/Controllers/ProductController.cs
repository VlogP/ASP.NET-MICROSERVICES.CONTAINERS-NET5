﻿using System.Threading.Tasks;
using ProductMicroservice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.BLL.Models.DTO;
using ProductMicroservice.API.Models;
using Microservice.Core.Infrastructure.OperationResult;

namespace ProductMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Produces(typeof(OperationResult<ProductDTO>))]
        public async Task<ActionResult> GetProducts()
        {
            var result = await _productService.GetAll();

            return Ok(result);
        }

        [HttpPost]
        [Produces(typeof(OperationResult<ProductDTO>))]
        public ActionResult AddProducts([FromBody] ProductAPI product)
        {
            var result = _productService.Add(new ProductDTO { Name = product.Name});

            return Ok(result);
        }
    }
}
