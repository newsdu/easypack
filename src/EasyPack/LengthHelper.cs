using System;

namespace AppAsToy.EasyPack
{
    internal static class LengthHelper
    {
        const byte META_INLINE = 0b_0000_0000;
        const byte META_EXTRA_BYTE_1 = 0b_0100_0000;
        const byte META_EXTRA_BYTE_2 = 0b_1000_0000;
        const byte META_EXTRA_BYTE_3 = 0b_1100_0000;

        const byte VALUE_MASK_INLINE = 0b_0011_1111;
        const ushort VALUE_MASK_EXTRA_BYTE_1 = 0x00ff | (VALUE_MASK_INLINE << 8);
        const uint VALUE_MASK_EXTRA_BYTE_2 = 0xffff | (VALUE_MASK_INLINE << 16);
        const uint VALUE_MASK_EXTRA_BYTE_3 = 0xffffff | (0b_0001_1111 << 24);

        public static int GetByteCount(int length)
        {
            if (length <= VALUE_MASK_INLINE)
            {
                return 1;
            }
            else if (length <= VALUE_MASK_EXTRA_BYTE_1)
            {
                return 2;
            }
            else if (length <= VALUE_MASK_EXTRA_BYTE_2)
            {
                return 3;
            }
            else if (length <= VALUE_MASK_EXTRA_BYTE_3)
            {
                return 4;
            }
            throw new ArgumentOutOfRangeException($"You can write Length less or {VALUE_MASK_EXTRA_BYTE_3:N0} in BufferWriter.");
        }

        public static void Write(ref BufferWriter writer, int? length)
        {
            if (length == null)
            {
                writer.WriteNull();
            }
            else if (length < 0)
            {
                throw new ArgumentOutOfRangeException($"You should write Length({length.Value}) equal or greater than 0 in BufferWriter.");
            }
            else if (length <= VALUE_MASK_INLINE)
            {
                writer.WriteByte((byte)(META_INLINE | length));
            }
            else if (length <= VALUE_MASK_EXTRA_BYTE_1)
            {
                writer.WriteUInt16((ushort)((META_EXTRA_BYTE_1 << 8) | (length & 0xffff)));
            }
            else if (length <= VALUE_MASK_EXTRA_BYTE_2)
            {
                writer.WriteUInt32((uint)((META_EXTRA_BYTE_2 << 24) | (length << 8)), 3);
            }
            else if (length <= VALUE_MASK_EXTRA_BYTE_3)
            {
                writer.WriteUInt32((uint)((META_EXTRA_BYTE_3 << 24) | length));
            }
            else
            {
                throw new ArgumentOutOfRangeException($"You should write Length({length.Value}) less or {VALUE_MASK_EXTRA_BYTE_3:N0} in BufferWriter.");
            }
        }

        public static int? Read(ref BufferReader reader)
        {
            var firstByte = reader.GetSpan()[0];
            if (firstByte == Constants.NULL)
            {
                reader.ReadNull();
                return null;
            }
            switch (firstByte & META_EXTRA_BYTE_3)
            {
                case META_INLINE:
                    reader.Advance(1);
                    return firstByte & VALUE_MASK_INLINE;

                case META_EXTRA_BYTE_1:
                    return reader.ReadUInt16() & VALUE_MASK_EXTRA_BYTE_1;

                case META_EXTRA_BYTE_2:
                    return (int)(reader.ReadUInt32(3) & VALUE_MASK_EXTRA_BYTE_2);

                case META_EXTRA_BYTE_3:
                    return (int)(reader.ReadUInt32() & VALUE_MASK_EXTRA_BYTE_3);

                default:
                    throw new InvalidOperationException("");
            };
        }
    }
}
