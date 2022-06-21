using System;
using System.Collections.Generic;

namespace AppAsToy.EasyPack.Serializers
{
    internal sealed class ObjectSerializer<T> : SharedSerializer<ObjectSerializer<T>, T>
    {
        public override void Deserialize(ref BufferReader reader, out T value)
        {
            Type? type = null;
            var header = reader.ReadByte();
            if (header == Constants.NULL)
            {
#pragma warning disable CS8601 // 가능한 null 참조 할당입니다.
                value = default;
                return;
            }
            var useHash = header != 0;
            if (useHash)
            {
                var hash = reader.ReadInt32();
                type = TypeHash.GetTypeFromHash<T>(hash);
            }
            else
            {
                type = GetTypeFromName(ref reader);
            }
            if (type == null)
            {
                value = default;
                return;
            }

            var serializer = ObjectSerializerFactory.Acquire(type);
            if (serializer != null)
            {
                serializer.Deserialize(ref reader, out var obj);
                value = (T)obj;
            }
            else
            {
                value = default;
#pragma warning restore CS8601 // 가능한 null 참조 할당입니다.
            }
        }

        public override void Serialize(ref BufferWriter writer, T value)
        {
            if (value == null)
            {
                writer.WriteLength(null);
                return;
            }
            var valueType = value.GetType();
            if (typeof(T) == typeof(object) || valueType.IsGenericType)
            {
                writer.WriteBoolean(false);
                writer.WriteString(valueType.GetFullNameForSerialization());
            }
            else
            {
                writer.WriteBoolean(true);
                writer.WriteInt32(valueType.GetFullNameForSerialization().GetHashCode());
            }
            var serializer = ObjectSerializerFactory.Acquire(valueType);
            serializer?.Serialize(ref writer, value);
        }

        Type? GetTypeFromName(ref BufferReader reader)
        {
            var typeName = reader.ReadString();
            if (string.IsNullOrEmpty(typeName))
                return null;
            var type = Type.GetType(typeName);
            if (type == null)
                throw new InvalidOperationException($"\"{typeName}\" type can not found.");
            return type;
        }
    }

    internal static class ObjectSerializerFactory
    {
        internal sealed class ObjectSerializer : ISerializer
        {
            readonly SerializeDelegate<object?> serializeDelegate;
            readonly DeserializeDelegate<object?> deserializeDelegate;

            internal ObjectSerializer(SerializeDelegate<object?> serializeDelegate, DeserializeDelegate<object?> deserializeDelegate)
            {
                this.serializeDelegate = serializeDelegate;
                this.deserializeDelegate = deserializeDelegate;
            }

            public void Deserialize(ref BufferReader reader, out object? value) => deserializeDelegate.Invoke(ref reader, out value);
            public void Serialize(ref BufferWriter writer, object? value) => serializeDelegate.Invoke(ref writer, value);
        }

        static readonly Dictionary<Type, ISerializer> serializerCache = new Dictionary<Type, ISerializer>();

        public static ISerializer Acquire(Type type)
        {
            if (!serializerCache.TryGetValue(type, out var serializer))
            {
                serializer = Create(type);
                serializerCache.Add(type, serializer);
            }
            return serializer;
        }

        static ISerializer Create(Type type)
        {
            return new ObjectSerializer(SerializerMaker.MakeSerializeDelegate<object?>(type),
                                        SerializerMaker.MakeDeserializeDelegate<object?>(type));
        }
    }
}
