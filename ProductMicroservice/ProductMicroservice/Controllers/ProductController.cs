using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuisnessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProductMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger,IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public ActionResult GetProducts()
        {
            var result = _productService.GetAll();


            return Ok(result);
        }

        [HttpPost]
        public ActionResult AddProducts()
        {
            _productService.Add(new ProductDataLayer.Models.Product { Id = Guid.NewGuid(), Name = "Test"});

            return Ok();
        }
    }
}
