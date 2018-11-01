using log4net;
using log4net.Config;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;
using Transport.Entities;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using Transport.Messages.Commands;
using Messages.Events;

namespace Transport.Configuration
{
    public class EndpointConfig
    {
        public static ILog Log;
        public static string EndpointName => GetStringConfigValue(nameof(EndpointName));
        public static string ShipmentTrackingEndpointName => GetStringConfigValue("ShipmentTrackingEndpointName");
        public static string ErrorQueue => GetStringConfigValue(nameof(ErrorQueue));
        public static int ImmediateRetries => GetIntConfigValue(nameof(ImmediateRetries));
        public static bool DelayedRetriesEnabled => GetBoolConfigValue(nameof(DelayedRetriesEnabled));
        public static int DelayedRetries => GetIntConfigValue(nameof(DelayedRetries));
        public static int DelayedRetriesTimeIncreaseSeconds => GetIntConfigValue(nameof(DelayedRetriesTimeIncreaseSeconds));
        public static int Concurrency => GetIntConfigValue(nameof(Concurrency));
        public static int CacheSubscriptionsForMinutes => GetIntConfigValue(nameof(CacheSubscriptionsForMinutes));
        public static string AuditQueue => GetStringConfigValue(nameof(AuditQueue));
        public static int AuditOverrideTimeToBeReceivedSeconds => GetIntConfigValue(nameof(AuditOverrideTimeToBeReceivedSeconds));

        public static string NsbConnString => ConfigurationManager.ConnectionStrings["ShipmentTracking"].ConnectionString;
        public static string TransportConnString => ConfigurationManager.ConnectionStrings["Transport"].ConnectionString;

        public static (Assembly, string)[] MessageRoutes = {
            (typeof(ConfirmTransportJobDeliveredCommand).Assembly, EndpointName)
        };

        public static (Type, string)[] TypePublisherRoutes = {
            (typeof(IContainerAddedEvent), ShipmentTrackingEndpointName)
        };

        public static Assembly[] NHibernateFluentAssemblies =
        {
            typeof(Container).Assembly
        };

        public static void Customize(EndpointConfiguration endpointConfiguration)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository);
            var logFactory = NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            if (Log == null)
                Log = LogManager.GetLogger(typeof(EndpointConfig));

            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<SqlUnitOfWork>(DependencyLifecycle.InstancePerUnitOfWork);
                });

            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo(ErrorQueue);
            endpointConfiguration.UseContainer<StructureMapBuilder>(customizations => customizations.ExistingContainer(TransportIoc.Container));
            endpointConfiguration.UseSerialization<XmlSerializer>();

            var sqlPersistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            sqlPersistence.SqlDialect<SqlDialect.MsSqlServer>().Schema("nsb");
            sqlPersistence.ConnectionBuilder(() => new SqlConnection(NsbConnString));
            sqlPersistence.SubscriptionSettings().CacheFor(TimeSpan.FromMinutes(CacheSubscriptionsForMinutes));

            endpointConfiguration.Recoverability().Delayed(settings =>
            {
                if (DelayedRetriesEnabled)
                {
                    settings.NumberOfRetries(DelayedRetries);
                    settings.TimeIncrease(TimeSpan.FromSeconds(DelayedRetriesTimeIncreaseSeconds));
                }
                else
                {
                    settings.NumberOfRetries(0);
                }
            });
            endpointConfiguration.Recoverability().Immediate(settings => settings.NumberOfRetries(ImmediateRetries));
            endpointConfiguration.TimeoutManager().LimitMessageProcessingConcurrencyTo(Concurrency);

            endpointConfiguration.AuditProcessedMessagesTo(AuditQueue);
            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.UseSchemaForEndpoint(EndpointName, "nsb");
            transport.UseSchemaForEndpoint(ShipmentTrackingEndpointName, "nsb");
            transport.ConnectionString(NsbConnString);
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.UseSchemaForQueue(ErrorQueue, "nsb");
            transport.UseSchemaForQueue(AuditQueue, "nsb");

            var routing = transport.Routing();
            foreach (var (assembly, endpoint) in MessageRoutes)
            {
                routing.RegisterPublisher(assembly, endpoint);
                routing.RouteToEndpoint(assembly, endpoint);
            }
            foreach (var (type, endpoint) in TypePublisherRoutes)
            {
                routing.RegisterPublisher(type, endpoint);
            }
        }

        private static string GetStringConfigValue(string appSettingKey)
        {
            return ConfigurationManager.AppSettings[appSettingKey] ??
                   throw new Exception($"No {appSettingKey} specified in config");
        }

        private static int GetIntConfigValue(string appSettingKey)
        {
            return int.TryParse(ConfigurationManager.AppSettings[appSettingKey], out var p)
                ? p
                : throw new Exception($"No {appSettingKey} specified in config");
        }

        private static bool GetBoolConfigValue(string appSettingKey)
        {
            return bool.TryParse(ConfigurationManager.AppSettings[appSettingKey], out var p)
                ? p
                : throw new Exception($"No {appSettingKey} specified in config");
        }
    }
}
