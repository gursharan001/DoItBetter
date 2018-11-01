using Core;
using NServiceBus;
using System;

namespace Messages.Commands
{
    public class BookShipmentCommand : ICommand
    {
        public BookShipmentCommand(string origin, string destination, DateTime etd, DateTime eta, ContainerType containerType, string containerName)
        {
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            Etd = etd;
            Eta = eta;
            ContainerType = containerType;
            ContainerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
        }

        public string Origin { get; private set; }
        public string Destination { get; private set; }
        public DateTime Etd { get; private set; }
        public DateTime Eta { get; private set; }
        public ContainerType ContainerType { get; private set; }
        public string ContainerName { get; private set; }
    }
}
