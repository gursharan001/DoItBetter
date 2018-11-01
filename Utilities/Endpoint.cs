using NHibernate;
using NServiceBus.Testing;
using StructureMap;
using System;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Endpoint
    {
        // Use to make an arrangment on an endpoint, not using a handler.
        public static void Arrange(
            IContainer container,
            Action<IContainer> arrange)
        {
            using (var nc = container.GetNestedContainer())
            {
                using (var session = nc.GetInstance<ISession>())
                {
                    var tx = session.BeginTransaction();
                    try
                    {
                        arrange(nc);
                        tx.Commit();
                    }
                    catch
                    {
                        tx?.Rollback();
                        throw;
                    }
                }
            }
        }

        public static void ArrangeOnSqlSession(
            IContainer container,
            Action<ISession> arrange)
        {
            using (var nc = container.GetNestedContainer())
            {
                using (var session = nc.GetInstance<ISession>())
                {
                    var tx = session.BeginTransaction();
                    try
                    {
                        arrange(session);
                        tx.Commit();
                    }
                    catch
                    {
                        tx?.Rollback();
                        throw;
                    }
                }
            }
        }

        public static T Arrange<T>(
            IContainer container,
            Func<IContainer, T> arrange)
        {
            using (var nc = container.GetNestedContainer())
            {
                using (var session = nc.GetInstance<ISession>())
                {
                    var tx = session.BeginTransaction();
                    try
                    {
                        var result = arrange(nc);
                        tx.Commit();
                        return result;
                    }
                    catch
                    {
                        tx?.Rollback();
                        throw;
                    }
                }
            }
        }

        public static T ArrangeOnSqlSession<T>(
            IContainer container,
            Func<ISession, T> arrange)
        {
            return Arrange(container,
                nc =>
                {
                    var session = nc.GetInstance<ISession>();
                    return arrange(session);
                });
        }

        public static async Task<TestableMessageHandlerContext> Arrange<THandler>(
            IContainer container,
            Func<THandler, TestableMessageHandlerContext, Task> act)
        {
            return await Execute(container, act);
        }

        private static async Task<TestableMessageHandlerContext> Execute<THandler>(
            IContainer container,
            Func<THandler, TestableMessageHandlerContext, Task> act)
        {
            using (var nestedContainer = container.GetNestedContainer())
            {
                var messageContext = new TestableMessageHandlerContext();
                var nc = nestedContainer;
                using (var session = nc.GetInstance<ISession>())
                {
                    var tx = session.BeginTransaction();
                    try
                    {
                        var handler = nc.GetInstance<THandler>();
                        await act(handler, messageContext).ConfigureAwait(false);
                        tx.Commit();
                    }
                    catch
                    {
                        tx?.Rollback();
                        throw;
                    }
                }

                return messageContext;
            }
        }

        public static async Task<TestableMessageHandlerContext> Act<THandler>(
            IContainer container,
            Func<THandler, TestableMessageHandlerContext, Task> act)
        {
            return await Execute(container, act);
        }

        public static T ActOnContainer<T>(
            IContainer container,
            Func<IContainer, T> act)
        {
            using (var nc = container.GetNestedContainer())
            {
                using (var session = nc.GetInstance<ISession>())
                {
                    var tx = session.BeginTransaction();
                    try
                    {
                        var result = act(nc);
                        tx.Commit();
                        return result;
                    }
                    catch
                    {
                        tx?.Rollback();
                        throw;
                    }
                }
            }
        }

        public static void ActOnContainer(
            IContainer container,
            Action<IContainer> act)
        {
            using (var nc = container.GetNestedContainer())
            {
                using (var session = nc.GetInstance<ISession>())
                {
                    var tx = session.BeginTransaction();
                    try
                    {
                        act(nc);
                        tx.Commit();
                    }
                    catch
                    {
                        tx?.Rollback();
                        throw;
                    }
                }
            }
        }

        public static void AssertThat(
            IContainer container,
            Action<IContainer> assertions)
        {
            using (var nestedContainer = container.GetNestedContainer())
            {
                assertions(nestedContainer);
            }
        }

        public static T AssertThat<T>(
            IContainer container,
            Func<IContainer, T> assertions)
        {
            using (var nestedContainer = container.GetNestedContainer())
            {
                return assertions(nestedContainer);
            }
        }

        public static void AssertOnSqlSessionThat(
            IContainer container,
            Action<ISession> assertions)
        {
            AssertThat(container,
                c =>
                {
                    // Note: No unit of work here because we don't expect commits in assertions
                    var session = c.GetInstance<ISession>();
                    assertions(session);
                });
        }

        public static T AssertOnSqlSessionThat<T>(
            IContainer container,
            Func<ISession, T> assertions)
        {
            return AssertThat(container,
                c =>
                {
                    // Note: No unit of work here because we don't expect commits in assertions
                    var session = c.GetInstance<ISession>();
                    return assertions(session);
                });
        }
    }
}
