using MassTransit;
using Microservice.Messages.Messages.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportBuisnessLayer.Consumers
{
    public class ReportConsumer : IConsumer<TestMessage>
    {
        public ReportConsumer()
        {

        }

        public async Task Consume(ConsumeContext<TestMessage> context)
        {
            Console.WriteLine(context.Message.Id);
        }
    }
}
