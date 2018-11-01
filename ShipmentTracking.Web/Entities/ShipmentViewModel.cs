using Core;
using System;

namespace ShipmentTracking.Web.Entities
{
    public class ShipmentViewModel
    {
        protected ShipmentViewModel() { }

        public int Id { get; private set; }
        public string Origin { get; private set; }
        public DateTime? Etd { get; private set; }
        public string Destination { get; private set; }
        public DateTime? Eta { get; private set; }
        public ContainerViewModel Container { get; private set; }
        public ShipmentStatus ShipmentStatus { get; private set; }
    }
}
