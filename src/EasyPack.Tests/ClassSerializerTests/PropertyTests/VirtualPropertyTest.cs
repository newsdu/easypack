using AppAsToy.EasyPack;
using FluentAssertions;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [Fact]
        public void Should_Serialize_Property_Virtual()
        {
            using var pack = Packer.Pack(new TestProperty_Derived_From_Virtual(1, 2, 3));
            pack.ToArray().Length.Should().Be(12);
            var value = Packer.Unpack<TestProperty_Derived_From_Virtual>(pack.ToSpan());
            value.PublicProperty1.Should().Be(1);
            value.PublicProperty2.Should().Be(2);
            value.PublicProperty3.Should().Be(3);
            ((TestProperty_Virtual)value).PublicProperty1.Should().Be(1);
            ((TestProperty_Virtual)value).PublicProperty2.Should().Be(2);
            ((TestProperty_Virtual)value).PublicProperty3.Should().Be(3);
        }

        class TestProperty_Virtual
        {
            public virtual int PublicProperty1 { get; set; }
            public virtual int PublicProperty2 { get; set; }
            public virtual int PublicProperty3 { get; set; }
        }

        class TestProperty_Virtual_From_Virtual : TestProperty_Virtual
        {
            public override int PublicProperty2 { get; set; }
        }

        class TestProperty_Derived_From_Virtual : TestProperty_Virtual_From_Virtual
        {
            public override int PublicProperty3 { get; set; }

            public TestProperty_Derived_From_Virtual(int publicProperty1, int publicProperty2, int publicProperty3)
            {
                PublicProperty1 = publicProperty1;
                PublicProperty2 = publicProperty2;
                PublicProperty3 = publicProperty3;
            }

            public TestProperty_Derived_From_Virtual()
            {
            }
        }
    }
}
