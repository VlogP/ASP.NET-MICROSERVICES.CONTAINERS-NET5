using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthMicroservice.BLL.Services.Interfaces;
using Microservice.Messages.Infrastructure.OperationResult;
using System.Collections.Generic;
using AuthMicroservice.BLL.Models.DTO.User;

namespace AuthMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/Auth/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Produces(typeof(OperationResult<List<UserDTO>>))]
        public ActionResult GetUsers()
        {
            var result = _userService.GetUsers();

            return Ok(result);
        }
    }
}
