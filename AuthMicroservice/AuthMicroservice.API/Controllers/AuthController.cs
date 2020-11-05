using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AuthMicroservice.API.Models;
using AuthMicroservice.BLL.Models.User;
using Microservice.Messages.Infrastructure.OperationResult;
using AuthMicroservice.BLL.Models.DTO.User;
using AuthMicroservice.BLL.Services.Interfaces;

namespace AuthMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController: ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        [Route("token")]
        [Produces(typeof(OperationResult<string>))]
        public async Task<ActionResult> GetToken([FromBody] UserLoginAPI userInfo)
        {
            var result = await _authService.GetToken(new UserLogin 
            { 
                Email = userInfo.Email,
                Password = userInfo.Password
            });

            return Ok(result);
        }

        [HttpPost]
        [Route("adduser")]
        [Produces(typeof(OperationResult<UserDTO>))]
        public ActionResult AddUser([FromBody] UserRegisterAPI userInfo)
        {
            var result = _authService.Add(new UserRegister
            {
                Email = userInfo.Email,
                Password = userInfo.Password
            });

            return Ok(result);
        }

    }
}
