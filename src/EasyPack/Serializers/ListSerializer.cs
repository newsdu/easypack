using System;
using System.Collections.Generic;

namespace AppAsToy.EasyPack.Serializers
{
    internal class ListSerializer<T> : SharedSerializer<ListSerializer<T>, List<T>?>
    {
        static readonly ISerializer<T> elementSerializer = Resolver.FindSerializer<T>();

        public override void Deserialize(ref BufferReader reader, out List<T>? value)
        {
            var length = reader.ReadLength();
            if (length.HasValue)
            {
                value = new List<T>(length.Value);
                for (int i = 0; i < length.Value; i++)
                {
                    elementSerializer.Deserialize(ref reader, out T element);
                    value.Add(element);
                }
            }
            else
            {
                value = null;
            }
        }

        public override void Serialize(ref BufferWriter writer, List<T>? value)
        {
            if (value != null)
            {
                writer.WriteLength(value.Count);
                for (int i = 0; i < value.Count; i++)
                    elementSerializer.Serialize(ref writer, value[i]);
            }
            else
            {
                writer.WriteLength(null);
            }
        }
    }

    internal sealed class BooleanListSerializer : ListSerializer<bool> { }
    internal sealed class CharListSerializer : ListSerializer<char> { }
    internal sealed class SByteListSerializer : ListSerializer<sbyte> { }
    internal sealed class Int16ListSerializer : ListSerializer<short> { }
    internal sealed class Int32ListSerializer : ListSerializer<int> { }
    internal sealed class Int64ListSerializer : ListSerializer<long> { }
    internal sealed class ByteListSerializer : ListSerializer<byte> { }
    internal sealed class UInt16ListSerializer : ListSerializer<ushort> { }
    internal sealed class UInt32ListSerializer : ListSerializer<uint> { }
    internal sealed class UInt64ListSerializer : ListSerializer<ulong> { }
    internal sealed class SingleListSerializer : ListSerializer<float> { }
    internal sealed class DoubleListSerializer : ListSerializer<double> { }
    internal sealed class DecimalListSerializer : ListSerializer<decimal> { }
    internal sealed class StringListSerializer : ListSerializer<string> { }
    internal sealed class DateTimeListSerializer : ListSerializer<DateTime> { }
    internal sealed class DateTimeOffsetListSerializer : ListSerializer<DateTimeOffset> { }
    internal sealed class TimeSpanListSerializer : ListSerializer<TimeSpan> { }
    internal sealed class GuidListSerializer : ListSerializer<Guid> { }

    internal sealed class NullableBooleanListSerializer : ListSerializer<bool?> { }
    internal sealed class NullableCharListSerializer : ListSerializer<char?> { }
    internal sealed class NullableSByteListSerializer : ListSerializer<sbyte?> { }
    internal sealed class NullableInt16ListSerializer : ListSerializer<short?> { }
    internal sealed class NullableInt32ListSerializer : ListSerializer<int?> { }
    internal sealed class NullableInt64ListSerializer : ListSerializer<long?> { }
    internal sealed class NullableByteListSerializer : ListSerializer<byte?> { }
    internal sealed class NullableUInt16ListSerializer : ListSerializer<ushort?> { }
    internal sealed class NullableUInt32ListSerializer : ListSerializer<uint?> { }
    internal sealed class NullableUInt64ListSerializer : ListSerializer<ulong?> { }
    internal sealed class NullableSingleListSerializer : ListSerializer<float?> { }
    internal sealed class NullableDoubleListSerializer : ListSerializer<double?> { }
    internal sealed class NullableDecimalListSerializer : ListSerializer<decimal?> { }
    internal sealed class NullableDateTimeListSerializer : ListSerializer<DateTime?> { }
    internal sealed class NullableDateTimeOffsetListSerializer : ListSerializer<DateTimeOffset?> { }
    internal sealed class NullableTimeSpanListSerializer : ListSerializer<TimeSpan?> { }
    internal sealed class NullableGuidListSerializer : ListSerializer<Guid?> { }
}
