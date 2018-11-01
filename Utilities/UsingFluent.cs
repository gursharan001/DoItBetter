using System;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace Utilities
{
    public static class UsingFluent
    {
        public static EquivalencyAssertionOptions<T> SqlDateComparison<T>(this EquivalencyAssertionOptions<T> config)
        {
            return config.Using<DateTime>(DateTimeSqlPrecisionComparer).WhenTypeIs<DateTime>();
        }

        private static void DateTimeSqlPrecisionComparer(IAssertionContext<DateTime> ctx)
        {
            var retrieved = ctx.Subject;
            var expected = ctx.Expectation;
            retrieved.Should().BeCloseTo(expected, 1000);
        }
    }
}
