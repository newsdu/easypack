using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Not_Serialize_Property_WriteOnly()
        {
            using var pack = Packer.Pack(new TestProperty_WriteOnly(3));
            var value = Packer.Unpack<TestProperty_WriteOnly>(pack.ToSpan());
            value.PrivateProperty.Should().Be(0);
        }

        class TestProperty_WriteOnly
        {
            private int privateProperty;
            public int PublicProperty
            {
                set => privateProperty = value;
            }

            [IgnorePack] public int PrivateProperty => privateProperty;

            public TestProperty_WriteOnly() { }

            public TestProperty_WriteOnly(int publicProperty)
            {
                PublicProperty = publicProperty;
            }
        }
    }
}
