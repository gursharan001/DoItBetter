using log4net;
using StructureMap;

namespace ShipmentTracking.Configuration
{
    public class ShipmentTrackingIoc
    {
        public static ILog Log = LogManager.GetLogger(typeof(ShipmentTrackingIoc));

        public static IContainer Container { get; set; }

        static ShipmentTrackingIoc()
        {
            Log.Info("Creating the StuctureMap Container.");
            Container = new Container()
            {
                Name = "ShipmentTrackingIoc"
            };
        }
    }
}
