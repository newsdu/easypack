using AppAsToy.EasyPack;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPack.Tests
{
    partial class ClassSerializerTests
    {
        [TestMethod]
        public void Should_Serialize_Property_Abstract()
        {
            using var pack = Packer.Pack(new TestProperty_Derived_From_Abstract(3));
            pack.ToArray().Length.Should().Be(4);
            var value = Packer.Unpack<TestProperty_Derived_From_Abstract>(pack.ToSpan());
            value.PublicProperty.Should().Be(3);
            ((TestProperty_Abstract)value).PublicProperty.Should().Be(3);
        }

        abstract class TestProperty_Abstract
        {
            public abstract int PublicProperty { get; set; }
        }

        class TestProperty_Derived_From_Abstract : TestProperty_Abstract
        {
            public override int PublicProperty { get; set; }

            public TestProperty_Derived_From_Abstract(int publicProperty)
            {
                PublicProperty = publicProperty;
            }

            public TestProperty_Derived_From_Abstract()
            {
            }
        }
    }
}
