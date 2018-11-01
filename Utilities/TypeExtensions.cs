using System;
using System.Linq;
using System.Reflection;

namespace Utilities
{
    public static class TypeExtensions
    {
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Where(it => it.IsGenericType).Any(it => it.GetGenericTypeDefinition() == genericType))
            {
                return true;
            }

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return baseType.IsGenericType &&
                   baseType.GetGenericTypeDefinition() == genericType ||
                   IsAssignableToGenericType(baseType, genericType);
        }

        public static ConstructorInfo GetConstructorWithMostParameters(this Type type)
        {
            return type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetParameters().Any())
                .OrderBy(x => x.IsPublic ? 0 : 1)
                .ThenByDescending(x => x.GetParameters().Length)
                .FirstOrDefault();
        }

    }
}
