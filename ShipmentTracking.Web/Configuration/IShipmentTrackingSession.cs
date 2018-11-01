using NHibernate;
using System;

namespace ShipmentTracking.Web.Configuration
{
    public interface IShipmentTrackingSessionFactory
    {
        IShipmentTrackingSession OpenSession();
        IShipmentTrackingStatelessSession OpenStatelessSession();
    }

    public class ShipmentTrackingSessionFactory : IShipmentTrackingSessionFactory
    {
        public ShipmentTrackingSessionFactory(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        private ISessionFactory SessionFactory { get; }
        public IShipmentTrackingSession OpenSession()
        {
            return new ShipmentTrackingSession(SessionFactory.OpenSession());
        }

        public IShipmentTrackingStatelessSession OpenStatelessSession()
        {
            return new ShipmentTrackingStatelessSession(SessionFactory.OpenStatelessSession());
        }
    }

    public interface IShipmentTrackingSession : IDisposable
    {
        ISession Session { get; }
    }



    public class ShipmentTrackingSession : IShipmentTrackingSession
    {
        public ShipmentTrackingSession(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; }

        public void Dispose()
        {
            Session?.Dispose();
        }
    }


    public interface IShipmentTrackingStatelessSession : IDisposable
    {
        IStatelessSession Session { get; }
    }

    public class ShipmentTrackingStatelessSession : IShipmentTrackingStatelessSession
    {
        public ShipmentTrackingStatelessSession(IStatelessSession session)
        {
            Session = session;
        }

        public IStatelessSession Session { get; }

        public void Dispose()
        {
            Session?.Dispose();
        }
    }
}