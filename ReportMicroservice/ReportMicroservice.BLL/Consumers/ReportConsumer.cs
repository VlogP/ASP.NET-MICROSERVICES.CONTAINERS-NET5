using MassTransit;
using Microservice.Messages.Messages.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroservice.BLL.Consumers
{
    public class ReportConsumer : IConsumer<TestMessagePublish>
    {
        public ReportConsumer()
        {

        }

        public async Task Consume(ConsumeContext<TestMessagePublish> context)
        {
            Console.WriteLine(context.Message.Id);
        }
    }
}
