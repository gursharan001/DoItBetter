using FluentNHibernate.Mapping;

namespace Transport.Entities.Mappings
{
    public class TransportJobMap : ClassMap<TransportJob>
    {
        public TransportJobMap()
        {
            Id(x => x.Id);
            Not.LazyLoad();

            References(x => x.Container).Column("ContainerId");
            Map(x => x.RequestedDeliveryAddress);
            Map(x => x.RequestedDeliveryDate);
            Map(x => x.DeliveredTimestamp);
        }
    }
}
