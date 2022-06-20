using AppAsToy.EasyPack;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Buffers.Binary;


namespace AppAsToy.EasyPack.Tests
{
    [TestClass()]
    public class BufferReaderTests
    {
        [TestMethod()]
        public void IsValidTest()
        {
            new BufferReader().IsValid.Should().BeFalse();
            new BufferReader(new byte[1]).IsValid.Should().BeTrue();
        }

        [TestMethod()]
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

        [TestMethod()]
        public void GetSpanTest()
        {
            var reader = new BufferReader(new byte[4]);
            reader.GetSpan().Length.Should().Be(4);
            reader.Advance(3);
            reader.GetSpan().Length.Should().Be(1);
            reader.Advance(1);
            reader.GetSpan().IsEmpty.Should().BeTrue();
        }

        [TestMethod()]
        public void ReadNullTest()
        {
            var reader = new BufferReader(new byte[4]);
            reader.ReadNull();
            reader.ReadSize.Should().Be(1);
            reader.RemainSize.Should().Be(3);
        }

        [DataTestMethod]
        [DataRow((byte)0, false)]
        [DataRow((byte)1, true)]
        [DataRow(byte.MaxValue, true)]
        public void ReadBooleanTest(byte value, bool expected)
        {
            new BufferReader(new byte[] { value }).ReadBoolean().Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow((byte)1)]
        [DataRow(byte.MinValue)]
        [DataRow(byte.MaxValue)]
        public void ReadByteTest(byte value)
        {
            new BufferReader(new byte[] { value }).ReadByte().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow((sbyte)1)]
        [DataRow(sbyte.MinValue)]
        [DataRow(sbyte.MaxValue)]
        public void ReadSByteTest(sbyte value)
        {
            new BufferReader(new byte[] { (byte)value }).ReadSByte().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow((ushort)1)]
        public void ReadUInt16Test(ushort value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt16().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadUInt16().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow((short)1)]
        [DataRow(short.MinValue)]
        [DataRow(short.MaxValue)]
        public void ReadInt16Test(short value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt16().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadInt16().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow((char)1)]
        public void ReadCharTest(char value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadChar().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadChar().Should().Be(value);
        }

        [TestMethod]
        public void ReadUInt32Test()
        {
            var value = 1u;
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt32().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadUInt32().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(1u, 1)]
        [DataRow(1u, 2)]
        [DataRow(1u, 3)]
        [DataRow(1u, 4)]
        public void ReadUInt32Test(uint value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt32(bytesCount).Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadUInt32(bytesCount).Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(1u, -1)]
        [DataRow(1u, 5)]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void InvalidReadUInt32Test(uint value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt32(bytesCount);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(int.MaxValue)]
        public void ReadInt32Test(int value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt32().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadInt32().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(1, 2)]
        [DataRow(1, 3)]
        [DataRow(1, 4)]
        public void ReadInt32Test(int value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt32(bytesCount).Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadInt32(bytesCount).Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(1, -1)]
        [DataRow(1, 5)]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void InvalidReadInt32Test(int value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt32(bytesCount);
        }

        [TestMethod]
        public void ReadUInt64Test()
        {
            var value = 1ul;
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt64().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadUInt64().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(1ul, 1)]
        [DataRow(1ul, 2)]
        [DataRow(1ul, 3)]
        [DataRow(1ul, 4)]
        [DataRow(1ul, 5)]
        [DataRow(1ul, 6)]
        [DataRow(1ul, 7)]
        [DataRow(1ul, 8)]
        public void ReadUInt64Test(ulong value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt64(bytesCount).Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadUInt64(bytesCount).Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(1ul, -1)]
        [DataRow(1ul, 9)]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void InvalidReadUInt64Test(ulong value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadUInt64(bytesCount);
        }

        [DataTestMethod]
        [DataRow(1L)]
        [DataRow(long.MaxValue)]
        public void ReadInt64Test(long value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt64().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadInt64().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(1L, 1)]
        [DataRow(1L, 2)]
        [DataRow(1L, 3)]
        [DataRow(1L, 4)]
        [DataRow(1L, 5)]
        [DataRow(1L, 6)]
        [DataRow(1L, 7)]
        [DataRow(1L, 8)]
        public void ReadInt64Test(long value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt64(bytesCount).Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(value))).ReadInt64(bytesCount).Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(1L, -1)]
        [DataRow(1L, 9)]
        [ExpectedException(typeof(Exception),AllowDerivedTypes =true)]
        public void InvalidReadInt64Test(long value, int bytesCount)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadInt64(bytesCount);
        }

        [DataTestMethod]
        [DataRow(1f)]
        [DataRow(float.MinValue)]
        [DataRow(float.MaxValue)]
        public unsafe void ReadSingleTest(float value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadSingle().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(*(uint*)&value))).ReadSingle().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(1.0)]
        [DataRow(double.MinValue)]
        [DataRow(double.MaxValue)]
        public unsafe void ReadDoubleTest(double value)
        {
            new BufferReader(BitConverter.GetBytes(value)).ReadDouble().Should().NotBe(value);
            new BufferReader(BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(*(ulong*)&value))).ReadDouble().Should().Be(value);
        }

        [TestMethod]
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

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("abc")]
        [DataRow("가나다")]
        [DataRow("asdfghjklkjhgfdsaqwertyuiopoiuytrewqzxcvbnm,./.,mnbvcxz")]
        public void ReadStringTest(string value)
        {
            using var writer = BufferWriter.Rent();
            writer.WriteString(value);
            new BufferReader(writer.GetWrittenSpan()).ReadString().Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow(0)]
        [DataRow(0b_0011_1111)]
        [DataRow(0b_0011_1111__1111_1111)]
        [DataRow(0b_0011_1111__1111_1111__1111_1111)]
        [DataRow(0b_0001_1111__1111_1111__1111_1111__1111_1111)]
        public void ReadLengthTest(int? length)
        {
            using var writer = BufferWriter.Rent();
            writer.WriteLength(length);
            new BufferReader(writer.GetWrittenSpan()).ReadLength().Should().Be(length);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0b_0011_1111__1111_1111__1111_1111__1111_1111)]
        [DataRow(int.MaxValue)]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void InvalidReadLengthTest(int? length)
        {
            using var writer = BufferWriter.Rent();
            writer.WriteLength(length);
            new BufferReader(writer.GetWrittenSpan()).ReadLength();
        }
    }
}