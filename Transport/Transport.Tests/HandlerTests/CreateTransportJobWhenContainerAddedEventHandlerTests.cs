using Core.ValueObjects;
using FluentAssertions;
using Messages.Events;
using NServiceBus.Testing;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Transport.Entities;
using Transport.Handlers;
using Utilities;

namespace Transport.Tests.HandlerTests
{
    [TestFixture]
    public class CreateTransportJobWhenContainerAddedEventHandlerTests
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
        public async Task Handler_CreatesTransportJob()
        {
            var evt = Test.CreateInstance<IContainerAddedEvent>(e =>
            {
                e.ContainerId = DataProvider.Get<int>();
                e.ContainerNumber = DataProvider.Get<string>();
                e.ContainerType = DataProvider.Get<Core.ContainerType>();
                e.CargoDimensions = DataProvider.Get<CargoDimensions>();
            });

            await Endpoint.Act<CreateTransportJobWhenContainerAddedEventHandler>(AssemblySetupFixture.EndpointTestContainer, (h, ctx) => h.Handle(evt, ctx));

            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var transportJob = s.Query<TransportJob>().Single();
                transportJob.Container.Id.Should().Be(evt.ContainerId);
                transportJob.Container.ContainerNumber.Should().Be(evt.ContainerNumber);
                transportJob.Container.ContainerType.Should().Be(evt.ContainerType);
                transportJob.Container.CargoDimensions.Should().BeEquivalentTo(evt.CargoDimensions);
                transportJob.DeliveredTimestamp.Should().NotHaveValue();
            });
        }
    }
}
