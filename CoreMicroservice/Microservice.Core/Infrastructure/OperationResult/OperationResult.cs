using Microservice.Core.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Microservice.Core.Infrastructure.OperationResult
{
    public class OperationResult : IOperationResult
    {
        public OperationResult()
        {
            Errors = new List<string>();
        }

        public ResultType Type { get; set; }

        public List<string> Errors { get; set; }

        public string Description { get { return Type.GetDisplayName(); } }

        [JsonIgnore]
        public bool IsSuccess { get { return Type == ResultType.Success; } }
    }

    public class OperationResult<T>: IOperationResult
    {
        public OperationResult()
        {
            Errors = new List<string>();
        }

        public ResultType Type { get; set; }

        public T Data { get; set; }

        public List<string> Errors { get; set; }

        public string Description { get { return Type.GetDisplayName(); } }

        [JsonIgnore]
        public bool IsSuccess { get { return Type == ResultType.Success; } }
    }
}
