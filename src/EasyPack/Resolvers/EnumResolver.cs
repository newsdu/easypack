using AppAsToy.EasyPack.Serializers;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class EnumResolver : SharedResolver<EnumResolver>
    {
        public override ISerializer<T>? FindSerializer<T>() 
            => typeof(T).IsEnum ? EnumSerializer<T>.Shared : null;
    }
}
