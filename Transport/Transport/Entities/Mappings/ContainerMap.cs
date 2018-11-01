using FluentNHibernate.Mapping;

namespace Transport.Entities.Mappings
{
    public class ContainerMap : ClassMap<Container>
    {
        public ContainerMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Not.LazyLoad();

            Map(x => x.ContainerNumber);
            Map(x => x.ContainerType);
            Component(m => m.CargoDimensions, x =>
            {
                x.Map(y => y.NumberOfPackages);
                x.Map(y => y.UnitOfMeasure);
                x.Map(y => y.Volume);
                x.Map(y => y.Weight);
            });
        }
    }
}
