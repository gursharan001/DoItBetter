using Core;
using Core.ValueObjects;
using NServiceBus;

namespace Messages.Events
{
    public interface IContainerAddedEvent : IEvent
    {
        int ContainerId { get; set; }
        ContainerType? ContainerType { get; set; }
        string ContainerNumber { get; set; }
        CargoDimensions CargoDimensions { get; set; }
    }
}
