using FluentAssertions;
using System;
using System.Buffers.Binary;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    public class BufferReaderTests
    {
        [Fact]
        public void IsValidTest()
        {
            new BufferReader().IsValid.Should().BeFalse();
            new BufferReader(new byte[1]).IsValid.Should().BeTrue();
        }

        [Fact]
        public void AdvanceTest()
        {
            var reader = new BufferReader(new byte[4]);
            reader.Capacity.Should().Be(4);
            reader.RemainSize.Should().Be(4);
            reader.ReadSize.Should().Be(0);
            reader.Advance(3);
            reader.Capacity.Should().Be(4);
            reader.RemainSize.Should().Be(1);
            reader.ReadSize.Should().Be(3);
        }

        [Fact]
        public void GetSpanTest()
        {
            var reader = new BufferReader(new byte[4]);
            reader.GetSpan().Length.Should().Be(4);
            reader.Advance(3);
            reader.GetSpan().Length.Should().Be(1);
            reader.Advance(1);
            reader.GetSpan().IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void ReadNullTest()
        {
            var reader = new BufferReader(new byte[4]);
            reader.ReadNull();
            reader.ReadSize.Should().Be(1);
            reader.RemainSize.Should().Be(3);
        }

        [Theory]
        [InlineData((byte)0, false)]
        [InlineData((byte)1, true)]
        [InlineData(byte.MaxValue, true)]
        public void ReadBooleanTest(byte value, bool expected)
        {
            new BufferReader(new byte[] { value }).ReadBoolean().Should().Be(expected);
        }

        [Theory]
        [InlineData((byte)1)]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        public void ReadByteTest(byte value)
        {
            new BufferReader(new byte[] { value }).ReadByte().Should().Be(value);
        }

        [Theory]
        [InlineData((sbyte)1)]
        [InlineData(sbyte.MinValue)]
        [InlineData(sbyte.MaxValue)]
        public void ReadSByteTest(sbyte value)
        {
            new BufferReader(new byte[] { (byte)value }).ReadSByte().Should().Be(value);
        }

        [Theory]
        [InlineData((ushort)1)]
        public void ReadUInt16Test(ushort value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt16().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadUInt16().Should().Be(value);
        }

        [Theory]
        [InlineData((short)1)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        public void ReadInt16Test(short value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt16().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadInt16().Should().Be(value);
        }

        [Theory]
        [InlineData((char)1)]
        public void ReadCharTest(char value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadChar().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadChar().Should().Be(value);
        }

        [Fact]
        public void ReadUInt32Test()
        {
            var value = 1u;
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt32().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadUInt32().Should().Be(value);
        }

        [Theory]
        [InlineData(1U, 2)]
        [InlineData(1U, 3)]
        [InlineData(1U, 4)]
        public void ReadUInt32Test_BytesCount(uint value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)[..bytesCount]).ReadUInt32(bytesCount).Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))[(4-bytesCount)..]).ReadUInt32(bytesCount).Should().Be(value);
        }

        [Theory]
        [InlineData(1u, -1)]
        [InlineData(1u, 5)]
        public void InvalidReadUInt32Test(uint value, int bytesCount)
        {
            new Action(() => new BufferReader(BitConverter.GetBytes(value)).ReadUInt32(bytesCount)).Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void ReadInt32Test(int value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt32().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadInt32().Should().Be(value);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(1, 4)]
        public void ReadInt32Test_BytesCount(int value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)[..bytesCount]).ReadInt32(bytesCount).Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))[(4-bytesCount)..]).ReadInt32(bytesCount).Should().Be(value);
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(1, 5)]
        public void InvalidReadInt32Test(int value, int bytesCount)
        {
            new Action(() => new BufferReader(BitConverter.GetBytes(value)).ReadInt32(bytesCount)).Should().Throw<Exception>();
        }

        [Fact]
        public void ReadUInt64Test()
        {
            var value = 1ul;
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt64().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadUInt64().Should().Be(value);
        }

        [Theory]
        [InlineData(1UL, 2)]
        [InlineData(1UL, 3)]
        [InlineData(1UL, 4)]
        [InlineData(1UL, 5)]
        [InlineData(1UL, 6)]
        [InlineData(1UL, 7)]
        [InlineData(1UL, 8)]
        public void ReadUInt64Test_BytesCount(ulong value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)[..bytesCount]).ReadUInt64(bytesCount).Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))[(8-bytesCount)..]).ReadUInt64(bytesCount).Should().Be(value);
        }

        [Theory]
        [InlineData(1ul, -1)]
        [InlineData(1ul, 9)]
        public void InvalidReadUInt64Test(ulong value, int bytesCount)
        {
            new Action(() => new BufferReader(BitConverter.GetBytes(value)).ReadUInt64(bytesCount)).Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(1L)]
        [InlineData(long.MaxValue)]
        public void ReadInt64Test(long value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt64().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadInt64().Should().Be(value);
        }

        [Theory]
        [InlineData(1L, 2)]
        [InlineData(1L, 3)]
        [InlineData(1L, 4)]
        [InlineData(1L, 5)]
        [InlineData(1L, 6)]
        [InlineData(1L, 7)]
        [InlineData(1L, 8)]
        public void ReadInt64Test_BytesCount(long value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)[..bytesCount]).ReadInt64(bytesCount).Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))[(8-bytesCount)..]).ReadInt64(bytesCount).Should().Be(value);
        }

        [Theory]
        [InlineData(1L, -1)]
        [InlineData(1L, 9)]
        public void InvalidReadInt64Test(long value, int bytesCount)
        {
            new Action(() => new BufferReader(BitConverter.GetBytes(value)).ReadInt64(bytesCount)).Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(1f)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue)]
        public unsafe void ReadSingleTest(float value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadSingle().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(*(uint*)&value))).ReadSingle().Should().Be(value);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        public unsafe void ReadDoubleTest(double value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadDouble().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(*(ulong*)&value))).ReadDouble().Should().Be(value);
        }

        [Fact]
        public void ReadDecimalTest()
        {
            Test(1m);
            Test(decimal.MinValue);
            Test(decimal.MaxValue);
            static unsafe void Test(decimal value)
            {
                new BufferReader(new ReadOnlySpan<byte>(&value, sizeof(decimal))).ReadDecimal().Should().NotBe(value);
                var copyValue = value;
                var copyValueSpan = new Span<byte>(&copyValue, sizeof(decimal));
                copyValueSpan.Reverse();
                new BufferReader(copyValueSpan).ReadDecimal().Should().Be(value);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("가나다")]
        [InlineData("asdfghjklkjhgfdsaqwertyuiopoiuytrewqzxcvbnm,./.,mnbvcxz")]
        public void ReadStringTest(string value)
        {
            using var writer = BufferWriter.Rent();
            writer.WriteString(value);
            new BufferReader(writer.GetWrittenSpan()).ReadString().Should().Be(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(0b_0011_1111)]
        [InlineData(0b_0011_1111__1111_1111)]
        [InlineData(0b_0011_1111__1111_1111__1111_1111)]
        [InlineData(0b_0001_1111__1111_1111__1111_1111__1111_1111)]
        public void ReadLengthTest(int? length)
        {
            using var writer = BufferWriter.Rent();
            writer.WriteLength(length);
            new BufferReader(writer.GetWrittenSpan()).ReadLength().Should().Be(length);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0b_0011_1111__1111_1111__1111_1111__1111_1111)]
        [InlineData(int.MaxValue)]
        public void InvalidReadLengthTest(int? length)
        {
            new Action(() =>
            {
                using var writer = BufferWriter.Rent();
                writer.WriteLength(length);
                new BufferReader(writer.GetWrittenSpan()).ReadLength();
            }).Should().Throw<Exception>();
        }
    }
}