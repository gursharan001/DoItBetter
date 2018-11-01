using NServiceBus;

namespace Transport.Messages.Commands
{
    public class ConfirmTransportJobDeliveredCommand : ICommand
    {
        public ConfirmTransportJobDeliveredCommand(int transportJobId)
        {
            TransportJobId = transportJobId;
        }

        public int TransportJobId { get; private set; }
    }
}
