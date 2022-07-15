using AppAsToy.EasyPack.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppAsToy.EasyPack
{
    public static class Resolver
    {
        static readonly MethodInfo findSerializerTMethod;
        static readonly IResolver[] resolvers = new IResolver[]
        {
            new BuiltinResolver(),
            new EnumResolver(),
            new NullableResolver(),
            new CollectionResolver(),
            new TupleResolver(),
            new ClassResolver(),
            new StructResolver(),
            new AbstractResolver(),
        };

        static readonly Dictionary<Type, ISerializer> serializerCache = new Dictionary<Type, ISerializer>();

        static Resolver()
        {
            findSerializerTMethod = typeof(Resolver).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(method => method.IsGenericMethod && method.Name == nameof(FindSerializer));
        }

        public static ISerializer<T> FindSerializer<T>()
        {
            if (!serializerCache.TryGetValue(typeof(T), out var serializer))
            {
                foreach (var resolver in resolvers)
                {
                    serializer = resolver.FindSerializer<T>();
                    if (serializer != null)
                        break;
                }
                if (serializer is ISerializer<T>)
                    serializerCache.Add(typeof(T), serializer);
                else
                    throw new InvalidOperationException($"{typeof(T).Name} type cannot serialize.");
            }
            return (ISerializer<T>)serializer;
        }

        public static ISerializer FindSerializer(Type type)
        {
            if (!serializerCache.TryGetValue(type, out var serializer))
                serializer = (ISerializer)findSerializerTMethod.MakeGenericMethod(type).Invoke(null, Array.Empty<object>());
            return serializer;
        }
    }

    public abstract class SharedResolver<TResolver> : IResolver
        where TResolver : SharedResolver<TResolver>, new()
    {
        public static readonly TResolver Shared = new TResolver();

        public abstract ISerializer<T>? FindSerializer<T>();
    }
}
