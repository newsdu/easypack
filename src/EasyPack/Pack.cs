using System;
using System.IO;

namespace AppAsToy.EasyPack
{
    public readonly struct Pack : IDisposable
    {
        readonly BufferWriter writer;

        internal Pack(BufferWriter writer) => this.writer = writer;

        public byte[] ToArray() => ToSpan().ToArray();
        public ReadOnlySpan<byte> ToSpan() => writer.GetWrittenSpan();
        public Stream ToStream() => new ReadOnlyMemoryStream(writer.GetWrittenMemory());

        void DisposeInternal()
        {
            if (writer.IsValid)
                writer.Dispose();
        }

        public void Dispose()
        {
            DisposeInternal();
            GC.SuppressFinalize(this);
        }
    }
}