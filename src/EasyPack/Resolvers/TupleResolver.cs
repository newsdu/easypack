using AppAsToy.EasyPack.Serializers;
using System.Runtime.CompilerServices;

namespace AppAsToy.EasyPack.Resolvers
{
    internal sealed class TupleResolver : SharedResolver<TupleResolver>
    {
        public override ISerializer<T>? FindSerializer<T>()
            => typeof(ITuple).IsAssignableFrom(typeof(T)) ? 
            typeof(TupleSerializer<>).GetSharedSerializer<T>(typeof(T)): null;
    }
}
