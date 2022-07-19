using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Serialize_Property_InitOnly()
        {
            {
                using var pack = Packer.Pack(new TestProperty_InitOnly(1));
                var value = Packer.Unpack<TestProperty_InitOnly>(pack.ToSpan());
                value.publicProperty.Should().Be(1);
            }
            {
                using var pack = Packer.Pack(new TestProperty_InitOnly_With_PackConstructor(1));
                var value = Packer.Unpack<TestProperty_InitOnly_With_PackConstructor>(pack.ToSpan());
                value.publicProperty.Should().Be(1);
            }
        }

        

        class TestProperty_InitOnly
        {
            public int publicProperty { get; init; }

            public TestProperty_InitOnly()
            {
            }

            public TestProperty_InitOnly(int publicProperty)
            {
                this.publicProperty = publicProperty;
            }
        }

        class TestProperty_InitOnly_With_PackConstructor
        {
            public int publicProperty { get; init; }

            [PackConstructor]
            public TestProperty_InitOnly_With_PackConstructor(int publicProperty)
            {
                this.publicProperty = publicProperty;
            }
        }
    }
}