using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductMicroservice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProductMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var result = await _productService.GetAll();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult AddProducts()
        {
            var result = _productService.Add(new ProductMicroservice.DAL.Models.Product { Id = Guid.NewGuid(), Name = "Test"});

            return Ok(result);
        }
    }
}
