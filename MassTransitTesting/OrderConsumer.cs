using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitTesting
{
    internal class OrderConsumer : IConsumer<CreateOrder>
    {
        public async Task Consume(ConsumeContext<CreateOrder> context)
        {
            Console.WriteLine($"Consumed ID: {context.Message.Id}, {context.Message.Name}");
            Task.Delay(1000);
        }
    }
}
