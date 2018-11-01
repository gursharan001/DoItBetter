using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;

namespace Core
{
    public class EnumIntConvention : IPropertyConvention, IConventionAcceptance<IPropertyInspector>
    {
        public void Apply(IPropertyInstance target)
        {
            target.CustomType(target.Property.PropertyType);
        }

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Property.PropertyType.IsEnum ||
                // Check for nullable generic
                (
                    x.Property.PropertyType.IsGenericType
                    && x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    && x.Property.PropertyType.GetGenericArguments().Length == 1
                    && x.Property.PropertyType.GetGenericArguments()[0].IsEnum
                ));
        }
    }
}
