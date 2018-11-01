using Core;

namespace ShipmentTracking.Entities
{
    public class Container : IAggregate<int>
    {
        protected Container() { }

        public Container(string containerNumber, ContainerType? containerType, decimal? weight, decimal? volume, int? numberOfPackages, UOM? unitOfMeasure)
        {
            ContainerNumber = containerNumber;
            ContainerType = containerType;
            Weight = weight;
            Volume = volume;
            NumberOfPackages = numberOfPackages;
            UnitOfMeasure = unitOfMeasure;
        }

        public int Id { get; private set; }
        public string ContainerNumber { get; private set; }
        public ContainerType? ContainerType { get; private set; }
        public decimal? Weight { get; private set; }
        public decimal? Volume { get; private set; }
        public int? NumberOfPackages { get; private set; }
        public UOM? UnitOfMeasure { get; private set; }

        public void SetContainerNumber(string containerNumber)
        {
            ContainerNumber = containerNumber;
        }

        public void SetContainerType(ContainerType? containerType)
        {
            ContainerType = containerType;
        }

        public void SetCargoInformation(decimal? weight, decimal? volume, int? numberOfPackages, UOM? unitOfMeasure)
        {
            Weight = weight;
            Volume = volume;
            NumberOfPackages = numberOfPackages;
            UnitOfMeasure = unitOfMeasure;
        }
    }
}
