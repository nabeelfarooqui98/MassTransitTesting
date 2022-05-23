using MassTransit;
using System;
using System.Threading.Tasks;

namespace MassTransitTesting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bus = SetupBus();
            bus.Start();

            bus.ConnectReceiveEndpoint("my_queue_1", c =>
            {
                c.Consumer(typeof(OrderConsumer), type => new OrderConsumer());
            });

            var producer = new OrderProducer(bus);
            var sendingQueue = "my_queue_1";
            producer.Send(new CreateOrder { Description = "test", Name = "test" }, $"exchange:{sendingQueue}").GetAwaiter().GetResult();
            Console.ReadLine();

        }

        private static IBusControl SetupBus()
        {
            //works with in memory too, using rabbit mq to better see whats happening
            var bus = MassTransit.Bus.Factory.CreateUsingInMemory(cfg =>
            {
                //cfg.Host(new Uri("rabbitmq://10.164.1.53:5672/test1"), hst =>
                //{
                //    hst.Username("test");
                //    hst.Password("test");
                //});

                cfg.ReceiveEndpoint();

                cfg.ReceiveEndpoint("my_queue_2", c =>
                {
                    c.Consumer(typeof(OrderConsumer), type => new OrderConsumer());
                });

                cfg.ReceiveEndpoint("fault_queue", c =>
                {
                    c.Consumer(typeof(OrderFaultConsumer), type => new OrderFaultConsumer());
                });

            });

           
            return bus;
        }
    }
}
