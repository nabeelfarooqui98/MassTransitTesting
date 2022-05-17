using MassTransit;
using System;
using System.Threading.Tasks;

namespace MassTransitTesting
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var bus = SetupBus();
            bus.Start();

            var producer = new OrderProducer(bus);
            await producer.Send(new CreateOrder { Description = "test", Name = "test" }, "queue:my_queue");

            Console.ReadLine();
        }

        private static IBusControl SetupBus()
        {
            return MassTransit.Bus.Factory.CreateUsingInMemory(cfg =>
            {
                //cfg.Host(new Uri(""), hst =>
                //{
                //    hst.Username("");
                //    hst.Password("");
                //});

                cfg.ReceiveEndpoint("my_queue", c =>
                {
                    c.Consumer(typeof(OrderConsumer), type => new OrderConsumer());
                    c.Consumer(typeof(OrderFaultConsumer), type => new OrderFaultConsumer());
                });

            });

        }
    }
}
