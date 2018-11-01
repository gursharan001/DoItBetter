using StructureMap;

namespace Transport.Configuration
{
    class TransportRegistry : Registry
    {
        public TransportRegistry()
        {
            Scan(cfg =>
            {
                cfg.TheCallingAssembly();
                cfg.WithDefaultConventions();
            });
        }
    }
}
