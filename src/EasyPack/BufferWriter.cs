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
        public void Write(bool value) => Write((byte)(value ? 1 : 0));
        public void Write(byte value)
        {
            ThrowIfCannotWrite(1);
            owner.Memory.Span[WrittenSize++] = value;
        }
        public void Write(sbyte value) => Write((byte)value);
        public void Write(ushort value) => Write<ushort>(value);
        public void Write(short value) => Write<short>(value);
        public void Write(char value) => Write<char>(value);
        public void Write(uint value) => Write<uint>(value);
        public void Write(uint value, int bytesCount) => Write<uint>(value, bytesCount);
        public void Write(int value) => Write<int>(value);
        public void Write(int value, int bytesCount) => Write<int>(value, bytesCount);
        public void Write(ulong value) => Write<ulong>(value);
        public void Write(ulong value, int bytesCount) => Write<ulong>(value, bytesCount);
        public void Write(long value) => Write<long>(value);
        public void Write(long value, int bytesCount) => Write<long>(value, bytesCount);
        public void Write(float value) => Write<float>(value);
        public void Write(double value) => Write<double>(value);
        public void Write(decimal value) => Write<decimal>(value);
        public void Write(string? value)
        {
            if (value == null)
            {
                WriteNull();
                return;
            }

            if (value.Length == 0)
            {
                Write(Constants.ZERO);
                return;
            }

            var length = UTF8EncodingCache.Encoding.GetByteCount(value);
            WriteLength(length);
            UTF8EncodingCache.Encoding.GetBytes(value, GetSpan());
            Advance(length);
        }

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

        public void WriteLength(int length) => LengthHelper.Write(ref this, length);

        public void Dispose()
        {
            if (isDisposed)
                return;
            owner.Dispose();
            isDisposed = true;
        }
    }
}
