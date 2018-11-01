using log4net;
using StructureMap;

namespace Transport.Configuration
{
    public class TransportIoc
    {
        public static ILog Log = LogManager.GetLogger(typeof(TransportIoc));

        public static IContainer Container { get; set; }

        static TransportIoc()
        {
            Log.Info("Creating the StuctureMap Container.");
            Container = new Container()
            {
                Name = "TransportIoc"
            };
        }
    }
}
