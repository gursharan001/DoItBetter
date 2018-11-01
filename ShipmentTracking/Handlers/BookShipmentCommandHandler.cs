using Messages.Commands;
using NHibernate;
using NServiceBus;
using ShipmentTracking.Entities;
using System;
using System.Threading.Tasks;

namespace ShipmentTracking.Handlers
{
    public class BookShipmentCommandHandler : IHandleMessages<BookShipmentCommand>
    {
        private readonly ISession _session;

        public BookShipmentCommandHandler(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task Handle(BookShipmentCommand message, IMessageHandlerContext context)
        {
            var container = new Container(message.ContainerName, message.ContainerType, null, null, null, null);
            _session.Save(container);
            var shipment = new Shipment(message.Origin, message.Etd, message.Destination, message.Eta, container);
            _session.Save(shipment);
            return Task.CompletedTask;
        }
    }
}
