using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitTesting
{
    public class OrderProducer
    {
        private readonly IBusControl busControl;

        public OrderProducer(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        public async Task Send(CreateOrder createOrder, string address)
        {
            var endpoint = await busControl.GetSendEndpoint(new Uri(address));
            await endpoint.Send(createOrder);
            Console.WriteLine("Produced!");
        }
        public async Task Send(IEnumerable<CreateOrder> createOrders, string address)
        {
            var endpoint = await busControl.GetSendEndpoint(new Uri(address));
            await endpoint.SendBatch(createOrders);
        }
    }
}
