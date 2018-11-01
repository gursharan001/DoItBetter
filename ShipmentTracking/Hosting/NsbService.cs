using NServiceBus;
using ShipmentTracking.Configuration;
using System.Threading.Tasks;

namespace ShipmentTracking.Hosting
{
    public class NsbService
    {
        private IEndpointInstance _endpointInstance;
        public void Start()
        {
            AsyncStart().GetAwaiter().GetResult();
        }

        public void Stop()
        {
            _endpointInstance?.Stop().GetAwaiter().GetResult();
        }

        private async Task AsyncStart()
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointConfig.EndpointName);
            EndpointConfig.Customize(endpointConfiguration);
            _endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            ShipmentTrackingIoc.Container.Configure(x => x.For<IEndpointInstance>().Use(_endpointInstance));
        }
    }
}
