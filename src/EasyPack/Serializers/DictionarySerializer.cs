using System.Collections.Generic;

namespace AppAsToy.EasyPack.Serializers
{
    internal class DictionarySerializer<TKey, TValue> : SharedSerializer<DictionarySerializer<TKey, TValue>, Dictionary<TKey, TValue>?>
    {
        static readonly ISerializer<TKey> keySerializer = Resolver.FindSerializer<TKey>();
        static readonly ISerializer<TValue> valueSerializer = Resolver.FindSerializer<TValue>();

        public override void Deserialize(ref BufferReader reader, out Dictionary<TKey, TValue>? value)
        {
            var length = reader.ReadLength();
            if (length.HasValue)
            {
                value = new Dictionary<TKey, TValue>(length.Value);
                for (int i = 0; i < length.Value; i++)
                {
                    keySerializer.Deserialize(ref reader, out TKey key);
                    valueSerializer.Deserialize(ref reader, out TValue keyValue);
                    value.Add(key, keyValue);
                }
            }
            else
            {
                value = null;
            }
        }

        public override void Serialize(ref BufferWriter writer, Dictionary<TKey, TValue>? value)
        {
            if (value != null)
            {
                writer.WriteLength(value.Count);
                foreach (var item in value)
                {
                    keySerializer.Serialize(ref writer, item.Key);
                    valueSerializer.Serialize(ref writer, item.Value);
                }
            }
            else
            {
                writer.WriteLength(null);
            }
        }
    }
}
