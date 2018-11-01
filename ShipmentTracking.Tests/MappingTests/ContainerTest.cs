using FluentAssertions;
using NUnit.Framework;
using ShipmentTracking.Entities;
using ShipmentTracking.Web.Entities;
using Utilities;

namespace ShipmentTracking.Tests.MappingTests
{
    [TestFixture]
    public class ContainerTest : AggregateDalTest<Container, int>
    {
        protected override StructureMap.IContainer ExecutionContainer => AssemblySetupFixture.EndpointTestContainer;

        [Test]
        public void ViewModel_Matches_Entity()
        {
            var entity = Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                return new TestObjectBuilder<Container>().BuildAndPersist(s);
            });

            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.WebTestContainer, s =>
            {
                var vm = s.Get<ContainerViewModel>(entity.Id);
                vm.Should().BeEquivalentTo(entity);
            });
        }
    }
}
