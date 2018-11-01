using Messages.Commands.V2;
using Messages.Events;
using NHibernate;
using NServiceBus;
using ShipmentTracking.Entities;
using System;
using System.Threading.Tasks;

namespace ShipmentTracking.Handlers
{
    public class BookShipmentAndTransportCommandHandler : IHandleMessages<BookShipmentAndTransportCommand>
    {
        private readonly ISession _session;

        public BookShipmentAndTransportCommandHandler(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public async Task Handle(BookShipmentAndTransportCommand message, IMessageHandlerContext context)
        {
            var container = new Container(message.ContainerName, message.ContainerType, null, null, null, null);
            _session.Save(container);
            var shipment = new Shipment(message.Origin, message.Etd, message.Destination, message.Eta, container);
            _session.Save(shipment);
            await context.Publish<IContainerAddedEvent>(e =>
            {
                e.ContainerId = container.Id;
                e.ContainerNumber = container.ContainerNumber;
                e.ContainerType = container.ContainerType;
                e.CargoDimensions = new Core.ValueObjects.CargoDimensions(container.Weight, container.Volume, container.NumberOfPackages, container.UnitOfMeasure);
            });
        }
    }
}
