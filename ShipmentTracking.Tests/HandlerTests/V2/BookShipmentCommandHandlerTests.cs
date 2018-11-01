using FluentAssertions;
using Messages.Commands.V2;
using Messages.Events;
using NUnit.Framework;
using ShipmentTracking.Entities;
using ShipmentTracking.Handlers;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace ShipmentTracking.Tests.HandlerTests
{
    [TestFixture]
    public class BookShipmentAndTransportCommandHandlerTests
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
        public async Task Handler_CreatesShipment_PublishesEvent()
        {
            var cmd = DataProvider.Get<BookShipmentAndTransportCommand>();

            var testContext = await Endpoint.Act<BookShipmentAndTransportCommandHandler>(AssemblySetupFixture.EndpointTestContainer, (h, ctx) => h.Handle(cmd, ctx));
            var publishedEvent = testContext.PublishedMessages.Select(x => x.Message).Cast<IContainerAddedEvent>().Single();
            
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

                publishedEvent.ContainerId.Should().Be(shipment.Container.Id);
                publishedEvent.ContainerNumber.Should().Be(cmd.ContainerName);
                publishedEvent.ContainerType.Should().Be(cmd.ContainerType);
                publishedEvent.CargoDimensions.NumberOfPackages.Should().Be(shipment.Container.NumberOfPackages);
                publishedEvent.CargoDimensions.UnitOfMeasure.Should().Be(shipment.Container.UnitOfMeasure);
                publishedEvent.CargoDimensions.Weight.Should().Be(shipment.Container.Weight);
                publishedEvent.CargoDimensions.Volume.Should().Be(shipment.Container.Volume);
            });
        }
    }
}
