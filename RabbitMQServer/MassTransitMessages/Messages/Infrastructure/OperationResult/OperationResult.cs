﻿using Microservice.Messages.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microservice.Messages.Infrastructure.OperationResult
{
    public class OperationResult<T> : IOperationResult 
        where T : class
    {
        public ResultType Type { get; set; }

        public string Description { get { return Type.GetDisplayName(); } }

        public T Data { get; set; }

        public List<string> Errors { get; set; }
    }
}
