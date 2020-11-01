using AuthMicroservice.BLL.Models;
using AuthMicroservice.BLL.Services.Interfaces;
using AuthMicroservice.API.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult> GetToken([FromBody] UserLoginModelWeb userInfo)
        {
            var result = await _authService.GetToken(new UserLoginModelBL 
            { 
                Email = userInfo.Email,
                Password = userInfo.Password
            });

            return Ok(result);
        }

    }
}
