using MassTransit;
using Microservice.Core.Infrastructure.OperationResult;
using Microservice.Core.Messages.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroservice.BLL.ResponseConsumers
{
    public class ReportResponseConsumer: IConsumer<TestMessageRequest>
    {
        public ReportResponseConsumer()
        {

        }

        public async Task Consume(ConsumeContext<TestMessageRequest> context)
        {
            var respond = new OperationResult<TestMessageResponse> { 
                Data = new TestMessageResponse {
                    Text = context.Message.Text
                },
                Type = ResultType.Success
            };

            await context.RespondAsync<OperationResult<TestMessageResponse>>(respond);
        }
    }
}
