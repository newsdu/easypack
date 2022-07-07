using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AppAsToy.EasyPack.Serializers
{
    delegate void SerializeDelegate<T>(ref BufferWriter writer, T value);
    delegate void DeserializeDelegate<T>(ref BufferReader reader, out T value);

    internal static class SerializerMaker
    {
        public static SerializeDelegate<T> MakeSerializeDelegate<T>() => MakeSerializeDelegate<T>(typeof(T));

        public static SerializeDelegate<T> MakeSerializeDelegate<T>(Type realType)
        {
            var writerParam = Expression.Parameter(typeof(BufferWriter).MakeByRefType());
            var valueParam = Expression.Parameter(typeof(T));
            var currentValue = typeof(T) == realType ? (Expression)valueParam : Expression.Convert(valueParam, realType);
            var block = Expression.Block(EnumerateSerializeExpression(realType, writerParam, currentValue, realType.EnumerateWithBaseTypesReverse()));
            return Expression.Lambda<SerializeDelegate<T>>(block, writerParam, valueParam).Compile();
        }

        public static DeserializeDelegate<T> MakeDeserializeDelegate<T>(bool skipCreateInstance = false) => MakeDeserializeDelegate<T>(typeof(T), skipCreateInstance);

        public static DeserializeDelegate<T> MakeDeserializeDelegate<T>(Type realType, bool skipCreateInstance = false)
        {
            var readerParam = Expression.Parameter(typeof(BufferReader).MakeByRefType());
            var valueParam = Expression.Parameter(typeof(T).MakeByRefType());
            var block = CreateDeserializeExpressionBlock(realType, readerParam, valueParam, skipCreateInstance, typeof(T) != realType);
            return Expression.Lambda<DeserializeDelegate<T>>(block, readerParam, valueParam).Compile();

        }

        static IEnumerable<Expression> EnumerateSerializeExpression(Type realType, ParameterExpression writer, Expression? value, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                foreach (var field in GetSerializableFields(type))
                {
                    var memberExpression = Expression.Field(value, field);
                    var serializer = Resolver.FindSerializer(field.FieldType);
                    yield return Expression.Call(Expression.Constant(serializer), serializer.GetSerializeMethod(), writer, memberExpression);
                }

                foreach (var property in GetSerializableProperties(realType, type))
                {
                    var memberExpression = Expression.Property(value, property);
                    var serializer = Resolver.FindSerializer(property.PropertyType);
                    yield return Expression.Call(Expression.Constant(serializer), serializer.GetSerializeMethod(), writer, memberExpression);
                }
            }
        }

        static IEnumerable<FieldInfo> GetSerializableFields(Type type)
        {
            var flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            return type.GetFields(flag)
                .Where(field => !field.IsLiteral &&
                                !typeof(Delegate).IsAssignableFrom(field.FieldType) &&
                                !field.HasAttribute<NonSerializedAttribute>() &&
                                !field.HasAttribute<IgnorePackAttribute>() &&
                                (field.IsPublic || field.HasAttribute<LetsPackAttribute>()));
        }

        static IEnumerable<PropertyInfo> GetSerializableProperties(Type realType, Type type)
        {
            var flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            return type.GetProperties(flag)
                .Where(property => property.CanRead &&
                                   !typeof(Delegate).IsAssignableFrom(property.PropertyType) &&
                                   !property.HasAttribute<NonSerializedAttribute>() &&
                                   !property.HasAttribute<IgnorePackAttribute>() &&
                                   (property.GetMethod.IsPublic || property.HasAttribute<LetsPackAttribute>()) &&
                                   (!property.GetMethod.IsVirtual || property.GetMethod.IsFinal || type == realType.GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.DeclaringType));
        }

        

        static BlockExpression CreateDeserializeExpressionBlock(Type type, ParameterExpression reader, ParameterExpression value, bool skipCreateInstance, bool useTempVariable)
        {
            var constructor = FindPackConstructor(type);
            if (constructor != null)
            {
                var parameters = constructor.GetParameters();
                var paramVars = parameters.Select(p => Expression.Variable(p.ParameterType)).ToArray();
                return Expression.Block(paramVars, EnumerateDeserializeExpressionsFromConstructor(reader, value, parameters, paramVars, constructor));
            }
            else if (useTempVariable)
            {
                var tempVariable = Expression.Variable(type);
                return Expression.Block(Enumerable.Repeat(tempVariable, 1),
                    EnumerateDeserializeExpressions(type, reader, value, skipCreateInstance, tempVariable));
            }
            else
            {
                return Expression.Block(EnumerateDeserializeExpressions(type, reader, value, skipCreateInstance));
            }
        }

        static IEnumerable<Expression> EnumerateDeserializeExpressionsFromConstructor(ParameterExpression reader, 
                                                                                      ParameterExpression value, 
                                                                                      ParameterInfo[] parameters, 
                                                                                      ParameterExpression[] paramVars, 
                                                                                      ConstructorInfo? constructor)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                var paramVar = paramVars[i];
                var serializer = Resolver.FindSerializer(param.ParameterType);
                yield return Expression.Call(Expression.Constant(serializer), serializer.GetDeserializeMethod(), reader, paramVar);
            }
            yield return Expression.Assign(value, Expression.New(constructor, paramVars));
        }

        static IEnumerable<Expression> EnumerateDeserializeExpressions(Type type, ParameterExpression reader, ParameterExpression value, bool skipCreateInstance, ParameterExpression? tempVariable = null)
        {
            var currentValue = tempVariable ?? value;
            
            if (!skipCreateInstance || tempVariable != null)
                yield return Expression.Assign(currentValue, Expression.New(type));

            foreach (var typeItem in type.EnumerateWithBaseTypesReverse())
            {
                foreach (var field in GetDeserializableFields(typeItem))
                {
                    var memberExpression = Expression.Field(currentValue, field);
                    var serializer = Resolver.FindSerializer(field.FieldType);
                    yield return Expression.Call(Expression.Constant(serializer), serializer.GetDeserializeMethod(), reader, memberExpression);
                }

                foreach (var property in GetDeserializableProperties(type, typeItem))
                {
                    var memberExpression = Expression.Property(currentValue, property);
                    var serializer = Resolver.FindSerializer(property.PropertyType);
                    yield return Expression.Call(Expression.Constant(serializer), serializer.GetDeserializeMethod(), reader, memberExpression);
                }
            }

            if (tempVariable != null)
            {
                if (type.IsValueType)
                {
                    var convertedValue = Expression.Convert(currentValue, value.Type);
                    yield return Expression.Assign(value, convertedValue);
                }
                else
                {
                    yield return Expression.Assign(value, currentValue);
                }
            }
        }

        static ConstructorInfo? FindPackConstructor(Type type)
        {
            var constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .FirstOrDefault(c => c.GetCustomAttribute<PackConstructorAttribute>() != null);
            if (constructor == null)
                return null;
            if (IsValidConstructor(type, constructor))
                return constructor;
            throw new InvalidOperationException("Constructor has PackConstructorAttribute should have parameters same as serializable fields and properties");
        }

        static bool IsValidConstructor(Type type, ConstructorInfo constructor)
        {
            var serializableMemberTypes = EnumerateSerializeMemberTypes(type);
            var constructorParameters = constructor.GetParameters();
            return serializableMemberTypes.SequenceEqual(constructorParameters.Select(p => p.ParameterType));

            static IEnumerable<Type> EnumerateSerializeMemberTypes(Type type)
            {
                foreach (var typeItem in type.EnumerateWithBaseTypesReverse())
                {
                    foreach (var field in GetSerializableFields(typeItem))
                        yield return field.FieldType;

                    foreach (var property in GetSerializableProperties(type, typeItem))
                        yield return property.PropertyType;
                }
            }
        }

        static IEnumerable<FieldInfo> GetDeserializableFields(Type type)
        {
            var fields = GetSerializableFields(type);
            foreach (var field in fields)
            {
                if (field.IsInitOnly)
                    throw new InvalidOperationException($"{field.Name} field is readonly. It can not deserialize.");
            }
            return fields;
        }

        static IEnumerable<PropertyInfo> GetDeserializableProperties(Type realType, Type type)
        {
            var properties = GetSerializableProperties(realType, type);
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                    throw new InvalidOperationException($"{property.Name} property can not write. It can not deserialize.");
            }
            return properties;
        }
    }
}
