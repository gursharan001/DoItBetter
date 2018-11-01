using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utilities
{

    public static class DataProvider
    {
        public static T Get<T>(params ProviderOverride[] providers)
        {
            return (T)Get(typeof(T), providers);
        }

        public static T GetNotMatching<T>(params T[] values)
        {
            const int maxAttempts = 10;
            for (int i = 0; i < maxAttempts; i++)
            {
                var attempt = Get<T>();
                if (!values.Contains(attempt))
                {
                    return attempt;
                }
            }
            throw new Exception($"DataProvider GetNotMatching <{typeof(T)}> failed to provide with maximum number of attempts");
        }

        public static T[] GetArray<T>(int size = DefaultArrayLength)
        {
            return (T[])AnArrayOfType(typeof(T), size);
        }

        public static object Get(Type type, IEnumerable<ProviderOverride> providers)
        {
            providers.ToList().ForEach(p => _providers[p.Type] = p.Provider);
            object value = Get(type, true);
            RestoreDefaultProviders();
            return value;
        }

        public static object Get(Type t, bool allowSetProperties = false)
        {
            var nullable = Nullable.GetUnderlyingType(t);

            if (nullable != null)
                return Get(nullable, allowSetProperties);

            if (IsBasicType(t)) return _providers[t]();

            if (t.IsEnum)
                return RandomEnum(t);

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var genericArgs = t.GetGenericArguments();

                if (genericArgs.Length == 1 && IsBasicType(genericArgs[0]))
                    return _providers[genericArgs[0]]();

                return null;
            }

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                return GetKeyValuePair(t);
            }

            if (typeof(Guid) == t)
                return Guid.NewGuid();

            var enumerableObject = TryGettingAnEnumerableObject(t, allowSetProperties);
            if (enumerableObject != null) return enumerableObject;

            var dictionaryObject = TryGettingADictionaryObject(t);
            if (dictionaryObject != null) return dictionaryObject;

            if (t.IsClass || t.IsValueType)
            {
                return CreateObject(t, allowSetProperties);
            }

            throw new Exception($"Cannot create type: {t}");
        }

        private static object TryGettingADictionaryObject(Type type)
        {
            if (type.IsGenericType)
            {
                var isGenericDictionary = type.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                                          || type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
                if (isGenericDictionary)
                {
                    Type[] typeArguments = type.GetGenericArguments();
                    Type openGenericDictionary = typeof(Dictionary<,>);
                    Type actualDictionaryType = openGenericDictionary.MakeGenericType(typeArguments);
                    var emptyConstructor = actualDictionaryType.GetConstructor(new Type[0]);
                    if (emptyConstructor != null)
                    {
                        var dictionary = (IDictionary)emptyConstructor.Invoke(new object[0]);
                        object key = Get(typeArguments[0]);
                        object value = Get(typeArguments[1]);
                        dictionary.Add(key, value);

                        return dictionary;
                    }
                }
            }
            return null;
        }

        private static object TryGettingAnEnumerableObject(Type type, bool allowSetProperties = false)
        {
            if (type.IsGenericType)
            {
                return GetArrayIfCompatible(type, allowSetProperties);
            }

            var t = FindOutTheGenericDefinitionOfType(type.GetInterfaces());
            return t != null ? GetArrayIfCompatible(t, allowSetProperties) : null;
        }

        private static Type FindOutTheGenericDefinitionOfType(IEnumerable<Type> interfaces)
        {
            return interfaces
                .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private static object GetArrayIfCompatible(Type type, bool allowSetProperties)
        {
            return type.IsAssignableFrom(type.GetGenericArguments()[0].MakeArrayType())
                ? AnArrayOfType(type.GetGenericArguments()[0], allowSetProperties: allowSetProperties)
                : null;
        }

        private static object AnArrayOfType(Type enumerableType, int size = DefaultArrayLength, bool allowSetProperties = false)
        {
            if (enumerableType != null)
            {
                var a = Array.CreateInstance(enumerableType, size);
                for (var i = 0; i < size; i++)
                {
                    a.SetValue(Get(enumerableType, allowSetProperties), i);
                }
                return a;
            }
            return null;
        }

        private static object CreateObject(Type type, bool allowSetProperties = false)
        {
            var constructorWithMostParameters = type.GetConstructorWithMostParameters();
            if (constructorWithMostParameters != null)
            {
                var parameters = GetMethodParametersArray(constructorWithMostParameters);
                return constructorWithMostParameters.Invoke(parameters);
            }
            if (!type.GetProperties().Any())
            {
                var defaultConstructor = type.GetConstructor(new Type[0]);
                if (defaultConstructor != null) return defaultConstructor.Invoke(new object[0]);
            }

            if (allowSetProperties)
            {
                return ObjectPropertySetter.CreateObjectWithAllPropertiesSet(type);
            }

            var msg = $"For the DataProvider to create an instance of {type.Name}, it needs a public parameterised constructor";
            throw new Exception(msg);
        }

        private static object[] GetMethodParametersArray(ConstructorInfo constructorWithMostParameters)
        {
            var parameters = constructorWithMostParameters
                .GetParameters()
                .Select(x => x.ParameterType)
                .Select(x => Get(x))
                .ToArray();
            return parameters;
        }

        static DataProvider()
        {
            Random = new Random();
            RestoreDefaultProviders();
        }

        public static string CreatePrefixed(string prefix)
        {
            var provider = _providers[typeof(string)];
            return prefix + provider();
        }

        public static char[] Letters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q',
            'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        public static char[] Digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };


        public static char GetRandomLetter()
        {
            return Letters[Random.Next(0, 25)];
        }

        public static char GetNextMaskedChar(char mask)
        {
            if (mask == '#')
                return Digits[Random.Next(0, 9)];
            if (mask == '!')
                return GetRandomLetter();
            if (mask == '@')
                return char.ToUpper(GetRandomLetter());
            return mask;
        }

        public static string GetStringLike(string mask)
        {
            var b = new StringBuilder();
            foreach (char c in mask)
            {
                char nextChar = GetNextMaskedChar(c);
                b.Append(nextChar);
            }
            return b.ToString();
        }

        public static bool IsBasicType(Type type)
        {
            return _providers.ContainsKey(type);
        }

        public static T Create<T>()
        {
            if (typeof(T).IsEnum)
                return RandomEnum<T>();
            var provider = _providers[typeof(T)];
            return (T)provider();
        }

        private static readonly IDictionary<Type, Func<object>> CustomProviders = new Dictionary<Type, Func<object>>();

        public static void RegisterCustomProviders(IDictionary<Type, Func<object>> customProviders)
        {
            foreach (var provider in customProviders)
            {
                _providers[provider.Key] = provider.Value;
                CustomProviders[provider.Key] = provider.Value;
            }
        }

        private static void RestoreDefaultProviders()
        {
            _providers.Clear();
            _providers = new Dictionary<Type, Func<object>>(DefaultProviders);
            foreach (var provider in CustomProviders)
            {
                _providers.Add(provider);
            }
        }

        private static readonly IDictionary<Type, Func<object>> DefaultProviders = new Dictionary<Type, Func<object>>
        {
            {typeof(DateTime), RandomDate},
            {typeof(TimeSpan), RandomTimeSpan},
            {typeof(DateTimeOffset), RandomDateTimeOffset},
            {typeof(int), NextInt},
            {typeof(string), NextString},
            {typeof(char), NextChar},
            {typeof(bool), () => false},
            {typeof(decimal), NextDecimal},
            {typeof(double), RandomDouble},
            {typeof(byte), NextByte},
        };

        private static object GetKeyValuePair(Type t)
        {
            var typeArguments = t.GetGenericArguments();
            var openGenericType = typeof(KeyValuePair<,>);
            var concreteType = openGenericType.MakeGenericType(typeArguments);
            var constructor = concreteType.GetConstructor(typeArguments);

            return constructor?.Invoke(new[]
            {
                Get(typeArguments[0]),
                Get(typeArguments[1])
            });
        }

        private static IDictionary<Type, Func<object>> _providers = new Dictionary<Type, Func<object>>();

        private static readonly Random Random;

        private static object RandomDate()
        {
            return RandomDate(DateTimeKind.Unspecified);
        }

        private static object RandomTimeSpan()
        {
            return TimeSpan.FromMinutes(Random.Next(0, 3600));
        }

        public static DateTime RandomDate(DateTimeKind kind)
        {
            return RandomDateWithinGivenDaysFromToday(1000, kind);
        }

        public static DateTime RandomDateOnDaysOfWeek(DateTimeKind kind, params DayOfWeek[] daysOfWeek)
        {
            var date = RandomDateWithinGivenDaysFromToday(1000, kind);
            while (!daysOfWeek.Contains(date.DayOfWeek))
            {
                date = date.AddDays(1);
            }
            return date;
        }

        public static string GetValidEmail()
        {
            return $"{GetStringOfLength(10)}@{GetStringOfLength(10)}.com";
        }

        public static DateTime RandomDateWithinGivenDaysFromToday(uint plusMinusDays, DateTimeKind kind = DateTimeKind.Unspecified)
        {
            int hours = Random.Next(0, 23);
            int minutes = Random.Next(0, 59);
            int seconds = Random.Next(0, 59);

            var now = kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;

            var randomDate = new DateTime(now.Year, now.Month, now.Day, hours, minutes, seconds, kind);

            int randomInt = Random.Next((int)(-1 * plusMinusDays), (int)plusMinusDays);

            return randomDate.AddDays(randomInt);
        }

        private static object RandomDateTimeOffset()
        {
            int hours = Random.Next(0, 23);
            int minutes = Random.Next(0, 59);
            int seconds = Random.Next(0, 59);

            int randomMonth = Random.Next(1, 12);
            int randomDay = Random.Next(1, 28);

            int thisYear = DateTime.Today.Year;

            var randomDate = new DateTimeOffset(thisYear, randomMonth, randomDay, hours, minutes, seconds, TimeSpan.Zero);

            int randomInt = Random.Next(-1000, 1000);

            return randomDate.AddDays(randomInt);
        }

        private static T RandomEnum<T>()
        {
            Array allValues = Enum.GetValues(typeof(T));
            int randomIndex = Random.Next(0, allValues.Length);
            return (T)allValues.GetValue(randomIndex);
        }

        private static object RandomDouble()
        {
            return Random.NextDouble();
        }

        private static object RandomEnum(Type t)
        {
            Array allValues = Enum.GetValues(t);
            int randomIndex = Random.Next(0, allValues.Length);
            return allValues.GetValue(randomIndex);
        }

        private static int _nextInt = 1;

        private static object NextInt()
        {
            return _nextInt++;
        }

        private static decimal _nextDecimal = 10.0m;

        private static object NextDecimal()
        {
            _nextDecimal += 0.1m;
            return _nextDecimal;
        }

        private static Byte _nextByte = 1;
        private static object NextByte()
        {
            return _nextByte++;
        }

        private static int _nextStringNumber = 1;
        private const int DefaultArrayLength = 2;

        private static object NextString()
        {
            return $"{_nextStringNumber++:D5}";
        }

        private static int _nextCharNumber = 1;
        private static object NextChar()
        {
            int num = ++_nextCharNumber;
            if (_nextCharNumber == 25) _nextCharNumber = 0;
            return (char)('a' + num);
        }

        private static readonly Dictionary<Type, int> NextEnumIndex = new Dictionary<Type, int>();
        private static object NextEnumValue<T>() where T : struct, IConvertible
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var enumValues = Enum.GetValues(enumType).Cast<T>().ToArray();

            if (!NextEnumIndex.ContainsKey(enumType)) NextEnumIndex[enumType] = 0;

            var nextEnumIndex = NextEnumIndex[enumType];
            var returnValue = enumValues[nextEnumIndex];

            if (++nextEnumIndex >= enumValues.Length) nextEnumIndex = 0;
            NextEnumIndex[enumType] = nextEnumIndex;
            return returnValue;
        }

        public static string GetStringOfLength(int length)
        {
            if (length < 0) throw new ArgumentException("length must be zero or greater", nameof(length));
            var builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string GetGuidString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
