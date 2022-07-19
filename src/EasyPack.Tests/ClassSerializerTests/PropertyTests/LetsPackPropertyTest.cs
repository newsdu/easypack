using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Serialize_Property_LetsPack()
        {
            using var pack = Packer.Pack(new TestProperty_LetsPack(1, 2, 3));
            var value = Packer.Unpack<TestProperty_LetsPack>(pack.ToSpan());
            value.PrivateProperty.Should().Be(1);
            value.ProtectedProperty.Should().Be(2);
            value.publicProperty.Should().Be(3);
        }

        class TestProperty_LetsPack
        {
            [LetsPack] private int privateProperty { get; set; }
            [LetsPack] protected int protectedProperty { get; set; }
            [LetsPack] public int publicProperty { get; set; }

            [IgnorePack] public int PrivateProperty => privateProperty;
            [IgnorePack] public int ProtectedProperty => protectedProperty;

            public TestProperty_LetsPack()
            {
            }

            public TestProperty_LetsPack(int privateProperty, int protectedProperty, int publicProperty)
            {
                this.privateProperty = privateProperty;
                this.protectedProperty = protectedProperty;
                this.publicProperty = publicProperty;
            }
        }
    }
}
