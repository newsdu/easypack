using System;
using System.Buffers;

namespace AppAsToy.EasyPack
{
    public struct BufferWriter : IDisposable
    {
        readonly IMemoryOwner<byte> owner;
        bool isDisposed;
        
        public int Capacity => owner.Memory.Length;
        public int RemainSize => Capacity - WrittenSize;
        public int WrittenSize { get; private set; }

        public bool IsValid => owner != null && !isDisposed;

        public static BufferWriter Rent(int size = 4096)
        {
            return new BufferWriter(size);
        }

        BufferWriter(int size)
        {
            owner = MemoryPool<byte>.Shared.Rent(size);
            isDisposed = false;
            WrittenSize = 0;
        }

        public void Advance(int count) => WrittenSize += count;
        public Span<byte> GetSpan() => owner.Memory.Span[WrittenSize..];
        public Memory<byte> GetWrittenMemory() => owner.Memory[..WrittenSize];
        public Span<byte> GetWrittenSpan() => owner.Memory.Span[..WrittenSize];

        public void WriteNull() => Write(Constants.NULL);
        public void WriteBoolean(bool value) => Write((byte)(value ? 1 : 0));
        public void WriteByte(byte value)
        {
            ThrowIfCannotWrite(1);
            owner.Memory.Span[WrittenSize++] = value;
        }
        public void WriteSByte(sbyte value) => Write((byte)value);
        public void WriteUInt16(ushort value) => Write<ushort>(value);
        public void WriteInt16(short value) => Write<short>(value);
        public void WriteChar(char value) => Write<char>(value);
        public void WriteUInt32(uint value) => Write<uint>(value);
        public void WriteUInt32(uint value, int bytesCount) => Write<uint>(value, bytesCount);
        public void WriteInt32(int value) => Write<int>(value);
        public void WriteInt32(int value, int bytesCount) => Write<int>(value, bytesCount);
        public void WriteUInt64(ulong value) => Write<ulong>(value);
        public void WriteUInt64(ulong value, int bytesCount) => Write<ulong>(value, bytesCount);
        public void WriteInt64(long value) => Write<long>(value);
        public void WriteInt64(long value, int bytesCount) => Write<long>(value, bytesCount);
        public void WriteSingle(float value) => Write<float>(value);
        public void WriteDouble(double value) => Write<double>(value);
        public void WriteDecimal(decimal value) => Write<decimal>(value);
        public void WriteString(string? value)
        {
            if (value == null || value.Length == 0)
            {
                WriteLength(value?.Length);
                return;
            }

            var length = UTF8EncodingCache.Encoding.GetByteCount(value);
            WriteLength(length);
            UTF8EncodingCache.Encoding.GetBytes(value, GetSpan());
            Advance(length);
        }

        public void WriteLength(int? length) => LengthHelper.Write(ref this, length);

        public unsafe void Write<T>(T value) where T : unmanaged
        {
            var valueSpan = new Span<byte>(&value, sizeof(T));
            if (BitConverter.IsLittleEndian)
                valueSpan.Reverse();
            WriteInternal(valueSpan);
        }

        public unsafe void Write<T>(T value, int bytesCount) where T : unmanaged
        {
            ThrowIfOutOfRange<T>(bytesCount);
            Span<byte> valueSpan;
            if (BitConverter.IsLittleEndian)
            {
                valueSpan = new Span<byte>((byte*)&value + (sizeof(T) - bytesCount), bytesCount);
                valueSpan.Reverse();
            }
            else
            {
                valueSpan = new Span<byte>(&value, bytesCount);
            }
            WriteInternal(valueSpan);
        }

        unsafe void WriteInternal(in Span<byte> valueSpan)
        {
            ThrowIfCannotWrite(valueSpan.Length);
            valueSpan.CopyTo(GetSpan());
            Advance(valueSpan.Length);
        }

        static unsafe void ThrowIfOutOfRange<T>(int bytesCount) where T : unmanaged
        {
            if (bytesCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(bytesCount), $"bytesCount({bytesCount}) should be more than zero.");
            if (bytesCount > sizeof(T))
                throw new ArgumentOutOfRangeException(nameof(bytesCount), $"bytesCount({bytesCount}) should be less or eqaul to size({sizeof(T)}).");
        }

        private void ThrowIfCannotWrite(int size)
        {
            if (size > RemainSize)
                throw new OverflowException($"RemainSize({RemainSize}) is less than size({size}).");
        }

        

        public void Dispose()
        {
            if (isDisposed)
                return;
            owner.Dispose();
            isDisposed = true;
        }
    }
}
