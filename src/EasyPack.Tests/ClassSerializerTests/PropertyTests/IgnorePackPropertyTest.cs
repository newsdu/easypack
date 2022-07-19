using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Serialize_Property_IgnorePack()
        {
            using var pack = Packer.Pack(new TestProperty_IgnorePack(1, 2, 3));
            var value = Packer.Unpack<TestProperty_IgnorePack>(pack.ToSpan());
            value.PrivateProperty.Should().Be(0);
            value.ProtectedProperty.Should().Be(0);
            value.publicProperty.Should().Be(0);
        }

        class TestProperty_IgnorePack
        {
            [IgnorePack] private int privateProperty { get; set; }
            [IgnorePack] protected int protectedProperty { get; set; }
            [IgnorePack] public int publicProperty { get; set; }

            [IgnorePack] public int PrivateProperty => privateProperty;
            [IgnorePack] public int ProtectedProperty => protectedProperty;

            public TestProperty_IgnorePack()
            {
            }

            public TestProperty_IgnorePack(int privateProperty, int protectedProperty, int publicProperty)
            {
                this.privateProperty = privateProperty;
                this.protectedProperty = protectedProperty;
                this.publicProperty = publicProperty;
            }
        }
    }
}