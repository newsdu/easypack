using System;

namespace AppAsToy.EasyPack
{
    public ref struct BufferReader
    {
        readonly ReadOnlySpan<byte> data;

        public int Capacity => data.Length;
        public int RemainSize => Capacity - ReadSize;
        public int ReadSize { get; private set; }

        public bool IsValid => !data.IsEmpty;

        public BufferReader(ReadOnlySpan<byte> data)
        {
            this.data = data;
            ReadSize = 0;
        }

        public void Advance(int count) => ReadSize += count;
        public ReadOnlySpan<byte> GetSpan() => data[ReadSize..];
        public void ReadNull() => Read<byte>();
        public bool ReadBoolean() => Read<byte>() != 0;
        public byte ReadByte()
        {
            ThrowIfCannotRead(1);
            return data[ReadSize++];
        }
        public sbyte   ReadSByte() => (sbyte)ReadByte();
        public ushort  ReadUInt16() => Read<ushort>();
        public short   ReadInt16() => Read<short>();
        public char    ReadChar() => Read<char>();
        public uint    ReadUInt32() => Read<uint>();
        public uint    ReadUInt32(int bytesCount) => Read<uint>(bytesCount);
        public int     ReadInt32() => Read<int>();
        public int     ReadInt32(int bytesCount) => Read<int>(bytesCount);
        public ulong   ReadUInt64() => Read<ulong>();
        public ulong   ReadUInt64(int bytesCount) => Read<ulong>(bytesCount);
        public long    ReadInt64() => Read<long>();
        public long    ReadInt64(int bytesCount) => Read<long>(bytesCount);
        public float   ReadSingle() => Read<float>();
        public double  ReadDouble() => Read<double>();
        public decimal ReadDecimal() => Read<decimal>();
        public string? ReadString()
        {
            var length = ReadLength();
            if (length == null)
                return null;

            if (length == 0)
                return string.Empty;

            var value = new string(UTF8EncodingCache.Encoding.GetString(GetSpan()[..length.Value]));
            Advance(length.Value);
            return value;
        }

        public int? ReadLength() => LengthHelper.Read(ref this);

        public unsafe T Read<T>() where T : unmanaged
        {
            T value;
            var valueSpan = new Span<byte>(&value, sizeof(T));
            GetSpan()[..sizeof(T)].CopyTo(valueSpan);
            if (BitConverter.IsLittleEndian)
                valueSpan.Reverse();
            Advance(sizeof(T));
            return value;
        }

        public unsafe T Read<T>(int bytesCount) where T : unmanaged
        {
            ThrowIfOutOfRange<T>(bytesCount);
            T value;
            var bufferSpan = GetSpan()[..bytesCount];
            if (BitConverter.IsLittleEndian)
            {
                var valueSpan = new Span<byte>(&value, bytesCount);
                bufferSpan.CopyTo(valueSpan);
                valueSpan.Reverse();
            }
            else
            {
                var valueSpan = new Span<byte>((byte*)&value + (sizeof(T) - bytesCount), bytesCount);
                bufferSpan.CopyTo(valueSpan);
            }
            return value;
        }

        static unsafe void ThrowIfOutOfRange<T>(int bytesCount) where T : unmanaged
        {
            if (bytesCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(bytesCount), $"bytesCount({bytesCount}) should be more than zero.");
            if (bytesCount > sizeof(T))
                throw new ArgumentOutOfRangeException(nameof(bytesCount), $"bytesCount({bytesCount}) should be less or eqaul to size({sizeof(T)}).");
        }

        private void ThrowIfCannotRead(int size)
        {
            if (size > RemainSize)
                throw new OverflowException($"RemainSize({RemainSize}) is less than size({size}).");
        }
    }
}
