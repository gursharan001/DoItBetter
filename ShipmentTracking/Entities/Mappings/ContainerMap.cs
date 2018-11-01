using FluentNHibernate.Mapping;

namespace ShipmentTracking.Entities.Mappings
{
    public class ContainerMap : ClassMap<Container>
    {
        public ContainerMap()
        {
            Id(x => x.Id);
            Not.LazyLoad();

            Map(x => x.ContainerNumber);
            Map(x => x.ContainerType);
            Map(x => x.Weight);
            Map(x => x.Volume);
            Map(x => x.UnitOfMeasure);
            Map(x => x.NumberOfPackages);
        }
    }
}
