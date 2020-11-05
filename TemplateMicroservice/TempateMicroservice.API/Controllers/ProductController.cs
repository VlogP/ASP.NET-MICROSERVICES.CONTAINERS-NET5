using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TempateMicroservice.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TempateMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly ITempateService _productService;

        public ProductController(ILogger<ProductController> logger, ITempateService productService)
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
            var result = _productService.Add(new TempateMicroservice.DAL.Models.TemplateModel { Id = Guid.NewGuid(), Name = "Test"});

            return Ok(result);
        }
    }
}
