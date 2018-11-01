using Core;
using FluentNHibernate.Cfg.Db;
using ShipmentTracking.Web.Entities;
using StructureMap;
using System.Configuration;
using System.Reflection;

namespace ShipmentTracking.Web.Configuration
{
    public class ShipmentTrackingWebRegistry : Registry
    {
        public ShipmentTrackingWebRegistry()
        {
            Scan(cfg =>
            {
                cfg.TheCallingAssembly();
                cfg.WithDefaultConventions();
            }
            );

            ForSingletonOf<IShipmentTrackingSessionFactory>().Use(() => GetSessionFactory());
            For<IShipmentTrackingSession>()
                .Use(ctx => ctx.GetInstance<IShipmentTrackingSessionFactory>()
                    .OpenSession());
            For<IShipmentTrackingStatelessSession>()
                .Use(ctx => ctx.GetInstance<IShipmentTrackingSessionFactory>()
                    .OpenStatelessSession());
        }

        private static readonly Assembly[] FluentMappedAssemblies =
        {
            typeof(ShipmentViewModel).Assembly
        };

        private static IShipmentTrackingSessionFactory GetSessionFactory()
        {
            var sessionFactory = SqlDatabase.Configure(
                ConfigurationManager.ConnectionStrings["ShipmentTracking"].ConnectionString,
                "dbo",
                FluentMappedAssemblies,
                MsSqlConfiguration.MsSql2012);

            return new ShipmentTrackingSessionFactory(sessionFactory);
        }
    }
}