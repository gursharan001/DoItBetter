using Core;
using FluentAssertions;
using Messages.Commands;
using NUnit.Framework;
using ShipmentTracking.Entities;
using ShipmentTracking.Handlers;
using System.Threading.Tasks;
using Utilities;

namespace ShipmentTracking.Tests.HandlerTests
{
    [TestFixture]
    public class ConfirmShipmentDeliveredCommandHandlerTests
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
            var cmd = Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var container = new TestObjectBuilder<Container>().BuildAndPersist(s);
                var shipment = new TestObjectBuilder<Shipment>()
                                .SetArgument(x => x.Container, container)
                                .BuildAndPersist(s);
                shipment.ShipmentStatus.Should().Be(ShipmentStatus.Booked);
                return new ConfirmShipmentDeliveredCommand(shipment.Id);
            });

            await Endpoint.Act<ConfirmShipmentDeliveredCommandHandler>(AssemblySetupFixture.EndpointTestContainer, (h, ctx) => h.Handle(cmd, ctx));

            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var shipment = s.Get<Shipment>(cmd.ShipmentId);
                shipment.ShipmentStatus.Should().Be(ShipmentStatus.Delivered);
            });
        }
    }
}
