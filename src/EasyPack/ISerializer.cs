using System;
using System.Reflection;

namespace AppAsToy.EasyPack
{
    public interface ISerializer
    {
        void Serialize(ref BufferWriter writer, object? value);
        void Deserialize(ref BufferReader reader, out object? value);
    }

    public interface ISerializer<T> : ISerializer
    {
        void Serialize(ref BufferWriter writer, T value);
        void Deserialize(ref BufferReader reader, out T value);
    }

    internal abstract class SharedSerializer<TSerializer, TValue> : ISerializer<TValue>
        where TSerializer : ISerializer<TValue>, new()
    {
        public static readonly TSerializer Shared = new TSerializer();

        public abstract void Deserialize(ref BufferReader reader, out TValue value);
        public abstract void Serialize(ref BufferWriter writer, TValue value);

        void ISerializer.Serialize(ref BufferWriter writer, object? value)
        {
#pragma warning disable CS8604 // 가능한 null 참조 인수입니다.
            Serialize(ref writer, value is TValue typeValue ? typeValue : default);
#pragma warning restore CS8604 // 가능한 null 참조 인수입니다.
        }

        void ISerializer.Deserialize(ref BufferReader reader, out object? value)
        {
            Deserialize(ref reader, out var typeValue);
            value = typeValue;
        }
    }

    internal static class SerializerExtension
    {
        public static MethodInfo? GetSerializeMethod(this ISerializer? serializer)
        {
            return serializer?.GetType().GetMethod(nameof(ISerializer.Serialize));
        }

        public static MethodInfo? GetDeserializeMethod(this ISerializer? serializer)
        {
            return serializer?.GetType().GetMethod(nameof(ISerializer.Deserialize));
        }
    }
}