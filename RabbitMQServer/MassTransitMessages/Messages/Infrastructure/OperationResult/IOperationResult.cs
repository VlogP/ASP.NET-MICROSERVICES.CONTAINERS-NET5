using Microservice.Messages.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Messages.Infrastructure.OperationResult
{
    public interface IOperationResult
    {
        public ResultType Type { get; set; }

        public List<string> Errors { get; set; }
    }
}
