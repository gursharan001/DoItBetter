using Core;
using System;

namespace Transport.Entities
{
    public class TransportJob : IAggregate<int>
    {
        protected TransportJob() { }

        public TransportJob(Container container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void ConfirmDelivered(DateTime timestamp)
        {
            DeliveredTimestamp = timestamp;
        }

        public int Id { get; private set; }
        public Container Container { get; private set; }
        public string RequestedDeliveryAddress { get; private set; }
        public DateTime? RequestedDeliveryDate { get; private set; }
        public DateTime? DeliveredTimestamp { get; private set; }
    }
}
