using AppAsToy.EasyPack.Serializers;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class StructResolver : SharedResolver<ClassResolver>
    {
        public override ISerializer<T>? FindSerializer<T>() =>
            typeof(T).IsValueType ? StructSerializer<T>.Shared : null;
    }
}
