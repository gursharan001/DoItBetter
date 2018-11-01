using NServiceBus;

namespace Messages.Commands
{
    public class ConfirmShipmentDepartedCommand : ICommand
    {
        public ConfirmShipmentDepartedCommand(int shipmentId)
        {
            ShipmentId = shipmentId;
        }

        public int ShipmentId { get; private set; }
    }
}
