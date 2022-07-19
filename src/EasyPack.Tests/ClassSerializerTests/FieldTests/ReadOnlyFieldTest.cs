using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Serialize_Field_ReadOnly()
        {
            using var pack = Packer.Pack(new TestField_ReadOnly(1, 2, 3));
            var value = Packer.Unpack<TestField_ReadOnly>(pack.ToSpan());
            value.PrivateField.Should().Be(0);
            value.ProtectedField.Should().Be(0);
            value.publicField.Should().Be(3);
        }

        class TestField_ReadOnly
        {
            private readonly int privateField;
            protected readonly int protectedField;
            public readonly int publicField;

            [IgnorePack] public int PrivateField => privateField;
            [IgnorePack] public int ProtectedField => protectedField;

            public TestField_ReadOnly() { }

            public TestField_ReadOnly(int privateField, int protectedField, int publicField)
            {
                this.privateField = privateField;
                this.protectedField = protectedField;
                this.publicField = publicField;
            }

            [PackConstructor]
            public TestField_ReadOnly(int publicField)
            {
                this.publicField = publicField;
            }
        }
    }
}
