using System;

namespace Utilities
{
    public struct ProviderOverride
    {
        public readonly Type Type;
        public readonly Func<object> Provider;

        public ProviderOverride(Type type, Func<object> provider)
        {
            Type = type;
            Provider = provider;
        }
    }
}
