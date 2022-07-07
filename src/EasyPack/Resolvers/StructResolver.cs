using AppAsToy.EasyPack.Serializers;
using System;
using System.Reflection;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class StructResolver : SharedResolver<ClassResolver>
    {
        public override ISerializer<T>? FindSerializer<T>()
        {
            try
            {
                return typeof(T).IsValueType ? StructSerializer<T>.Shared : null;
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
