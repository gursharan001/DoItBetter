using Core;

namespace ShipmentTracking.Web.Entities
{
    public class ContainerViewModel
    {
        protected ContainerViewModel() { }

        public int Id { get; private set; }
        public string ContainerNumber { get; private set; }
        public ContainerType? ContainerType { get; private set; }
        public decimal? Weight { get; private set; }
        public decimal? Volume { get; private set; }
        public int? NumberOfPackages { get; private set; }
        public UOM? UnitOfMeasure { get; private set; }
    }
}