using FluentAssertions;
using Messages.Commands;
using NUnit.Framework;
using ShipmentTracking.Entities;
using ShipmentTracking.Handlers;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace ShipmentTracking.Tests.HandlerTests
{
    [TestFixture]
    public class BookShipmentCommandHandlerTests
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
        public async Task Handler_CreatesShipment()
        {
            var cmd = DataProvider.Get<BookShipmentCommand>();

            await Endpoint.Act<BookShipmentCommandHandler>(AssemblySetupFixture.EndpointTestContainer, (h, ctx) => h.Handle(cmd, ctx));
            
            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var shipment = s.Query<Shipment>().Single();
                shipment.Container.ContainerNumber.Should().Be(cmd.ContainerName);
                shipment.Container.ContainerType.Should().Be(cmd.ContainerType);
                shipment.Origin.Should().Be(cmd.Origin);
                shipment.Destination.Should().Be(cmd.Destination);
                shipment.Etd.Should().Be(cmd.Etd);
                shipment.Eta.Should().Be(cmd.Eta);
                shipment.Destination.Should().Be(cmd.Destination);
            });
        }
    }
}
