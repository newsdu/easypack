using AppAsToy.EasyPack.Serializers;
using System;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class NullableResolver : SharedResolver<NullableResolver>
    {
        public override ISerializer<T>? FindSerializer<T>()
            => typeof(T).IsValueType && typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>) ?
                typeof(NullableSerializer<>).GetSharedSerializer<T>(Nullable.GetUnderlyingType(typeof(T))) : null;
    }
}
