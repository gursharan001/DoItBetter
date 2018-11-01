using Messages.Events;
using NHibernate;
using NServiceBus;
using System;
using System.Threading.Tasks;
using Transport.Entities;

namespace Transport.Handlers
{
    public class CreateTransportJobWhenContainerAddedEventHandler : IHandleMessages<IContainerAddedEvent>
    {
        private readonly ISession _session;

        public CreateTransportJobWhenContainerAddedEventHandler(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task Handle(IContainerAddedEvent message, IMessageHandlerContext context)
        {
            var container = new Container(message.ContainerId, message.ContainerNumber, message.ContainerType, message.CargoDimensions);
            _session.Save(container);
            var transportJob = new TransportJob(container);
            _session.Save(transportJob);

            return Task.CompletedTask;
        }
    }
}
