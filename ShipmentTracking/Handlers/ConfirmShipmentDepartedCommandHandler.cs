using Core;
using Messages.Commands;
using NHibernate;
using NServiceBus;
using ShipmentTracking.Entities;
using System;
using System.Threading.Tasks;

namespace ShipmentTracking.Handlers
{
    public class ConfirmShipmentDepartedCommandHandler : IHandleMessages<ConfirmShipmentDepartedCommand>
    {
        private readonly ISession _session;

        public ConfirmShipmentDepartedCommandHandler(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task Handle(ConfirmShipmentDepartedCommand message, IMessageHandlerContext context)
        {
            var shipment = _session.Get<Shipment>(message.ShipmentId);
            shipment.UpdateShipmentStatus(ShipmentStatus.Departed);
            return Task.CompletedTask;
        }
    }
}
