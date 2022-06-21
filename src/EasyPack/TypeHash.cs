using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppAsToy.EasyPack
{
    internal static class TypeHash
    {
        static readonly Dictionary<Type, Dictionary<int, Type>> typeHashByTHintMap = new Dictionary<Type, Dictionary<int, Type>>();

        static TypeHash()
        {
            
        }

        static IEnumerable<Type> EnumerableAllTypesInAllLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes());
        }

        public static Type GetTypeFromHash<THint>(int hash)
        {
            if (!typeHashByTHintMap.TryGetValue(typeof(THint), out var typeHashMap))
            {
                typeHashMap = new Dictionary<int, Type>();
                foreach (var t in EnumerableAllTypesInAllLoadedAssemblies()
                                    .Where(t => !t.IsInterface && !t.IsAbstract && typeof(THint).IsAssignableFrom(t)))
                {
                    var typeHash = t.GetFullNameForSerialization().GetHashCode();
                    if (typeHashMap.TryGetValue(typeHash, out var prevAddedType))
                        throw new InvalidOperationException($"\"\" type and \"\" type hash(0x{typeHash:X8}) is same. You should rename one of the two types.");
                    typeHashMap.Add(typeHash, t);
                }
                typeHashByTHintMap.Add(typeof(THint), typeHashMap);
            }
            if (typeHashMap.TryGetValue(hash, out var item))
                return item;

            throw new ArgumentException($"Not found type on GetTypeFromHash(). (hash: 0x{hash:X8})", nameof(hash));
        }
    }
}
