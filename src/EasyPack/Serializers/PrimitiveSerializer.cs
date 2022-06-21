using System;

namespace AppAsToy.EasyPack.Serializers
{
    internal class PrimitiveSerializer<T> : SharedSerializer<PrimitiveSerializer<T>, T>
        where T : unmanaged
    {
        public override void Deserialize(ref BufferReader reader, out T value) => value = reader.Read<T>();
        public override void Serialize(ref BufferWriter writer, T value) => writer.Write(value);
    }

    internal sealed class BooleanSerializer : PrimitiveSerializer<bool> { }
    internal sealed class CharSerializer : PrimitiveSerializer<char> { }
    internal sealed class SByteSerializer : PrimitiveSerializer<sbyte> { }
    internal sealed class Int16Serializer : PrimitiveSerializer<short> { }
    internal sealed class Int32Serializer : PrimitiveSerializer<int> { }
    internal sealed class Int64Serializer : PrimitiveSerializer<long> { }
    internal sealed class ByteSerializer : PrimitiveSerializer<byte> { }
    internal sealed class UInt16Serializer : PrimitiveSerializer<ushort> { }
    internal sealed class UInt32Serializer : PrimitiveSerializer<uint> { }
    internal sealed class UInt64Serializer : PrimitiveSerializer<ulong> { }
    internal sealed class SingleSerializer : PrimitiveSerializer<float> { }
    internal sealed class DoubleSerializer : PrimitiveSerializer<double> { }
    internal sealed class DecimalSerializer : PrimitiveSerializer<decimal> { }

    internal sealed class StringSerializer : SharedSerializer<StringSerializer, string?>
    {
        public override void Deserialize(ref BufferReader reader, out string? value) => value = reader.ReadString();
        public override void Serialize(ref BufferWriter writer, string? value) => writer.WriteString(value);
    }

    internal sealed class DateTimeSerializer : SharedSerializer<DateTimeSerializer, DateTime>
    {
        public override void Deserialize(ref BufferReader reader, out DateTime value) => value = new DateTime(reader.ReadInt64());
        public override void Serialize(ref BufferWriter writer, DateTime value) => writer.WriteInt64(value.Ticks);
    }

    internal sealed class DateTimeOffsetSerializer : SharedSerializer<DateTimeOffsetSerializer, DateTimeOffset>
    {
        public override void Deserialize(ref BufferReader reader, out DateTimeOffset value) => value = new DateTimeOffset(reader.ReadInt64(), new TimeSpan(reader.ReadInt64()));
        public override void Serialize(ref BufferWriter writer, DateTimeOffset value)
        {
            writer.WriteInt64(value.Ticks);
            writer.WriteInt64(value.Offset.Ticks);
        }
    }

    internal sealed class TimeSpanSerializer : SharedSerializer<TimeSpanSerializer, TimeSpan>
    {
        public override void Deserialize(ref BufferReader reader, out TimeSpan value) => value = new TimeSpan(reader.ReadInt64());
        public override void Serialize(ref BufferWriter writer, TimeSpan value) => writer.WriteInt64(value.Ticks);
    }

    internal sealed class GuidSerializer : SharedSerializer<GuidSerializer, Guid>
    {
        public override void Deserialize(ref BufferReader reader, out Guid value)
        {
            value = new Guid(reader.GetSpan()[..16]);
            reader.Advance(16);
        }
        public override void Serialize(ref BufferWriter writer, Guid value)
        {
            value.TryWriteBytes(writer.GetSpan());
            writer.Advance(16);
        }
    }
}
