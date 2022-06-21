using System;

namespace AppAsToy.EasyPack.Serializers
{
    internal sealed class ClassSerializer<T> : SharedSerializer<ClassSerializer<T>,T>
    {
        readonly SerializeDelegate<T> serializeDelegate;
        readonly DeserializeDelegate<T> deserializeDelegate;

        public ClassSerializer()
        {
            if (!typeof(T).IsClass)
                throw new InvalidOperationException($"{typeof(T).Name} is not a class.");

            serializeDelegate = SerializerMaker.MakeSerializeDelegate<T>();
            deserializeDelegate = SerializerMaker.MakeDeserializeDelegate<T>();
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
