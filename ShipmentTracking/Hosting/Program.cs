using Topshelf;

namespace ShipmentTracking.Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.UseLog4Net();
                x.Service<NsbService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(_ => new NsbService());
                    serviceConfigurator.WhenStarted(svc => svc.Start());
                    serviceConfigurator.WhenStopped(svc => svc.Stop());
                });

            });
        }
    }
}
