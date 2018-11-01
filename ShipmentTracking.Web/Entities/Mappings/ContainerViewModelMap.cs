using FluentNHibernate.Mapping;

namespace ShipmentTracking.Web.Entities.Mappings
{
    public class ContainerViewModelMap : ClassMap<ContainerViewModel>
    {
        public ContainerViewModelMap()
        {
            Table("Container");
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