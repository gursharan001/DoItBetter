using FluentAssertions;
using NUnit.Framework;
using Transport.Entities;
using Utilities;
using Container = Transport.Entities.Container;

namespace Transport.Tests.MappingTests
{
    [TestFixture]
    public class TransportJobTest
    {
        [SetUp]
        public void Setup()
        {
            Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                s.ClearAllTables(new[]
                {
                    nameof(Container)
                });
            });
        }

        [Test]
        public void MappingWorks()
        {
            var entity = Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var container = new TestObjectBuilder<Container>().BuildAndPersist(s);
                var transportJob = new TestObjectBuilder<TransportJob>()
                                    .SetArgument(x => x.Container, container)
                                    .BuildAndPersist(s);
                return transportJob;
            });

            Endpoint.AssertOnSqlSessionThat(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                var retrievedEntity = s.Get<TransportJob>(entity.Id);
                retrievedEntity.Should().BeEquivalentTo(entity);
            });
        }
    }
}
