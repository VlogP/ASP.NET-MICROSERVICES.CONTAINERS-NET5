using Microservice.Messages.Infrastructure.OperationResult;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.API.Infrastructure.Filters
{
    public class ControllerExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _environment;

        public ControllerExceptionFilter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var errorList = new List<string>();
            var data = new OperationResult<object>();
            var isDevEnv = _environment.IsDevelopment();

            errorList.Add(context.Exception.Message);

            if (isDevEnv)
            {
                errorList.Add(context.Exception.StackTrace);
            }

            data.Errors = errorList;
            data.Type = ResultType.Invalid;

            var result = new ObjectResult(data);
            result.StatusCode = (int)data.Type;

            context.Result = result;
        }
    }
}
