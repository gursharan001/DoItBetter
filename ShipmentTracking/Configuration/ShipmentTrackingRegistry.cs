using StructureMap;

namespace ShipmentTracking.Configuration
{
    class ShipmentTrackingRegistry : Registry
    {
        public ShipmentTrackingRegistry()
        {
            Scan(cfg =>
            {
                cfg.TheCallingAssembly();
                cfg.WithDefaultConventions();
            });
        }
    }
}
