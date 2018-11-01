using NHibernate;
using NUnit.Framework;
using ShipmentTracking.Web.Configuration;
using StructureMap;

namespace ShipmentTracking.Tests
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
                Name = "ShipmentTracking.Tests"
            };

            Configuration.SetupContainer.InitialiseIoc(c);
            return c;
        }

        public static IContainer WebTestContainer => NsbWebContainer ?? (NsbWebContainer = CreateNsbWebContainer());

        private static IContainer NsbWebContainer { get; set; }

        private static IContainer CreateNsbWebContainer()
        {
            var container = new Container
            {
                Name = "ShipmentTracking.Web.Tests"
            };

            container.Configure(x =>
            {
                x.AddRegistry<ShipmentTrackingWebRegistry>();
                x.AddRegistry<IocTestRegistry>();
            });

            return container;
        }

        private class IocTestRegistry : Registry
        {
            public IocTestRegistry()
            {
                For<ISession>().Use(ctx => ctx.GetInstance<IShipmentTrackingSession>().Session);
                For<IStatelessSession>().Use(ctx => ctx.GetInstance<IShipmentTrackingStatelessSession>().Session);
            }
        }
    }
}
