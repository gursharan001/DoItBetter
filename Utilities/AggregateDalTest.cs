using Core;
using NUnit.Framework;
using StructureMap;
using System;
using FluentAssertions;

namespace Utilities
{
    [TestFixture]
    [Category("Integration")]
    public abstract class AggregateDalTest<T, TIdentifier> where T : class, IAggregate<TIdentifier>
    {
        protected abstract IContainer ExecutionContainer { get; }

        protected virtual T CreateAggregate()
        {
            return DataProvider.Get<T>();
        }

        [Test]
        public void CanInsertAndRetrieveAggregate()
        {
            TestInsertAndRetrieve();
        }

        protected void TestInsertAndRetrieve(Func<T> creator = null)
        {
            var aggregate = Endpoint.ArrangeOnSqlSession(ExecutionContainer,
                session =>
                {
                    var item = creator != null
                        ? creator()
                        : CreateAggregate();

                    session.Save(item);
                    return item;
                });

            var retrievedAggregate = Endpoint.AssertOnSqlSessionThat(ExecutionContainer,
                session => session.Get<T>(aggregate.Id));

            retrievedAggregate.Should().BeEquivalentTo(aggregate,
                x => x.SqlDateComparison()
                    .IgnoringCyclicReferences());
        }
    }
}
