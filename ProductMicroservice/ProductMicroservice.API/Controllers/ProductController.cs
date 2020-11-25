using System.Threading.Tasks;
using ProductMicroservice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microservice.Core.Infrastructure.OperationResult;
using ProductMicroservice.BLL.Models.DTO.Product;
using ProductMicroservice.API.Models.Product;
using System.Collections.Generic;
using ProductMicroservice.BLL.Models.Product;
using AutoMapper;

namespace ProductMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces(typeof(OperationResult<List<ProductGetDTO>>))]
        public async Task<ActionResult> GetProducts()
        {
            var result = await _productService.GetAll();

            return Ok(result);
        }

        [HttpPost]
        [Produces(typeof(OperationResult<ProductPostDTO>))]
        public ActionResult AddProduct([FromBody] ProductPostAPI product)
        {
            var result = _productService.Add(_mapper.Map<ProductPost>(product));

            return Ok(result);
        }
    }
}
