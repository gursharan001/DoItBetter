using FluentNHibernate.Mapping;

namespace ShipmentTracking.Web.Entities.Mappings
{
    public class ShipmentGridViewModelMap : ClassMap<ShipmentGridViewModel>
    {
        public ShipmentGridViewModelMap()
        {
            Table("vw_ShipmentGrid");
            Id(x => x.Id);
            Not.LazyLoad();

            Map(x => x.Origin);
            Map(x => x.Etd);
            Map(x => x.Destination);
            Map(x => x.Eta);
            Map(x => x.ContainerNumber);
            Map(x => x.ShipmentStatus);
        }
    }
}