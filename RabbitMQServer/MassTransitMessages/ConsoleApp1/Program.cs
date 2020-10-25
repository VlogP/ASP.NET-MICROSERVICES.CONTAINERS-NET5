using Microservice.Messages.Infrastructure.OperationResult;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var grege = "gregre";

            var c1111 = typeof(OperationResult<>).MakeGenericType(grege.GetType());

            var c11111 = c1111 as IOperationResult;

            var c = new OperationResult<string> { Data = "htrhrthtrhtrhtrhrth", Type = ResultType.Invalid};

            var c1 = c as object;

            var c2 = c1 as IOperationResult;

            var c3 = c2.Type;

            c2.Type = ResultType.Success;
        }
    }
}
