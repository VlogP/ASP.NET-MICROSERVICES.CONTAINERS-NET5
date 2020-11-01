using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microservice.Messages.Infrastructure.OperationResult
{
    public enum ResultType
    {
        [Display(Name = "Undefined")]
        Undefined = 0,
        [Display(Name = "Success")]
        Success = 200,
        [Display(Name = "Bad Request")]
        BadRequest = 400,
        [Display(Name = "Internal Server Error")]
        Invalid = 500
    }
}
