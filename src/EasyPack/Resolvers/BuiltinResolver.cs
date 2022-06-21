using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class BuiltinResolver : SharedResolver<BuiltinResolver>
    {
        readonly Dictionary<Type, ISerializer> builtinSerializerMap;

        public override ISerializer<T>? FindSerializer<T>()
        {
            if (builtinSerializerMap.TryGetValue(typeof(T), out var serializer) &&
                serializer is ISerializer<T> typeSerializer)
                return typeSerializer;

            return null;
        }

        public BuiltinResolver()
        {
            builtinSerializerMap = 
                typeof(ISerializer).Assembly
                    .GetTypes()
                    .Where(IsBuiltinSerializer)
                    .ToDictionary(GetSerializationTargetType, GetSharedSerializer);
        }

        static bool IsBuiltinSerializer(Type type)
        {
            if (!type.IsSealed || type.IsGenericType)
                return false;

            return type.BaseType
                .EnumerateWithBaseTypes()
                .Any(type => type.IsGenericType &&
                             !type.IsGenericTypeDefinition &&
                             type.GetGenericTypeDefinition() == typeof(SharedSerializer<,>));
        }

        static Type GetSerializationTargetType(Type builtinSerializerType)
        {
            return builtinSerializerType.BaseType
                .EnumerateWithBaseTypes()
                .First(type => type.IsGenericType &&
                               !type.IsGenericTypeDefinition &&
                               type.GetGenericTypeDefinition() == typeof(SharedSerializer<,>))
                .GetGenericArguments()[1];
        }

        static ISerializer GetSharedSerializer(Type builtinSerializerType)
        {
            return (ISerializer)builtinSerializerType
                .GetField("Shared", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .GetValue(null);
        }
    }
}
