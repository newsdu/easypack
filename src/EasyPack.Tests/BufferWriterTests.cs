using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AppAsToy.EasyPack.Tests
{
    [TestClass()]
    public class BufferWriterTests
    {
        [TestMethod]
        public void IsValidTest()
        {
            new BufferWriter().IsValid.Should().BeFalse();
            using var writer = BufferWriter.Rent();
            writer.IsValid.Should().BeTrue();
        }

        [TestMethod()]
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

        [TestMethod()]
        public void AdvanceTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.RemainSize.Should().Be(16);
            writer.WrittenSize.Should().Be(0);
            writer.Advance(7);
            writer.WrittenSize.Should().Be(7);
            writer.RemainSize.Should().Be(9);
        }

        [TestMethod()]
        public void GetSpanTest()
        {
            using var writer = BufferWriter.Rent(128);
            writer.GetSpan().Length.Should().Be(128);
            writer.Advance(7);
            writer.GetSpan().Length.Should().Be(121);
        }

        [TestMethod()]
        public void GetWrittenMemoryTest()
        {
            using var writer = BufferWriter.Rent(8);
            writer.GetWrittenMemory().Length.Should().Be(0);
            writer.Advance(7);
            writer.GetWrittenMemory().Length.Should().Be(7);
        }

        [TestMethod()]
        public void GetWrittenSpanTest()
        {
            using var writer = BufferWriter.Rent(8);
            writer.GetWrittenSpan().Length.Should().Be(0);
            writer.Advance(7);
            writer.GetWrittenSpan().Length.Should().Be(7);
        }

        [TestMethod()]
        public void WriteNullTest()
        {
            using var writer = BufferWriter.Rent(2);
            writer.WriteNull();
            writer.GetWrittenSpan()[0].Should().Be(Constants.NULL);
        }

        [TestMethod()]
        public void WriteBoolTest()
        {
            using var writer = BufferWriter.Rent(2);
            writer.Write(true);
            writer.GetWrittenSpan()[0].Should().Be(1);
            writer.Write(false);
            writer.GetWrittenSpan()[1].Should().Be(0);
        }

        [TestMethod()]
        public unsafe void WriteCharTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.Write('\u1234');
            writer.GetWrittenSpan().SequenceEqual(new byte[]{ 0x12, 0x34 }).Should().BeTrue();
        }

        [TestMethod()]
        public void WriteByteTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.Write((byte)0x12);
            writer.GetWrittenSpan()[0].Should().Be(0x12);
        }

        [TestMethod()]
        public void WriteUShortTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.Write((ushort)0x5678);
            writer.GetWrittenSpan().SequenceEqual(new byte[] {0x56,0x78}).Should().BeTrue();
        }

        [TestMethod()]
        public void WriteUIntTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.Write((uint)0x12345678);
            writer.GetWrittenSpan().SequenceEqual(new byte[] { 0x12, 0x34, 0x56, 0x78 }).Should().BeTrue();
        }

        [TestMethod()]
        public void WriteULongTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.Write((ulong)0x1234567887654321);
            writer.GetWrittenSpan()
                  .SequenceEqual(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x87, 0x65, 0x43, 0x21 })
                  .Should()
                  .BeTrue();
        }

        [TestMethod()]
        public void WriteSByteTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.Write((sbyte)0x12);
            writer.GetWrittenSpan()[0].Should().Be(0x12);
        }

        [TestMethod()]
        public void WriteShortTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.Write((short)0x5678);
            writer.GetWrittenSpan().SequenceEqual(new byte[] { 0x56, 0x78 }).Should().BeTrue();
        }

        [TestMethod()]
        public void WriteIntTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.Write(0x12345678);
            writer.GetWrittenSpan().SequenceEqual(new byte[] { 0x12, 0x34, 0x56, 0x78 }).Should().BeTrue();
        }

        [TestMethod()]
        public void WriteLongTest()
        {
            using var writer = BufferWriter.Rent(16);
            writer.Write(0x1234567887654321);
            writer.GetWrittenSpan()
                  .SequenceEqual(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x87, 0x65, 0x43, 0x21 })
                  .Should()
                  .BeTrue();
        }

        [TestMethod()]
        public unsafe void WriteHalfTest()
        {
            var value = (Half)0x1234;
            byte* ptr = (byte*)&value;
            using var writer = BufferWriter.Rent(16);
            var writtenValue = new Span<byte>(ptr, 2);
            writer.Write(value);
            writtenValue.Reverse();
            writer.GetWrittenSpan()
                  .SequenceEqual(writtenValue)
                  .Should()
                  .BeTrue();
        }

        [TestMethod()]
        public unsafe void WriteFloatTest()
        {
            var value = (float)0x12345678;
            byte* ptr = (byte*)&value;
            using var writer = BufferWriter.Rent(16);
            var writtenValue = new Span<byte>(ptr, 4);
            writer.Write(value);
            writtenValue.Reverse();
            writer.GetWrittenSpan()
                  .SequenceEqual(writtenValue)
                  .Should()
                  .BeTrue();
        }

        [TestMethod()]
        public unsafe void WriteDoubleTest()
        {
            var value = (double)0x1234567887654321;
            byte* ptr = (byte*)&value;
            using var writer = BufferWriter.Rent(16);
            var writtenValue = new Span<byte>(ptr, 8);
            writer.Write(value);
            writtenValue.Reverse();
            writer.GetWrittenSpan()
                  .SequenceEqual(writtenValue)
                  .Should()
                  .BeTrue();
        }

        [TestMethod()]
        public unsafe void WriteDecimalTest()
        {
            decimal value = new decimal(0x12345678, 0x23456789, 0x68765543, false, 28);
            byte* ptr = (byte*)&value;
            using var writer = BufferWriter.Rent(16);
            var writtenValue = new Span<byte>(ptr, 16);
            writer.Write(value);
            writtenValue.Reverse();
            writer.GetWrittenSpan()
                  .SequenceEqual(writtenValue)
                  .Should()
                  .BeTrue();
        }

        [TestMethod()]
        public unsafe void WriteStringTest()
        {
            using (var writer = BufferWriter.Rent(16))
            {
                writer.Write(null);
                writer.GetWrittenSpan()[0].Should().Be(Constants.NULL);
                writer.Write("");
                writer.GetWrittenSpan()[1].Should().Be(Constants.ZERO);
            }
            using (var writer = BufferWriter.Rent(16))
            {
                writer.Write("abc");
                writer.GetWrittenSpan()[0].Should().Be(3);
                writer.GetWrittenSpan()[1].Should().Be((byte)'a');
                writer.GetWrittenSpan()[2].Should().Be((byte)'b');
                writer.GetWrittenSpan()[3].Should().Be((byte)'c');
            }
        }

        [TestMethod()]
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
                new Action(() => writer.WriteLength(0b_0011_1111 << 24 | 0xffffff)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod()]
        public void DisposeTest()
        {
            using var writer = BufferWriter.Rent();
            writer.IsValid.Should().BeTrue();
            writer.Dispose();
            writer.IsValid.Should().BeFalse();
            new Action(() => writer.Dispose()).Should().NotThrow();
        }

        [TestMethod()]
        public void OverflowTest()
        {
            using var writer = BufferWriter.Rent(16);
            new Action(() => writer.Write(1, -1)).Should().Throw<ArgumentOutOfRangeException>();
            new Action(() => writer.Write(2, 5)).Should().Throw<ArgumentOutOfRangeException>();
            writer.Advance(16);
            new Action(() => writer.Write(3)).Should().Throw<OverflowException>();
        }
    }
}