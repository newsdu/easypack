using AppAsToy.EasyPack;
using FluentAssertions;
using System;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{

    public partial class PackerTests
    {
        [Fact]
        public void Pack_Primitive()
        {
            Test_Pack_Unpack(false);
            Test_Pack_Unpack(true);
            Test_Pack_Unpack('a');
            Test_Pack_Unpack('\u1234');
            Test_Pack_Unpack((sbyte)1);
            Test_Pack_Unpack((short)1);
            Test_Pack_Unpack(1);
            Test_Pack_Unpack(1L);
            Test_Pack_Unpack((byte)1);
            Test_Pack_Unpack((ushort)1);
            Test_Pack_Unpack(1U);
            Test_Pack_Unpack(1UL);
            Test_Pack_Unpack(0.000001f);
            Test_Pack_Unpack(0.000000000001);
            Test_Pack_Unpack(1000000000000000000000m);
            Test_Pack_Unpack(DateTime.Now);
            Test_Pack_Unpack(DateTimeOffset.Now);
            Test_Pack_Unpack(TimeSpan.FromMilliseconds(1234.5678));
            Test_Pack_Unpack(Guid.NewGuid());
        }

        void Test_Pack_Unpack<T>(T value)
        {
            using var pack = Packer.Pack(value);
            Packer.Unpack<T>(pack.ToSpan()).Should().Be(value);
        }
    }
}
