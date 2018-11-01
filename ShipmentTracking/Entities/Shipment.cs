using Core;
using System;

namespace ShipmentTracking.Entities
{
    public class Shipment : IAggregate<int>
    {
        protected Shipment() { }

        public Shipment(string origin, DateTime? etd, string destination, DateTime? eta, Container container)
        {
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
            Etd = etd ?? throw new ArgumentNullException(nameof(etd));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            Eta = eta ?? throw new ArgumentNullException(nameof(eta));
            Container = container ?? throw new ArgumentNullException(nameof(container));
            ShipmentStatus = ShipmentStatus.Booked;
        }

        public void UpdateShipmentStatus(ShipmentStatus shipmentStatus)
        {
            ShipmentStatus = shipmentStatus;
        }

        public int Id { get; private set; }
        public string Origin { get; private set; }
        public DateTime? Etd { get; private set; }
        public string Destination { get; private set; }
        public DateTime? Eta { get; private set; }
        public Container Container { get; private set; }
        public ShipmentStatus ShipmentStatus { get; private set; }
    }
}
