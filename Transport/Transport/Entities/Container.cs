using Core;
using Core.ValueObjects;
using System;

namespace Transport.Entities
{
    public class Container : IAggregate<int>
    {
        protected Container() { }

        public Container(int id, string containerNumber, ContainerType? containerType, CargoDimensions cargoDimensions)
        {
            Id = id;
            ContainerNumber = containerNumber;
            ContainerType = containerType;
            CargoDimensions = cargoDimensions;
        }

        public int Id { get; private set; }
        public string ContainerNumber { get; private set; }
        public ContainerType? ContainerType { get; private set; }
        public CargoDimensions CargoDimensions { get; private set; }
        public DateTime? StorageDate { get; private set; }

        public void SetStorageDate(DateTime storageDate)
        {
            StorageDate = storageDate;
        }
    }
}
