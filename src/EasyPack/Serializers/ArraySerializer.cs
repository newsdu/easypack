using System;

namespace AppAsToy.EasyPack.Serializers
{
    internal class ArraySerializer<T> : SharedSerializer<ArraySerializer<T>, T[]?>
    {
        static readonly ISerializer<T> elementSerializer = Resolver.FindSerializer<T>();

        public override void Deserialize(ref BufferReader reader, out T[]? value)
        {
            var length = reader.ReadLength();
            if (length.HasValue)
            {
                value = new T[length.Value];
                for (int i = 0; i < length.Value; i++)
                    elementSerializer.Deserialize(ref reader, out value[i]);
            }
            else
            {
                value = null;
            }
        }

        public override void Serialize(ref BufferWriter writer, T[]? value)
        {
            if (value != null)
            {
                writer.WriteLength(value.Length);
                for (int i = 0; i < value.Length; i++)
                    elementSerializer.Serialize(ref writer, value[i]);
            }
            else
            {
                writer.WriteLength(null);
            }
        }
    }

    internal sealed class BooleanArraySerializer : ArraySerializer<bool> { }
    internal sealed class CharArraySerializer : ArraySerializer<char> { }
    internal sealed class SByteArraySerializer : ArraySerializer<sbyte> { }
    internal sealed class Int16ArraySerializer : ArraySerializer<short> { }
    internal sealed class Int32ArraySerializer : ArraySerializer<int> { }
    internal sealed class Int64ArraySerializer : ArraySerializer<long> { }
    internal sealed class ByteArraySerializer : ArraySerializer<byte> { }
    internal sealed class UInt16ArraySerializer : ArraySerializer<ushort> { }
    internal sealed class UInt32ArraySerializer : ArraySerializer<uint> { }
    internal sealed class UInt64ArraySerializer : ArraySerializer<ulong> { }
    internal sealed class SingleArraySerializer : ArraySerializer<float> { }
    internal sealed class DoubleArraySerializer : ArraySerializer<double> { }
    internal sealed class DecimalArraySerializer : ArraySerializer<decimal> { }
    internal sealed class StringArraySerializer : ArraySerializer<string> { }
    internal sealed class DateTimeArraySerializer : ArraySerializer<DateTime> { }
    internal sealed class DateTimeOffsetArraySerializer : ArraySerializer<DateTimeOffset> { }
    internal sealed class TimeSpanArraySerializer : ArraySerializer<TimeSpan> { }
    internal sealed class GuidArraySerializer : ArraySerializer<Guid> { }

    internal sealed class NullableBooleanArraySerializer : ArraySerializer<bool?> { }
    internal sealed class NullableCharArraySerializer : ArraySerializer<char?> { }
    internal sealed class NullableSByteArraySerializer : ArraySerializer<sbyte?> { }
    internal sealed class NullableInt16ArraySerializer : ArraySerializer<short?> { }
    internal sealed class NullableInt32ArraySerializer : ArraySerializer<int?> { }
    internal sealed class NullableInt64ArraySerializer : ArraySerializer<long?> { }
    internal sealed class NullableByteArraySerializer : ArraySerializer<byte?> { }
    internal sealed class NullableUInt16ArraySerializer : ArraySerializer<ushort?> { }
    internal sealed class NullableUInt32ArraySerializer : ArraySerializer<uint?> { }
    internal sealed class NullableUInt64ArraySerializer : ArraySerializer<ulong?> { }
    internal sealed class NullableSingleArraySerializer : ArraySerializer<float?> { }
    internal sealed class NullableDoubleArraySerializer : ArraySerializer<double?> { }
    internal sealed class NullableDecimalArraySerializer : ArraySerializer<decimal?> { }
    internal sealed class NullableDateTimeArraySerializer : ArraySerializer<DateTime?> { }
    internal sealed class NullableDateTimeOffsetArraySerializer : ArraySerializer<DateTimeOffset?> { }
    internal sealed class NullableTimeSpanArraySerializer : ArraySerializer<TimeSpan?> { }
    internal sealed class NullableGuidArraySerializer : ArraySerializer<Guid?> { }
}
