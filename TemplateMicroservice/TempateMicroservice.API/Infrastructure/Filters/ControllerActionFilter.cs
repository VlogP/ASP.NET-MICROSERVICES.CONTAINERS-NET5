using Microservice.Messages.Infrastructure.OperationResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TempateMicroservice.API.Infrastructure.Filters
{
    public class ControllerActionFilter : IActionFilter
    {
        public ControllerActionFilter()
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var objectResult = (context.Result as ObjectResult);
            var isResultExist = objectResult != null;

            if (isResultExist)
            {
                var operationResult = objectResult.Value as IOperationResult;
                var codeResult = (int)operationResult.Type;
                objectResult.StatusCode = codeResult;
            }
            else
            {
                var errorList = new List<string>();
                errorList.Add("Controller didn't return objectResult data");
                var data = new OperationResult<object>();
                data.Type = ResultType.Invalid;
                data.Errors = errorList;

                objectResult = new ObjectResult(data);
                objectResult.StatusCode = (int)data.Type;
            }

            context.Result = objectResult;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
