using log4net;
using NServiceBus;
using StructureMap;

namespace Transport.Configuration
{
    public class SetupContainer : INeedInitialization
    {
        public static ILog Log = LogManager.GetLogger(typeof(SetupContainer));
        public void Customize(EndpointConfiguration configuration)
        {
            Log.Info("Initializing Container");
            InitialiseIoc(TransportIoc.Container);
        }

        public static void InitialiseIoc(IContainer container)
        {
            container.Configure(c => c.AddRegistry<TransportRegistry>());
            container.Configure(c => c.AddRegistry(new NHibernateRegistry(
                EndpointConfig.TransportConnString,
                "dbo",
                EndpointConfig.NHibernateFluentAssemblies)));
        }
    }
}
