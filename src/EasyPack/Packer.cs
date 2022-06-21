using System;

namespace AppAsToy.EasyPack
{
    public static class Packer
    {
        public static Pack Pack<T>(T value)
        {
            var writer = BufferWriter.Rent();
            var serializer = Resolver.FindSerializer<T>();
            serializer.Serialize(ref writer, value);
            return new Pack(writer);
        }

        public static T Unpack<T>(ReadOnlySpan<byte> bytes)
        {
            var reader = new BufferReader(bytes);
            var serializer = Resolver.FindSerializer<T>();
            serializer.Deserialize(ref reader, out var value);
            if (reader.RemainSize > 0)
                throw new InvalidOperationException($"Failed to unpack to \"{typeof(T).Name}\".");
            return value;
        }
    }
}
