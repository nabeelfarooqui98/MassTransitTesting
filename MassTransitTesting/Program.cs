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
            var sendingQueue = "my_queue_1";
            await producer.Send(new CreateOrder { Description = "test", Name = "test" }, $"rabbitmq://10.164.1.53:5672/test1/{sendingQueue}");
            Console.ReadLine();
        }

        private static IBusControl SetupBus()
        {
            //works with in memory too, using rabbit mq to better see whats happening
            return MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://10.164.1.53:5672/test1"), hst =>
                {
                    hst.Username("test");
                    hst.Password("test");
                });

                cfg.ReceiveEndpoint("my_queue_1", c =>
                {
                    c.Consumer(typeof(OrderConsumer), type => new OrderConsumer());
                    c.Consumer(typeof(OrderFaultConsumer), type => new OrderFaultConsumer());
                });

                cfg.ReceiveEndpoint("my_queue_2", c =>
                {
                    c.Consumer(typeof(OrderConsumer), type => new OrderConsumer());
                    c.Consumer(typeof(OrderFaultConsumer), type => new OrderFaultConsumer());
                });

            });

        }
    }
}
