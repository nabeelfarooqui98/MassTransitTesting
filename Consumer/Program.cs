using MassTransit;
using MassTransitTesting;
using System;

namespace Consumer
{
    internal class Program
    {
        const int numOfQueues = 2500;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var bus = SetupBus();
            Console.WriteLine("Bus setup.");
            bus.Start(TimeSpan.FromMinutes(20));
            Console.WriteLine("Bus started.");
            Console.ReadLine();
        }

        private static IBusControl SetupBus()
        {
            //works with in memory too, using rabbit mq to better see whats happening
            return MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://10.164.1.53:5672/memory"), hst =>
                {
                    hst.Username("test");
                    hst.Password("test");
                });

                for (int i = 0; i < numOfQueues; i++)
                {
                    cfg.ReceiveEndpoint($"my_queue_{i}", c =>
                    {
                        c.Consumer(typeof(OrderConsumer), type => new OrderConsumer());
                    });
                }

            });

        }
    }
}
