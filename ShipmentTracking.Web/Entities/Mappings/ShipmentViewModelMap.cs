using FluentNHibernate.Mapping;

namespace ShipmentTracking.Web.Entities.Mappings
{
    public class ShipmentViewModelMap : ClassMap<ShipmentViewModel>
    {
        public ShipmentViewModelMap()
        {
            Table("Shipment");
            Id(x => x.Id);
            Not.LazyLoad();

            Map(x => x.Origin);
            Map(x => x.Etd);
            Map(x => x.Destination);
            Map(x => x.Eta);
            References(x => x.Container).Column("ContainerId");
            Map(x => x.ShipmentStatus);
        }
    }
}