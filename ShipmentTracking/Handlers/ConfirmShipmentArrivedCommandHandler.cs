using Core;
using Messages.Commands;
using NHibernate;
using NServiceBus;
using ShipmentTracking.Entities;
using System;
using System.Threading.Tasks;

namespace ShipmentTracking.Handlers
{
    public class ConfirmShipmentArrivedCommandHandler : IHandleMessages<ConfirmShipmentArrivedCommand>
    {
        private readonly ISession _session;

        public ConfirmShipmentArrivedCommandHandler(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task Handle(ConfirmShipmentArrivedCommand message, IMessageHandlerContext context)
        {
            var shipment = _session.Get<Shipment>(message.ShipmentId);
            shipment.UpdateShipmentStatus(ShipmentStatus.Arrived);
            return Task.CompletedTask;
        }
    }
}
