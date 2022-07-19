using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    public partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Serialize_Property()
        {
            using var pack = Packer.Pack(new TestProperty(1, 2, 3));
            var value = Packer.Unpack<TestProperty>(pack.ToSpan());
            value.PrivateProperty.Should().Be(0);
            value.ProtectedProperty.Should().Be(0);
            value.publicProperty.Should().Be(3);
        }

        class TestProperty
        {
            private int privateProperty { get; set; }
            protected int protectedProperty { get; set; }
            public int publicProperty { get; set; }

            [IgnorePack] public int PrivateProperty => privateProperty;
            [IgnorePack] public int ProtectedProperty => protectedProperty;

            public TestProperty()
            {
            }

            public TestProperty(int privateProperty, int protectedProperty, int publicProperty)
            {
                this.privateProperty = privateProperty;
                this.protectedProperty = protectedProperty;
                this.publicProperty = publicProperty;
            }
        }
    }
}
