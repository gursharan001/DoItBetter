using Core;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using StructureMap;
using System.Reflection;

namespace Transport.Configuration
{
    public class NHibernateRegistry : Registry
    {
        public NHibernateRegistry(
            string connectionString,
            string schema,
            Assembly[] fluentAssemblies)
        {
            For<ISessionFactory>().Singleton().Use(SqlDatabase.Configure(
                connectionString,
                schema,
                fluentAssemblies,
                MsSqlConfiguration.MsSql2012));

            For<ISession>().ContainerScoped().Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());
        }
    }
}
