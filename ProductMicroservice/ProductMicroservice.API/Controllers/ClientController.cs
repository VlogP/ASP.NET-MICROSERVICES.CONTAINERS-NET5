using AutoMapper;
using Microservice.Core.Infrastructure.OperationResult;
using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.API.Models.Client;
using ProductMicroservice.BLL.Models.Client;
using ProductMicroservice.BLL.Models.DTO.Client;
using ProductMicroservice.BLL.Services.Interfaces;

namespace ProductMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/Product/Client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpPost]
        [Produces(typeof(OperationResult<ClientPostDTO>))]
        public ActionResult AddClient([FromBody] ClientPostAPI product)
        {
            var result = _clientService.Add(_mapper.Map<ClientPost>(product));

            return Ok(result);
        }
    }
}
