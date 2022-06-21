using System;
using System.Linq.Expressions;

namespace AppAsToy.EasyPack.Serializers
{
    internal class EnumSerializer<T> : SharedSerializer<EnumSerializer<T>, T>
    {
        delegate void SerializeDelegate(ref BufferWriter writer, T value);
        delegate void DeserializeDelegate(ref BufferReader reader, out T value);

        readonly SerializeDelegate serializeDelegate;
        readonly DeserializeDelegate deserializeDelegate;

        public EnumSerializer()
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException($"{typeof(T).Name} is not an enum type.");

            var enumUnderlyingType = typeof(T).GetEnumUnderlyingType();
            var enumUnderlyingTypeSerializer = Resolver.FindSerializer(enumUnderlyingType);

            serializeDelegate = CreateSerializeDelegate(enumUnderlyingType, enumUnderlyingTypeSerializer);
            deserializeDelegate = CreateDeserializeDelegate(enumUnderlyingType, enumUnderlyingTypeSerializer);
        }

        SerializeDelegate CreateSerializeDelegate(Type enumUnderlyingType, ISerializer enumUnderlyingSerializer)
        {
            var writerParam = Expression.Parameter(typeof(BufferWriter).MakeByRefType());
            var valueParam = Expression.Parameter(typeof(T));
            var convertedValue = Expression.Convert(valueParam, enumUnderlyingType);
            var serializeMethod = Expression.Call(Expression.Constant(enumUnderlyingSerializer),
                            enumUnderlyingSerializer.GetSerializeMethod(),
                            writerParam, convertedValue);
            return Expression.Lambda<SerializeDelegate>(serializeMethod, writerParam, valueParam).Compile();
        }

        DeserializeDelegate CreateDeserializeDelegate(Type enumUnderlyingType, ISerializer enumUnderlyingSerializer)
        {
            var readerParam = Expression.Parameter(typeof(BufferReader).MakeByRefType());
            var valueParam = Expression.Parameter(typeof(T).MakeByRefType());
            var tempValue = Expression.Variable(enumUnderlyingType);
            var deserializeMethod = Expression.Call(Expression.Constant(enumUnderlyingSerializer),
                            enumUnderlyingSerializer.GetDeserializeMethod(),
                            readerParam, tempValue);
            var convertValue = Expression.Convert(tempValue, typeof(T));
            var assignOutValue = Expression.Assign(valueParam, convertValue);
            return Expression.Lambda<DeserializeDelegate>(Expression.Block
            (
                new[] { tempValue },
                deserializeMethod, 
                assignOutValue
            ), readerParam, valueParam).Compile();
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
