using AppAsToy.EasyPack;
using FluentAssertions;
using System;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class PackerTests
    {
        [Fact]
        public void Pack_Nullable()
        {
            Test_Pack_Unpack_Nullable<int>(null);
            Test_Pack_Unpack_Nullable<bool>(true);
            Test_Pack_Unpack_Nullable<char>('a');
            Test_Pack_Unpack_Nullable<sbyte>(1);
            Test_Pack_Unpack_Nullable<short>(1);
            Test_Pack_Unpack_Nullable<int>(1);
            Test_Pack_Unpack_Nullable<long>(1);
            Test_Pack_Unpack_Nullable<byte>(1);
            Test_Pack_Unpack_Nullable<ushort>(1);
            Test_Pack_Unpack_Nullable<uint>(1);
            Test_Pack_Unpack_Nullable<ulong>(1);
            Test_Pack_Unpack_Nullable<float>(0.000001f);
            Test_Pack_Unpack_Nullable<double>(0.0000000000001);
            Test_Pack_Unpack_Nullable<decimal>(10000000000000000000m);
            Test_Pack_Unpack_Nullable<DateTime>(DateTime.Now);
            Test_Pack_Unpack_Nullable<DateTimeOffset>(DateTimeOffset.Now);
            Test_Pack_Unpack_Nullable<TimeSpan>(TimeSpan.FromMilliseconds(1234567.123));
            Test_Pack_Unpack_Nullable<Guid>(Guid.NewGuid());
        }

        void Test_Pack_Unpack_Nullable<T>(T? value)
            where T : struct
        {
            using var pack = Packer.Pack(value);
            Packer.Unpack<T?>(pack.ToSpan()).Should().Be(value);
        }
    }
}
