using FluentAssertions;
using System;
using System.Buffers.Binary;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{

    public class BufferWriterTests
    {
        [Fact]
        public void IsValidTest()
        {
            new BufferWriter().IsValid.Should().BeFalse();
            using var writer = BufferWriter.Rent();
            writer.IsValid.Should().BeTrue();
        }

        [Fact]
        public void RentTest()
        {
            using (var writer = BufferWriter.Rent(1024))
            {
                writer.Capacity.Should().Be(1024);
                writer.WrittenSize.Should().Be(0);
            }

            using (var writer = BufferWriter.Rent(8))
                writer.Capacity.Should().Be(16);
            using (var writer = BufferWriter.Rent(1023))
                writer.Capacity.Should().Be(1024);
            using (var writer = BufferWriter.Rent(1025))
                writer.Capacity.Should().Be(2048);
        }

        [Fact]
        public void AdvanceTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.RemainSize.Should().Be(16);
            writer.WrittenSize.Should().Be(0);
            writer.Advance(7);
            writer.WrittenSize.Should().Be(7);
            writer.RemainSize.Should().Be(9);
        }

        [Fact]
        public void GetSpanTest()
        {
            using var writer = BufferWriter.Rent(128);
            writer.GetSpan().Length.Should().Be(128);
            writer.Advance(7);
            writer.GetSpan().Length.Should().Be(121);
        }

        [Fact]
        public void GetWrittenMemoryTest()
        {
            using var writer = BufferWriter.Rent(8);
            writer.GetWrittenMemory().Length.Should().Be(0);
            writer.Advance(7);
            writer.GetWrittenMemory().Length.Should().Be(7);
        }

        [Fact]
        public void GetWrittenSpanTest()
        {
            using var writer = BufferWriter.Rent(8);
            writer.GetWrittenSpan().Length.Should().Be(0);
            writer.Advance(7);
            writer.GetWrittenSpan().Length.Should().Be(7);
        }

        [Fact]
        public void WriteNullTest()
        {
            using var writer = BufferWriter.Rent(2);
            writer.WriteNull();
            writer.GetWrittenSpan()[0].Should().Be(Constants.NULL);
        }

        [Fact]
        public void WriteBoolTest()
        {
            using var writer = BufferWriter.Rent(2);
            writer.WriteBoolean(true);
            writer.GetWrittenSpan()[0].Should().Be(1);
            writer.WriteBoolean(false);
            writer.GetWrittenSpan()[1].Should().Be(0);
        }

        [Fact]
        public unsafe void WriteCharTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.WriteChar('\u1234');
            writer.GetWrittenSpan().SequenceEqual(new byte[]{ 0x12, 0x34 }).Should().BeTrue();
        }

        [Fact]
        public void WriteByteTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.WriteByte(0x12);
            writer.GetWrittenSpan()[0].Should().Be(0x12);
        }

        [Fact]
        public void WriteUInt16Test()
        {
            ushort value = 0x5678;
            using var writer = BufferWriter.Rent(16);
            writer.WriteUInt16(value);
            BitConverter.ToUInt16(writer.GetWrittenSpan()).Should().NotBe(value);
            BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt16(writer.GetWrittenSpan())).Should().Be(value);
        }

        [Fact]
        public void WriteUInt32Test()
        {
            uint value = 0x12345678u;
            using var writer = BufferWriter.Rent(16);
            writer.WriteUInt32(value);
            BitConverter.ToUInt32(writer.GetWrittenSpan()).Should().NotBe(value);
            BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt32(writer.GetWrittenSpan())).Should().Be(value);
        }

        [Fact]
        public void WriteUInt64Test()
        {
            var value = 0x1234567898765432ul;
            using var writer = BufferWriter.Rent(16);
            writer.WriteUInt64(value);
            BitConverter.ToUInt64(writer.GetWrittenSpan()).Should().NotBe(value);
            BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt64(writer.GetWrittenSpan())).Should().Be(value);
        }

        [Fact]
        public void WriteSByteTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.WriteSByte(0x12);
            writer.GetWrittenSpan()[0].Should().Be(0x12);
        }

        [Fact]
        public void WriteInt16Test()
        {
            short value = 0x5678;
            using var writer = BufferWriter.Rent(16);
            writer.WriteInt16(value);
            BitConverter.ToInt16(writer.GetWrittenSpan()).Should().NotBe(value);
            BinaryPrimitives.ReverseEndianness(BitConverter.ToInt16(writer.GetWrittenSpan())).Should().Be(value);
        }

        [Fact]
        public void WriteInt32Test()
        {
            var value = 0x12345678;
            using var writer = BufferWriter.Rent(16);
            writer.WriteInt32(value);
            BitConverter.ToInt32(writer.GetWrittenSpan()).Should().NotBe(value);
            BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(writer.GetWrittenSpan())).Should().Be(value);
        }

        [Fact]
        public void WriteInt64Test()
        {
            var value = 0x1234567898765432L;
            using var writer = BufferWriter.Rent(16);
            writer.WriteInt64(value);
            BitConverter.ToInt64(writer.GetWrittenSpan()).Should().NotBe(value);
            BinaryPrimitives.ReverseEndianness(BitConverter.ToInt64(writer.GetWrittenSpan())).Should().Be(value);
        }

        [Fact]
        public unsafe void WriteHalfTest()
        {
            var halfValue = (Half)0x1234;
            var value = *(ushort*)&halfValue;
            using var writer = BufferWriter.Rent(16);
            writer.WriteUInt16(value);
            BitConverter.ToUInt16(writer.GetWrittenSpan()).Should().NotBe(value);
            BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt16(writer.GetWrittenSpan())).Should().Be(value);
        }

        [Fact]
        public unsafe void WriteSingleTest()
        {
            var value = (float)0x12345678;
            using var writer = BufferWriter.Rent(16);
            writer.WriteSingle(value);
            BitConverter.ToSingle(writer.GetWrittenSpan()).Should().NotBe(value);
            var single = BitConverter.ToSingle(writer.GetWrittenSpan());
            var singleUint = BinaryPrimitives.ReverseEndianness(*(uint*)&single);
            var reverseSingle = *(float*)&singleUint;
            reverseSingle.Should().Be(value);
        }

        [Fact]
        public unsafe void WriteDoubleTest()
        {
            var value = (double)0x1234567887654321;
            using var writer = BufferWriter.Rent(16);
            writer.WriteDouble(value);
            BitConverter.ToDouble(writer.GetWrittenSpan()).Should().NotBe(value);
            var single = BitConverter.ToDouble(writer.GetWrittenSpan());
            var singleUint = BinaryPrimitives.ReverseEndianness(*(ulong*)&single);
            var reverseSingle = *(double*)&singleUint;
            reverseSingle.Should().Be(value);
        }

        [Fact]
        public unsafe void WriteDecimalTest()
        {
            decimal value = new (0x12345678, 0x23456789, 0x68765543, false, 28);
            byte* ptr = (byte*)&value;
            using var writer = BufferWriter.Rent(16);
            var writtenValue = new Span<byte>(ptr, 16);
            writer.WriteDecimal(value);
            writtenValue.Reverse();
            writer.GetWrittenSpan()
                  .SequenceEqual(writtenValue)
                  .Should()
                  .BeTrue();
        }

        [Fact]
        public unsafe void WriteStringTest()
        {
            using (var writer = BufferWriter.Rent(16))
            {
                writer.WriteString(null);
                writer.GetWrittenSpan()[0].Should().Be(Constants.NULL);
                writer.WriteString("");
                writer.GetWrittenSpan()[1].Should().Be(Constants.ZERO);
            }
            using (var writer = BufferWriter.Rent(16))
            {
                writer.WriteString("abc");
                writer.GetWrittenSpan()[0].Should().Be(3);
                writer.GetWrittenSpan()[1].Should().Be((byte)'a');
                writer.GetWrittenSpan()[2].Should().Be((byte)'b');
                writer.GetWrittenSpan()[3].Should().Be((byte)'c');
            }
        }

        [Fact]
        public void WriteLengthTest()
        {
            using (var writer = BufferWriter.Rent(16))
            {
                writer.WriteLength(0b_0011_1111);
                writer.WrittenSize.Should().Be(1);
            }

            using (var writer = BufferWriter.Rent(16))
            {
                writer.WriteLength(0b_0011_1111 << 8 | 0xff);
                writer.WrittenSize.Should().Be(2);
            }

            using (var writer = BufferWriter.Rent(16))
            {
                writer.WriteLength(0b_0011_1111 << 16 | 0xffff);
                writer.WrittenSize.Should().Be(3);
            }

            using (var writer = BufferWriter.Rent(16))
            {
                writer.WriteLength(0b_0001_1111 << 24 | 0xffffff);
                writer.WrittenSize.Should().Be(4);
            }

            using (var writer = BufferWriter.Rent(16))
                new Action(() => writer.WriteLength(-1)).Should().Throw<ArgumentOutOfRangeException>();

            using (var writer = BufferWriter.Rent(16))
                new Action(() => writer.WriteLength(0b_0011_1111 << 24 | 0xffffff)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void DisposeTest()
        {
            using var writer = BufferWriter.Rent();
            writer.IsValid.Should().BeTrue();
            writer.Dispose();
            writer.IsValid.Should().BeFalse();
            new Action(() => writer.Dispose()).Should().NotThrow();
        }

        [Fact]
        public void OverflowTest()
        {
            using var writer = BufferWriter.Rent(16);
            new Action(() => writer.WriteInt32(1, -1)).Should().Throw<ArgumentOutOfRangeException>();
            new Action(() => writer.WriteInt32(2, 5)).Should().Throw<ArgumentOutOfRangeException>();
            writer.Advance(16);
            new Action(() => writer.WriteInt32(3)).Should().Throw<OverflowException>();
        }
    }
}