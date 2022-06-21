using System;

namespace AppAsToy.EasyPack
{
    public interface IResolver
    {
        ISerializer<T>? FindSerializer<T>();
    }
}
