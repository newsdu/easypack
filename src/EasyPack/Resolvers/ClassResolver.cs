using AppAsToy.EasyPack.Serializers;
using System;
using System.Reflection;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class ClassResolver : SharedResolver<ClassResolver>
    {
        public override ISerializer<T>? FindSerializer<T>()
        {
            try
            {
                var type = typeof(T);
                return type.IsClass &&
                    !type.IsAbstract &&
                    (type.IsSealed || !typeof(T).HasAttribute<PolymophicPackAttribute>()) ?
                    ClassSerializer<T>.Shared : null;
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
