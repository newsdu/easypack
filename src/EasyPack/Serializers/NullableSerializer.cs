using System;

namespace AppAsToy.EasyPack.Serializers
{
    internal class NullableSerializer<T> : SharedSerializer<NullableSerializer<T>, T?>
        where T : struct
    {
        static readonly ISerializer<T> valueSerializer = Resolver.FindSerializer<T>();

        public override void Deserialize(ref BufferReader reader, out T? value)
        {
            if (reader.ReadBoolean())
            {
                valueSerializer.Deserialize(ref reader, out T realValue);
                value = realValue;
            }
            else
            {
                value = null;
            }
        }

        public override void Serialize(ref BufferWriter writer, T? value)
        {
            writer.WriteBoolean(value.HasValue);
            if (value.HasValue)
                valueSerializer.Serialize(ref writer, value.Value);
        }
    }

    internal sealed class NullableBooleanSerializer : NullableSerializer<bool> { }
    internal sealed class NullableCharSerializer : NullableSerializer<char> { }
    internal sealed class NullableSByteSerializer : NullableSerializer<sbyte> { }
    internal sealed class NullableInt16Serializer : NullableSerializer<short> { }
    internal sealed class NullableInt32Serializer : NullableSerializer<int> { }
    internal sealed class NullableInt64Serializer : NullableSerializer<long> { }
    internal sealed class NullableByteSerializer : NullableSerializer<byte> { }
    internal sealed class NullableUInt16Serializer : NullableSerializer<ushort> { }
    internal sealed class NullableUInt32Serializer : NullableSerializer<uint> { }
    internal sealed class NullableUInt64Serializer : NullableSerializer<ulong> { }
    internal sealed class NullableSingleSerializer : NullableSerializer<float> { }
    internal sealed class NullableDoubleSerializer : NullableSerializer<double> { }
    internal sealed class NullableDecimalSerializer : NullableSerializer<decimal> { }
    internal sealed class NullableDateTimeSerializer : NullableSerializer<DateTime> { }
    internal sealed class NullableDateTimeOffsetSerializer : NullableSerializer<DateTimeOffset> { }
    internal sealed class NullableTimeSpanSerializer : NullableSerializer<TimeSpan> { }
    internal sealed class NullableGuidSerializer : NullableSerializer<Guid> { }
}
