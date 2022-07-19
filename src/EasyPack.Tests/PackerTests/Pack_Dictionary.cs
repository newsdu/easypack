using AppAsToy.EasyPack;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class PackerTests
    {
        [Fact]
        public void Pack_Dictionary()
        {
            Test_Pack_Unpack_Dictionary_Raw<string, int>(null);
            Test_Pack_Unpack_Dictionary_Raw(new Dictionary<string, int>());
            Test_Pack_Unpack_Dictionary(true);
            Test_Pack_Unpack_Dictionary('a');
            Test_Pack_Unpack_Dictionary<sbyte>(1);
            Test_Pack_Unpack_Dictionary<short>(1);
            Test_Pack_Unpack_Dictionary(1);
            Test_Pack_Unpack_Dictionary<long>(1);
            Test_Pack_Unpack_Dictionary<byte>(1);
            Test_Pack_Unpack_Dictionary<ushort>(1);
            Test_Pack_Unpack_Dictionary<uint>(1);
            Test_Pack_Unpack_Dictionary<ulong>(1);
            Test_Pack_Unpack_Dictionary(0.000001f);
            Test_Pack_Unpack_Dictionary(0.0000000000001);
            Test_Pack_Unpack_Dictionary(10000000000000000000m);
            Test_Pack_Unpack_Dictionary(DateTime.Now);
            Test_Pack_Unpack_Dictionary(DateTimeOffset.Now);
            Test_Pack_Unpack_Dictionary(TimeSpan.FromMilliseconds(1234567.123));
            Test_Pack_Unpack_Dictionary(Guid.NewGuid());
        }

        void Test_Pack_Unpack_Dictionary_Raw<K, V>(Dictionary<K, V> values)
            where K : notnull
        {
            using var pack = Packer.Pack(values);
            Packer.Unpack<Dictionary<K, V>>(pack.ToSpan()).Should().BeEquivalentTo(values);
        }

        void Test_Pack_Unpack_Dictionary<V>(V value)
        {
            Test_Pack_Unpack_Dictionary_Raw(new Dictionary<string, V> { { "a", value } });
        }
    }
}
