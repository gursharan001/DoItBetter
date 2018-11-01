using NUnit.Framework;
using StructureMap;

namespace Transport.Tests
{
    [SetUpFixture]
    public class AssemblySetupFixture
    {
        public static IContainer EndpointTestContainer => _container ?? (_container = CreateContainer());

        private static IContainer _container;

        private static IContainer CreateContainer()
        {
            var c = new Container
            {
                Name = "Transport.Tests"
            };

            Configuration.SetupContainer.InitialiseIoc(c);
            return c;
        }

        
    }
}
