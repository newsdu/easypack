using AppAsToy.EasyPack.Serializers;
using System;
using System.Reflection;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class AbstractResolver : SharedResolver<AbstractResolver>
    {
        public override ISerializer<T>? FindSerializer<T>()
        {
            try
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
            catch (TypeInitializationException ex)
            {
                try
                {
                    throw ex.InnerException;
                }
                catch (TargetInvocationException ex2)
                {
                    throw ex2.InnerException;
                }
            }
        }
    }
}
