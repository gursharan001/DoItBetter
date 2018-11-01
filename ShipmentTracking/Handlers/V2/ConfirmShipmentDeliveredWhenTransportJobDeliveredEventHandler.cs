using System;
using System.Linq;
using System.Threading.Tasks;
using Core;
using NHibernate;
using NServiceBus;
using ShipmentTracking.Entities;
using Transport.Messages.Events;

namespace ShipmentTracking.Handlers
{
    public class ConfirmShipmentDeliveredWhenTransportJobDeliveredEventHandler : IHandleMessages<ITransportJobDeliveredEvent>
    {
        private readonly ISession _session;

        public ConfirmShipmentDeliveredWhenTransportJobDeliveredEventHandler(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task Handle(ITransportJobDeliveredEvent message, IMessageHandlerContext context)
        {
            var shipment = _session.Query<Shipment>()
                                .Where(x => x.Container.Id == message.ContainerId)
                                .Single();
            shipment.UpdateShipmentStatus(ShipmentStatus.Delivered);
            return Task.CompletedTask;
        }
    }
}
