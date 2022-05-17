using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitTesting
{
    internal class OrderFaultConsumer : IConsumer<Fault<CreateOrder>>
    {
        public async Task Consume(ConsumeContext<Fault<CreateOrder>> context)
        {
            Console.WriteLine($"Fault consumed! {context.Message.Exceptions.FirstOrDefault().Message}");
        }
    }
}
