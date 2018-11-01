using Core;
using Messages.Commands;
using NHibernate;
using NServiceBus;
using ShipmentTracking.Entities;
using System;
using System.Threading.Tasks;

namespace ShipmentTracking.Handlers
{
    public class ConfirmShipmentDeliveredCommandHandler : IHandleMessages<ConfirmShipmentDeliveredCommand>
    {
        private readonly ISession _session;

        public ConfirmShipmentDeliveredCommandHandler(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task Handle(ConfirmShipmentDeliveredCommand message, IMessageHandlerContext context)
        {
            var shipment = _session.Get<Shipment>(message.ShipmentId);
            shipment.UpdateShipmentStatus(ShipmentStatus.Delivered);
            return Task.CompletedTask;
        }
    }
}
