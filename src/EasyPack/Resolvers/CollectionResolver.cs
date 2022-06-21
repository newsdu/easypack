using AppAsToy.EasyPack.Serializers;
using System.Collections.Generic;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class CollectionResolver : SharedResolver<CollectionResolver>
    {
        public override ISerializer<T>? FindSerializer<T>()
        {
            var type = typeof(T);
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<,>))
                    return typeof(DictionarySerializer<,>).GetSharedSerializer<T>(type.GetGenericArguments());

                if (genericTypeDefinition == typeof(List<>))
                    return typeof(ListSerializer<>).GetSharedSerializer<T>(type.GetGenericArguments());
            }
            else if (type.IsArray)
            {
                return typeof(ArraySerializer<>).GetSharedSerializer<T>(type.GetElementType());
            }
            return null;
        }
    }
}
