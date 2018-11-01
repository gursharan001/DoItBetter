using FluentNHibernate.Mapping;

namespace ShipmentTracking.Entities.Mappings
{
    public class ShipmentMap : ClassMap<Shipment>
    {
        public ShipmentMap()
        {
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
