using AppAsToy.EasyPack;
using FluentAssertions;
using System;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class PackerTests
    {
        [Fact]
        public void Pack_Array()
        {
            Test_Pack_Unpack_Array_Raw<int>(null);
            Test_Pack_Unpack_Array_Raw<int>(Array.Empty<int>());
            Test_Pack_Unpack_Array(true);
            Test_Pack_Unpack_Array('a');
            Test_Pack_Unpack_Array<sbyte>(1);
            Test_Pack_Unpack_Array<short>(1);
            Test_Pack_Unpack_Array(1);
            Test_Pack_Unpack_Array<long>(1);
            Test_Pack_Unpack_Array<byte>(1);
            Test_Pack_Unpack_Array<ushort>(1);
            Test_Pack_Unpack_Array<uint>(1);
            Test_Pack_Unpack_Array<ulong>(1);
            Test_Pack_Unpack_Array(0.000001f);
            Test_Pack_Unpack_Array(0.0000000000001);
            Test_Pack_Unpack_Array(10000000000000000000m);
            Test_Pack_Unpack_Array(DateTime.Now);
            Test_Pack_Unpack_Array(DateTimeOffset.Now);
            Test_Pack_Unpack_Array(TimeSpan.FromMilliseconds(1234567.123));
            Test_Pack_Unpack_Array(Guid.NewGuid());
        }        

        void Test_Pack_Unpack_Array_Raw<T>(T[] values)
        {
            using var pack = Packer.Pack(values);
            Packer.Unpack<T[]>(pack.ToSpan()).Should().BeEquivalentTo(values);
        }

        void Test_Pack_Unpack_Array<T>(T value)
        {
            Test_Pack_Unpack_Array_Raw(new T[] { value });
        }
    }
}
