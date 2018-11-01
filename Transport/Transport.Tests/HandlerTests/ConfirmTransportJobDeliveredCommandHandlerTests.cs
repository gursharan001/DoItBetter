using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Transport.Entities;
using Transport.Handlers;
using Transport.Messages.Commands;
using Transport.Messages.Events;
using Utilities;

namespace Transport.Tests.HandlerTests
{
    [TestFixture]
    public class ConfirmTransportJobDeliveredCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                s.ClearAllTables(new[]
                {
                    nameof(TransportJob),
                    nameof(Container)
                });
            });
        }

        [Test]
        public async Task Handler_UpdatesTransportJob_PublishesEvent()
        {
            var command = Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var container = new TestObjectBuilder<Container>().BuildAndPersist(s);
                var transportJob = new TestObjectBuilder<TransportJob>()
                                    .SetArgument(x => x.Container, container)
                                    .BuildAndPersist(s);
                var cmd = new ConfirmTransportJobDeliveredCommand(transportJob.Id);
                return cmd;
            });

            var context = await Endpoint.Act<ConfirmTransportJobDeliveredCommandHandler>(AssemblySetupFixture.EndpointTestContainer, (h, ctx) => h.Handle(command, ctx));
            var publishedEvt = context.PublishedMessages.Select(x => x.Message).OfType<ITransportJobDeliveredEvent>().Single();

            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var transportJob = s.Get<TransportJob>(command.TransportJobId);
                transportJob.DeliveredTimestamp.Should().HaveValue();

                publishedEvt.ContainerId.Should().Be(transportJob.Container.Id);
                publishedEvt.TransportJobId.Should().Be(transportJob.Id);
            });
        }
    }
}
