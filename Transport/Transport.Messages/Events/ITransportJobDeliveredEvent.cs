using NServiceBus;

namespace Transport.Messages.Events
{
    public interface ITransportJobDeliveredEvent : IEvent
    {
        int TransportJobId { get; set; }
        int ContainerId { get; set; }
    }
}
