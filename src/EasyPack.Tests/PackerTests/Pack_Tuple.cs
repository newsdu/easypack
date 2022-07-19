using AppAsToy.EasyPack;
using FluentAssertions;
using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class PackerTests
    {
        [Fact]
        public void Pack_Tuple()
        {
            TestPackTuple(new Tuple<int, string>(1, "abc"));
            TestPackTuple(new Tuple<Tuple<int, string>, Tuple<string, int>>(
                new Tuple<int, string>(1, "abc"), 
                new Tuple<string, int>("def", 2)));
            TestPackTuple(new ValueTuple<int, string>(1, "abc"));
            TestPackTuple(((1,"abc"), ("def", 2)));
        }

        public void TestPackTuple<TTuple>(TTuple tuple)
            where TTuple : ITuple
        {
            using var pack = Packer.Pack(tuple);
            Packer.Unpack<TTuple>(pack.ToSpan()).Should().Be(tuple);
        }
    }
}
