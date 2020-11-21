using Microservice.Core.Infrastructure.OperationResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.Filters
{
    public class ControllerResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var objectResult = context.Result as ObjectResult;
            var fluentValidationResult = objectResult?.Value as ValidationProblemDetails;
            var operationResult = new OperationResult<object>();

            if (fluentValidationResult != null)
            {
                var errors = fluentValidationResult.Errors
                    .SelectMany(item => item.Value.Select(value => item.Key + ": " + value))
                    .ToList();
                operationResult.Type = (ResultType)fluentValidationResult.Status;
                operationResult.Errors = errors;

                objectResult.Value = operationResult;
            }

            await next();
        }
    }
}
