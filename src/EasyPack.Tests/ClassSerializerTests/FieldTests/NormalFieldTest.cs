using AppAsToy.EasyPack;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [TestMethod]
        public void Should_Serialize_Field()
        {
            using var pack = Packer.Pack(new TestField(1,2,3));
            var value = Packer.Unpack<TestField>(pack.ToSpan());
            value.PrivateField.Should().Be(0);
            value.ProtectedField.Should().Be(0);
            value.publicField.Should().Be(3);
        }

        class TestField
        {
            private int privateField;
            protected int protectedField;
            public int publicField;

            [IgnorePack] public int PrivateField => privateField;
            [IgnorePack] public int ProtectedField => protectedField;

            public TestField()
            {
            }

            public TestField(int privateField, int protectedField, int publicField)
            {
                this.privateField = privateField;
                this.protectedField = protectedField;
                this.publicField = publicField;
            }
        }
    }
}
