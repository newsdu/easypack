using System;
using System.IO;

namespace AppAsToy.EasyPack
{
    internal sealed class ReadOnlyMemoryStream : Stream
    {
        readonly ReadOnlyMemory<byte> memory;
        long position;

        public ReadOnlyMemoryStream(ReadOnlyMemory<byte> memory) => this.memory = memory;

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length => memory.Length;
        public override long Position 
        {
            get => position; 
            set
            {
                if (value < 0 || value >= memory.Length)
                    throw new IndexOutOfRangeException($"{nameof(Position)}({value}) is out of range in Stream.");
                position = value;
            }
        }

        public override void Flush() { }
        public override int Read(byte[] buffer, int offset, int count)
        {
            memory.CopyTo(new Memory<byte>(buffer, offset, count));
            return count;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = Length + offset;
                    break;
            }
            return Position;
        }
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}