using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MassTransitTesting
{
    internal class Program
    {
        const int numOfQueues = 10;
        const int numOfMessages = 50;
        const int numOfBatches = 5;
        static void Main(string[] args)
        {
            var bus = SetupBus();
            bus.Start(TimeSpan.FromMinutes(10));

            var producer = new OrderProducer(bus);

            BulkProduce(producer).GetAwaiter().GetResult();

            Console.ReadLine();
        }

        private static async Task BulkProduce(OrderProducer producer)
        {
            Random ran = new Random();
            for (int i = 0; i < numOfBatches; i++)
            {
                var randomQueue = $"my_queue_{ran.Next(1, numOfQueues)}";

                var messages = new List<CreateOrder>();

                for (int j = 0; j < numOfMessages; j++)
                {
                    messages.Add(new CreateOrder { Id = i, Description = "test-desc", Name = $"Message from {randomQueue}" });
                }

                await producer.Send(messages, 
                                $"rabbitmq://10.164.1.53:5672/stress/{randomQueue}");

                if (i % 100 == 0)
                    Console.WriteLine("i: " + i);
            }
            Console.WriteLine("Done producing all messages");
        }

        private static IBusControl SetupBus()
        {
            //works with in memory too, using rabbit mq to better see whats happening
            return MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://10.164.1.53:5672/stress"), hst =>
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

                    if (i % 100 == 0)
                        Console.WriteLine("i: " + i);
                }

            });

        }
    }
}
