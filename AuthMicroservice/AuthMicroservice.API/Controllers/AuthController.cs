﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AuthMicroservice.BLL.Models.User;
using Microservice.Core.Infrastructure.OperationResult;
using AuthMicroservice.BLL.Models.DTO.User;
using AuthMicroservice.BLL.Services.Interfaces;
using System.Web;
using AuthMicroservice.API.Models.User;
using AuthMicroservice.BLL.Models.DTO.Token;
using Microsoft.AspNetCore.Http;
using System;

namespace AuthMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("token")]
        [Produces(typeof(OperationResult<TokenDTO>))]
        public async Task<ActionResult> GetToken([FromBody] UserLoginAPI userInfo)
        {
            var result = await _authService.GetToken(new UserLogin 
            { 
                Email = userInfo.Email,
                Password = userInfo.Password
            });

            if (result.IsSuccess)
            {
                var cookieOption = new CookieOptions
                {
                    MaxAge = TimeSpan.FromSeconds(result.Data.ExpiresIn),
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                HttpContext.Response.Cookies.Append("ASP.NET.Auth.Microservice.Access.Token", result.Data.AccessToken, cookieOption);
                HttpContext.Response.Cookies.Append("ASP.NET.Auth.Microservice.Refresh.Token", result.Data.RefreshToken, cookieOption);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("adduser")]
        [Produces(typeof(OperationResult<UserDTO>))]
        public async Task<ActionResult> AddUser([FromBody] UserRegisterAPI userInfo)
        {
            var result = await _authService.Add(new UserRegister
            {
                Email = userInfo.Email,
                Password = userInfo.Password
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("activate")]
        [Produces(typeof(OperationResult))]
        public ActionResult ActivateUser([FromQuery] UserActivationAPI activation)
        {
            var result = _authService.ActivateUser(activation.ActivationCode, activation.ActivationEmail);

            return Ok(result);
        }

    }
}
