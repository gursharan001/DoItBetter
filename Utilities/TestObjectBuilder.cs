using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilities
{
    public class TestObjectBuilder<T>
    {
        private readonly Type _type;
        private readonly ParameterProvider _parameterProvider;
        private readonly ParameterInfo[] _parameterInfos;

        public TestObjectBuilder()
        {
            _type = typeof(T);
            _parameterProvider = new ParameterProvider();
            _parameterInfos = _type.GetConstructorWithMostParameters().GetParameters().ToArray();
        }

        public ParameterInfo[] ConstructorParameters => _parameterInfos;

        public TestObjectBuilder<T> SetArgument(string arg, object value)
        {
            if (!_parameterInfos.Select(x => x.Name).Contains(arg))
            {
                string msg =
                    string.Format(
                        "The constructor with the most parameters for type {0}, does not have a a parameter named {1}.",
                        _type.Name,
                        arg);

                throw new Exception(msg);
            }
            _parameterProvider.SetArgument(arg, value);
            return this;
        }

        public TestObjectBuilder<T> SetTypeProvider<T2>(Func<T2> provider)
        {
            _parameterProvider.SetTypeProvider(provider);
            return this;
        }

        public TestObjectBuilder<T> Clone(T entity)
        {
            _parameterInfos.ToList()
                .ForEach(paramInfo =>
                {
                    var paramName = paramInfo.Name;
                    var property =
                        _type.GetProperty(paramInfo.Name,
                            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public |
                            BindingFlags.IgnoreCase);
                    if (property == null)
                    {
                        throw new Exception(
                            "Clone will not work on this entity. For clone to work, each constructor parameter must have a correspondingly named property (case doesn't matter)");
                    }
                    var entityParamValue =
                        property.GetValue(entity, null);
                    SetArgument(paramName,
                        entityParamValue);
                });
            return this;
        }


        /// <summary>
        /// Sets an argument based on an expression to the Property name being set.
        /// Depends on the argument to the constructor being named consistently
        /// eg. string MyProperty { get; set; } => public MyClass(string myProperty)
        /// MyProperty => myProperty
        /// </summary>
        public TestObjectBuilder<T> SetArgument<TParam>(Expression<Func<T, TParam>> expr, TParam value)
        {
            var propertyName = MemberUtility.GetMemberInfo(expr).Name;
            var paramName = string.Format("{0}{1}",
                Char.ToLower(propertyName[0]),
                propertyName.Substring(1));

            return SetArgument(paramName, value);
        }

        public T Build()
        {
            var parameters = _parameterInfos
                .Select(p => _parameterProvider.GetValue(p))
                .ToArray();

            var constr = _type.GetConstructorWithMostParameters();
            return (T)constr.Invoke(parameters);
        }


        public T BuildAndPersist(ISession session)
        {
            var t = Build();
            session.Save(t);
            return t;
        }
    }
    public class ParameterProvider
    {
        private readonly IDictionary<string, object> _argumentSetter = new Dictionary<string, object>();

        public ParameterProvider SetArgument(string arg, object value)
        {
            _argumentSetter[arg] = value;
            return this;
        }

        private readonly IDictionary<Type, Func<object>> _providerOverrides = new Dictionary<Type, Func<object>>();

        public ParameterProvider SetTypeProvider<T2>(Func<T2> provider)
        {
            _providerOverrides.Add(typeof(T2), () => provider());
            return this;
        }

        public ParameterProvider SetTypeProvider<T2>(T2 value)
        {
            _providerOverrides.Add(typeof(T2), () => value);
            return this;
        }


        public object GetValue(ParameterInfo parameterInfo)
        {
            var parameterName = parameterInfo.Name;
            var parameterType = parameterInfo.ParameterType;

            if (_argumentSetter.ContainsKey(parameterName))
            {
                return _argumentSetter[parameterName];
            }

            var specialProviders = _providerOverrides.Select(x => new ProviderOverride(x.Key, x.Value));
            return DataProvider.Get(parameterType, specialProviders);
        }
    }
}
