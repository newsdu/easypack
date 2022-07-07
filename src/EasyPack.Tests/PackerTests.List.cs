using AppAsToy.EasyPack;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EasyPack.Tests
{
    partial class PackerTests
    {
        [TestMethod]
        public void Should_Pack_List_Type()
        {
            Test_Pack_Unpack_List_Raw<int>(null);
            Test_Pack_Unpack_List_Raw(new List<int>());
            Test_Pack_Unpack_List(true);
            Test_Pack_Unpack_List('a');
            Test_Pack_Unpack_List<sbyte>(1);
            Test_Pack_Unpack_List<short>(1);
            Test_Pack_Unpack_List(1);
            Test_Pack_Unpack_List<long>(1);
            Test_Pack_Unpack_List<byte>(1);
            Test_Pack_Unpack_List<ushort>(1);
            Test_Pack_Unpack_List<uint>(1);
            Test_Pack_Unpack_List<ulong>(1);
            Test_Pack_Unpack_List(0.000001f);
            Test_Pack_Unpack_List(0.0000000000001);
            Test_Pack_Unpack_List(10000000000000000000m);
            Test_Pack_Unpack_List(DateTime.Now);
            Test_Pack_Unpack_List(DateTimeOffset.Now);
            Test_Pack_Unpack_List(TimeSpan.FromMilliseconds(1234567.123));
            Test_Pack_Unpack_List(Guid.NewGuid());
        }

        void Test_Pack_Unpack_List_Raw<T>(List<T> values)
        {
            using var pack = Packer.Pack(values);
            Packer.Unpack<List<T>>(pack.ToSpan()).Should().BeEquivalentTo(values);
        }

        void Test_Pack_Unpack_List<T>(T value)
        {
            Test_Pack_Unpack_List_Raw(new List<T> { value });
        }
    }
}
