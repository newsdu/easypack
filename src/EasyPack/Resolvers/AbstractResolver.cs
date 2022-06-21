using AppAsToy.EasyPack.Serializers;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class AbstractResolver : SharedResolver<AbstractResolver>
    {
        public override ISerializer<T>? FindSerializer<T>()
        {
            var type = typeof(T);
            if (type == typeof(object) || type.IsInterface)
                return ObjectSerializer<T>.Shared;

            if (!type.IsClass || type.IsSealed)
                return null;

            if (type.IsAbstract || typeof(T).HasAttribute<PolymophicPackAttribute>())
                return ObjectSerializer<T>.Shared;

            return null;
        }
    }
}
