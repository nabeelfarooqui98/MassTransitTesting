using MassTransit;
using MassTransitTesting;
using System;

namespace Producer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var bus = SetupBus();
            Console.WriteLine("Bus setup.");
            bus.Start();
            Console.WriteLine("Bus started.");

            //var producer = new OrderProducer(bus);
            //var message = new CreateOrder { Name = "hello" };
            //producer.Send(message, $"rabbitmq://10.164.1.53:5672/stress/my_queue_1");


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

            });

        }
    }
}
