using NHibernate;
using NServiceBus;
using System;
using System.Threading.Tasks;
using Transport.Entities;
using Transport.Messages.Commands;
using Transport.Messages.Events;

namespace Transport.Handlers
{
    public class ConfirmTransportJobDeliveredCommandHandler : IHandleMessages<ConfirmTransportJobDeliveredCommand>
    {
        private readonly ISession _session;

        public ConfirmTransportJobDeliveredCommandHandler(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task Handle(ConfirmTransportJobDeliveredCommand message, IMessageHandlerContext context)
        {
            var transportJob = _session.Get<TransportJob>(message.TransportJobId);
            transportJob.ConfirmDelivered(DateTime.UtcNow);
            return context.Publish<ITransportJobDeliveredEvent>(e =>
            {
                e.ContainerId = transportJob.Container.Id;
                e.TransportJobId = transportJob.Id;
            });
        }
    }
}
