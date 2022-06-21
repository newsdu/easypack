using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppAsToy.EasyPack
{
    internal static class ReflectionExtensions
    {
        public static IEnumerable<Type> EnumerateWithBaseTypes(this Type type, bool includeObjectType = false)
        {
            var currentType = type;
            while (currentType != null && (currentType != typeof(object) || includeObjectType))
            {
                yield return currentType;
                currentType = currentType.BaseType;
            }
        }

        public static IEnumerable<Type> EnumerateWithBaseTypesReverse(this Type type, bool includeObjectType = false)
        {
            return type.EnumerateWithBaseTypes(includeObjectType).Reverse();
        }

        public static bool HasAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return type.GetCustomAttribute<TAttribute>() != null;
        }

        public static bool HasAttribute<TAttribute>(this MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            return memberInfo.GetCustomAttribute<TAttribute>() != null;
        }

        public static ISerializer<T> GetSharedSerializer<T>(this Type type, params Type[] typeArguments)
        {
            if (!type.IsGenericTypeDefinition)
                throw new InvalidOperationException();

            return (ISerializer<T>)type.MakeGenericType(typeArguments)
                .GetField("Shared", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .GetValue(null);
        }

        public static string GetFullNameForSerialization(this Type type)
        {
            var fullShortName = $"{(string.IsNullOrEmpty(type.Namespace) ? string.Empty : type.Namespace + ".")}{type.Name}";
            var assemblyName = type.Assembly.GetName().Name;
            if (type.IsGenericType)
            {
                if (type.IsGenericTypeDefinition)
                    return GetFullNameForSerialization(type.GetGenericTypeDefinition());

                var arguments = type.GetGenericArguments();
                var argumentsJoined = string.Join(',', arguments.Select(a => $"[{GetFullNameForSerialization(a)}]"));
                return $"{fullShortName}[{argumentsJoined}],{assemblyName}";
            }
            else
            {
                return $"{fullShortName},{assemblyName}";
            }
        }
    }
}
