using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using System.Reflection;

namespace Core
{
    public static class SqlDatabase
    {
        public static NHibernate.Cfg.Configuration GetConfiguration(
            string connectionString,
            string defaultSchema,
            Assembly[] fluentAssemblies,
            MsSqlConfiguration sqlDialect)
        {
            var cfg = GetFluentConfiguration(connectionString, defaultSchema, fluentAssemblies, sqlDialect)
                .BuildConfiguration();
            return cfg;
        }

        public static ISessionFactory Configure(
            string connectionString,
            string defaultSchema,
            Assembly[] fluentAssemblies,
            MsSqlConfiguration sqlDialect)
        {
            var sessionFactory = GetFluentConfiguration(connectionString, defaultSchema, fluentAssemblies, sqlDialect)
                .BuildSessionFactory();
            return sessionFactory;
        }

        public static FluentConfiguration GetFluentConfiguration(string connectionString, string defaultSchema, Assembly[] fluentAssemblies, MsSqlConfiguration sqlDialect)
        {
            return Fluently.Configure()
                .Database(sqlDialect.ConnectionString(connectionString)
                    //.ShowSql()
                    .DefaultSchema(defaultSchema))
                .Mappings(m =>
                {
                    foreach (var assembly in fluentAssemblies)
                    {
                        m.FluentMappings.AddFromAssembly(assembly);
                    }

                    m.FluentMappings.Conventions.AddFromAssemblyOf<EnumIntConvention>()
                        .Conventions.Add(PrimaryKey.Name.Is(x => "Id"));
                    // .ExportTo(@"c:\temp\otDump"); // Uncomment to dump fluently generated .hbm.xml files
                })
                .ExposeConfiguration(c => { c.SetProperty("sql_types.keep_datetime", "true"); });
        }
    }
}
