using NServiceBus;

namespace Messages.Commands
{
    public class ConfirmShipmentArrivedCommand : ICommand
    {
        public ConfirmShipmentArrivedCommand(int shipmentId)
        {
            ShipmentId = shipmentId;
        }

        public int ShipmentId { get; private set; }
    }
}
