using log4net;
using NServiceBus;
using StructureMap;

namespace ShipmentTracking.Configuration
{
    public class SetupContainer : INeedInitialization
    {
        public static ILog Log = LogManager.GetLogger(typeof(SetupContainer));
        public void Customize(EndpointConfiguration configuration)
        {
            Log.Info("Initializing Container");
            InitialiseIoc(ShipmentTrackingIoc.Container);
        }

        public static void InitialiseIoc(IContainer container)
        {
            container.Configure(c => c.AddRegistry<ShipmentTrackingRegistry>());
            container.Configure(c => c.AddRegistry(new NHibernateRegistry(
                EndpointConfig.ConnString,
                "dbo",
                EndpointConfig.NHibernateFluentAssemblies)));
        }
    }
}
