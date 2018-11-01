using Core;
using FluentAssertions;
using Messages.Commands;
using NServiceBus.Testing;
using NUnit.Framework;
using ShipmentTracking.Entities;
using ShipmentTracking.Handlers;
using System.Threading.Tasks;
using Transport.Messages.Events;
using Utilities;

namespace ShipmentTracking.Tests.HandlerTests.V2
{
    [TestFixture]
    public class ConfirmShipmentDeliveredWhenTransportJobDeliveredEventHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                s.ClearAllTables(new[]
                {
                    nameof(Shipment),
                    nameof(Container)
                });
            });
        }

        [Test]
        public async Task Handler_UpdatesShipmentStatus()
        {
            var testParams = Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var container = new TestObjectBuilder<Container>().BuildAndPersist(s);
                var shipment = new TestObjectBuilder<Shipment>()
                                .SetArgument(x => x.Container, container)
                                .BuildAndPersist(s);
                var evt = Test.CreateInstance<ITransportJobDeliveredEvent>(e =>
                {
                    e.TransportJobId = DataProvider.Get<int>();
                    e.ContainerId = container.Id;
                });
                return new
                {
                    ShipmentId = shipment.Id,
                    evt
                };
            });

            await Endpoint.Act<ConfirmShipmentDeliveredWhenTransportJobDeliveredEventHandler>(AssemblySetupFixture.EndpointTestContainer, (h, ctx) => h.Handle(testParams.evt, ctx));

            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var shipment = s.Get<Shipment>(testParams.ShipmentId);
                shipment.ShipmentStatus.Should().Be(ShipmentStatus.Delivered);
            });
        }
    }
}
