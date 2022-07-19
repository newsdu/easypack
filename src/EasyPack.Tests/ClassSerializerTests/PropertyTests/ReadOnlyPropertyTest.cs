using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Serialize_Property_ReadOnly()
        {
            using var pack = Packer.Pack(new TestProperty_ReadOnly(1));
            var value = Packer.Unpack<TestProperty_ReadOnly>(pack.ToSpan());
            value.publicProperty.Should().Be(1);
        }

        class TestProperty_ReadOnly
        {
            public int publicProperty { get; }

            public TestProperty_ReadOnly()
            {
            }

            [PackConstructor]
            public TestProperty_ReadOnly(int publicProperty)
            {
                this.publicProperty = publicProperty;
            }
        }
    }
}