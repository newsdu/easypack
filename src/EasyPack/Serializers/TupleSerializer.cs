using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AppAsToy.EasyPack.Serializers
{
    internal class TupleSerializer<T> : SharedSerializer<TupleSerializer<T>, T>
        where T : ITuple
    {
        delegate void SerializeDelegate(ref BufferWriter writer, T value);
        delegate void DeserializeDelegate(ref BufferReader reader, out T value);

        static readonly SerializeDelegate serializeDelegate;
        static readonly DeserializeDelegate deserializeDelegate;

        static TupleSerializer()
        {
            var itemSerializers = MakeItemSerializers();
            serializeDelegate = MakeSerializeDelegate(itemSerializers);
            deserializeDelegate = MakeDeserializeDelegate(itemSerializers);
        }

        public override void Deserialize(ref BufferReader reader, out T value)
        {
            deserializeDelegate.Invoke(ref reader, out value);
        }

        public override void Serialize(ref BufferWriter writer, T value)
        {
            serializeDelegate.Invoke(ref writer, value);
        }

        static ISerializer[] MakeItemSerializers()
        {
            return typeof(T).GetGenericArguments()
                .Select(Resolver.FindSerializer)
                .ToArray();
        }

        static SerializeDelegate MakeSerializeDelegate(ISerializer[] itemSerializers)
        {
            var writerParam = Expression.Parameter(typeof(BufferWriter).MakeByRefType());
            var valueParam = Expression.Parameter(typeof(T));
            var items = typeof(T).GetMembers().Where(m =>
                m.MemberType == MemberTypes.Property ||
                m.MemberType == MemberTypes.Field);
            var serializeFields = items.Select((item, i) =>
            {
                var serializer = itemSerializers[i];
                return Expression.Call(
                    Expression.Constant(serializer), 
                    serializer.GetSerializeMethod(), 
                    writerParam, 
                    Expression.MakeMemberAccess(valueParam, item));
            });
            return Expression.Lambda<SerializeDelegate>(Expression.Block(serializeFields), writerParam, valueParam).Compile();
        }

        static DeserializeDelegate MakeDeserializeDelegate(ISerializer[] itemSerializers)
        {
            if (typeof(T).IsValueType)
                return MakeDeserializeDelegateForValueTuple(typeof(T).GetFields(), itemSerializers);
            else
                return MakeDeserializeDelegateForTuple(typeof(T).GetProperties(), itemSerializers);
        }

        static DeserializeDelegate MakeDeserializeDelegateForTuple(PropertyInfo[] properties, ISerializer[] itemSerializers)
        {
            var readerParam = Expression.Parameter(typeof(BufferReader).MakeByRefType());
            var valueParam = Expression.Parameter(typeof(T).MakeByRefType());

            var localVariables = properties.Select(f => Expression.Variable(f.PropertyType)).ToArray();
            IEnumerable<Expression> serializeFields = properties.Select((f, i) =>
            {
                var serializer = itemSerializers[i];
                return Expression.Call(Expression.Constant(serializer),
                    serializer.GetDeserializeMethod(),
                    readerParam,
                    localVariables[i]);
            });
            return Expression.Lambda<DeserializeDelegate>
                (
                    Expression.Block(localVariables, 
                        serializeFields.Append(
                            Expression.Assign(valueParam, 
                                Expression.New(typeof(T).GetConstructors().First(), localVariables)))), 
                    readerParam, 
                    valueParam
                )
                .Compile();
        }

        static DeserializeDelegate MakeDeserializeDelegateForValueTuple(FieldInfo[] fields, ISerializer[] itemSerializers)
        {
            var readerParam = Expression.Parameter(typeof(BufferReader).MakeByRefType());
            var valueParam = Expression.Parameter(typeof(T).MakeByRefType());

            var serializeFields = fields.Select((f, i) =>
            {
                var serializer = itemSerializers[i];
                return Expression.Call(Expression.Constant(serializer),
                    serializer.GetDeserializeMethod(),
                    readerParam,
                    Expression.Field(valueParam, f));
            });
            return Expression.Lambda<DeserializeDelegate>(Expression.Block(serializeFields), readerParam, valueParam).Compile();
        }
    }
}
