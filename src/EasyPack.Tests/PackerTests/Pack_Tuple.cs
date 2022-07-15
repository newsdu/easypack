using AppAsToy.EasyPack;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.CompilerServices;

namespace EasyPack.Tests
{
    partial class PackerTests
    {
        [TestMethod]
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
