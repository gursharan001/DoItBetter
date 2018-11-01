using NServiceBus;

namespace Messages.Commands
{
    public class ConfirmShipmentDeliveredCommand : ICommand
    {
        public ConfirmShipmentDeliveredCommand(int shipmentId)
        {
            ShipmentId = shipmentId;
        }

        public int ShipmentId { get; private set; }
    }
}
