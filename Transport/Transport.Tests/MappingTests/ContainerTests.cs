using NUnit.Framework;
using Utilities;

namespace Transport.Tests.MappingTests
{
    [TestFixture]
    public class ContainerTest : AggregateDalTest<Entities.Container, int>
    {
        [SetUp]
        public void Setup()
        {
            Endpoint.ArrangeOnSqlSession(AssemblySetupFixture.EndpointTestContainer, s =>
            {
                s.ClearAllTables(new[]
                {
                    nameof(Entities.Container)
                });
            });
        }

        protected override StructureMap.IContainer ExecutionContainer => AssemblySetupFixture.EndpointTestContainer;
    }
}
