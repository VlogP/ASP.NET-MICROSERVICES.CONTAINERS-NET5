using Microservice.Core.Infrastructure.OperationResult;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.Filters
{
    
    public class ControllerExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;

        public ControllerExceptionFilter(IHostingEnvironment environment, ILogger<ControllerExceptionFilter> logger)
        {           
            _environment = environment;
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var errorList = new List<string>();
            var data = new OperationResult<object>();
            var isDevEnv = _environment.IsDevelopment();

            errorList.Add(context.Exception.Message);
            _logger.LogError(context.Exception, context.Exception.Message);

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
