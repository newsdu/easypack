using AppAsToy.EasyPack;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EasyPack.Tests
{
    partial class PackerTests
    {
        [TestMethod]
        public void Pack_AbstractType_Test()
        {
            Test_Pack_Abstract(new TestDerivedClass1 { a = 1, b = 2 });
            Test_Pack_Abstract(new TestDerivedClass2 { a = 3, c = 4 });
        }
        
        void Test_Pack_Abstract(TestAbstract instance)
        {
            using var pack = Packer.Pack(instance);
            Packer.Unpack<TestAbstract>(pack.ToSpan()).Should().Be(instance);
        }

        abstract class TestAbstract : IEquatable<TestAbstract>
        {
            public abstract int a { get; set; }

            public bool Equals(TestAbstract? other) => a == other?.a;

            public override bool Equals(object obj) => obj is TestAbstract other && Equals(other);
        }

        class TestDerivedClass1 : TestAbstract
        {
            public override int a { get; set; }
            public int b { get; set; }
        }

        class TestDerivedClass2 : TestAbstract
        {
            public override int a { get; set; }
            public int c { get; set; }
        }
    }
}
