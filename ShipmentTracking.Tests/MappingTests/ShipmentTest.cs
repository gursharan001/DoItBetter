using FluentAssertions;
using NUnit.Framework;
using ShipmentTracking.Entities;
using ShipmentTracking.Web.Entities;
using Utilities;

namespace ShipmentTracking.Tests.MappingTests
{
    [TestFixture]
    public class ShipmentTest
    {
        
        [Test]
        public void Save_Works()
        {
            var expectedEntity = Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var container = DataProvider.Get<Container>();
                s.Save(container);
                var shipment = new TestObjectBuilder<Shipment>()
                                .SetArgument(x => x.Container, container)
                                .BuildAndPersist(s);
                return shipment;
            });

            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var shipment = s.Get<Shipment>(expectedEntity.Id);
                shipment.Should().BeEquivalentTo(expectedEntity);
            });
        }

        [Test]
        public void ViewModel_Matches_Entity()
        {
            var entity = Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var container = new TestObjectBuilder<Container>().BuildAndPersist(s);
                return new TestObjectBuilder<Shipment>()
                    .SetArgument(x => x.Container, container)
                    .BuildAndPersist(s);
            });

            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.WebTestContainer, s =>
            {
                var vm = s.Get<ShipmentViewModel>(entity.Id);
                vm.Should().BeEquivalentTo(entity);
            });
        }

        [Test]
        public void GridViewModel_Matches_Entity()
        {
            var entity = Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var container = new TestObjectBuilder<Container>().BuildAndPersist(s);
                return new TestObjectBuilder<Shipment>()
                    .SetArgument(x => x.Container, container)
                    .BuildAndPersist(s);
            });

            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.WebTestContainer, s =>
            {
                var vm = s.Get<ShipmentViewModel>(entity.Id);

                var gridVm = s.Get<ShipmentGridViewModel>(entity.Id);
                gridVm.Id.Should().Be(vm.Id);
                gridVm.Origin.Should().Be(vm.Origin);
                gridVm.Destination.Should().Be(vm.Destination);
                gridVm.Etd.Should().Be(vm.Etd);
                gridVm.Eta.Should().Be(vm.Eta);
                gridVm.ContainerNumber.Should().Be(vm.Container.ContainerNumber);
            });
        }
    }
}
