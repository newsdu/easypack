using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Serialize_Field_LetsPack()
        {
            using var pack = Packer.Pack(new TestField_LetsPack(1, 2, 3));
            var value = Packer.Unpack<TestField_LetsPack>(pack.ToSpan());
            value.PrivateField.Should().Be(1);
            value.ProtectedField.Should().Be(2);
            value.publicField.Should().Be(3);
        }

        class TestField_LetsPack
        {
            [LetsPack] private int privateField;
            [LetsPack] protected int protectedField;
            [LetsPack] public int publicField;

            [IgnorePack] public int PrivateField => privateField;
            [IgnorePack] public int ProtectedField => protectedField;

            public TestField_LetsPack()
            {
            }

            public TestField_LetsPack(int privateField, int protectedField, int publicField)
            {
                this.privateField = privateField;
                this.protectedField = protectedField;
                this.publicField = publicField;
            }
        }
    }
}
