using AppAsToy.EasyPack;
using FluentAssertions;
using System;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class PackerTests
    {
        [Fact]
        public void Pack_Interface()
        {
            Test_Pack_Interface(new TestImplementClass() { a = 1 });
            Test_Pack_Interface(new TestImplementStruct() { a = 2 });
        }

        void Test_Pack_Interface(ITestInterface instance)
        {
            using var pack = Packer.Pack(instance);
            Packer.Unpack<ITestInterface>(pack.ToSpan()).Should().Be(instance);
        }
        
        interface ITestInterface : IEquatable<ITestInterface>
        {
            int a { get; set; }
        }

        class TestImplementClass : ITestInterface
        {
            public int a { get; set; }

            public bool Equals(ITestInterface? other) => a == other?.a;
            public override bool Equals(object? obj) => obj is TestImplementClass other && Equals(other);
        }

        struct TestImplementStruct : ITestInterface
        {
            public int a { get; set; }
            public bool Equals(ITestInterface? other) => a == other?.a;
            public override bool Equals(object? obj) => obj is TestImplementStruct other && Equals(other);
        }
    }
}
