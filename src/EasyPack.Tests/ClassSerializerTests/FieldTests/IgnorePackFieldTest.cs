using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Serialize_Field_IgnorePack()
        {
            using var pack = Packer.Pack(new TestField_IgnorePack(1, 2, 3));
            var value = Packer.Unpack<TestField_IgnorePack>(pack.ToSpan());
            value.PrivateField.Should().Be(0);
            value.ProtectedField.Should().Be(0);
            value.publicField.Should().Be(0);
        }

        class TestField_IgnorePack
        {
            [IgnorePack] private int privateField;
            [IgnorePack] protected int protectedField;
            [IgnorePack] public int publicField;

            [IgnorePack] public int PrivateField => privateField;
            [IgnorePack] public int ProtectedField => protectedField;

            public TestField_IgnorePack()
            {
            }

            public TestField_IgnorePack(int privateField, int protectedField, int publicField)
            {
                this.privateField = privateField;
                this.protectedField = protectedField;
                this.publicField = publicField;
            }
        }
    }
}
