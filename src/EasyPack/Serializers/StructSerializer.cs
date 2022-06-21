using System;

namespace AppAsToy.EasyPack.Serializers
{
    internal sealed class StructSerializer<T> : SharedSerializer<StructSerializer<T>, T>
    {
        readonly SerializeDelegate<T> serializeDelegate;
        readonly DeserializeDelegate<T> deserializeDelegate;

        public StructSerializer()
        {
            if (!typeof(T).IsValueType)
                throw new InvalidOperationException($"{typeof(T).Name} is not a struct.");

            serializeDelegate = SerializerMaker.MakeSerializeDelegate<T>();
            deserializeDelegate = SerializerMaker.MakeDeserializeDelegate<T>(true);
        }

        public override void Deserialize(ref BufferReader reader, out T value)
        {
            deserializeDelegate.Invoke(ref reader, out value);
        }

        public override void Serialize(ref BufferWriter writer, T value)
        {
            serializeDelegate.Invoke(ref writer, value);
        }
    }
}
