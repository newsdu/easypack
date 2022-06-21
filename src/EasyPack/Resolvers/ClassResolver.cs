using AppAsToy.EasyPack.Serializers;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class ClassResolver : SharedResolver<ClassResolver>
    {
        public override ISerializer<T>? FindSerializer<T>()
        {
            var type = typeof(T);
            return type.IsClass && 
                !type.IsAbstract && 
                (type.IsSealed || !typeof(T).HasAttribute<PolymophicPackAttribute>()) ?
                ClassSerializer<T>.Shared : null;
        }
    }
}
