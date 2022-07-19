using AppAsToy.EasyPack;
using FluentAssertions;
using System;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class PackerTests
    {
        [Fact]
        public void Pack_Class()
        {
            var testClass = new TestClass();
            {
                using var pack = Packer.Pack(testClass);
                Packer.Unpack<TestClass>(pack.ToSpan()).Should().Be(testClass);
            }
            {
                testClass.publicSByte = 1;
                using var pack = Packer.Pack(testClass);
                Packer.Unpack<TestClass>(pack.ToSpan()).Should().NotBe(testClass);
            }
        }

        class TestClass : IEquatable<TestClass>
        {
            [LetsPack]
            bool privateBool = true;
            char privateChar;

            [LetsPack]
            sbyte privateSByte = 1;
            short privateInt16;

            [LetsPack]
            int privateInt32 = 2;
            long privateInt64;

            [LetsPack]
            byte privateByte = 3;
            ushort privateUInt16;

            [LetsPack]
            uint privateUInt32 = 4;
            ulong privateUInt64;

            [LetsPack]
            float privateFloat = 5;
            double privateDouble;

            [LetsPack]
            decimal privateDecimal = 6;
            DateTime privateDateTime;

            [LetsPack]
            DateTimeOffset privateDateTimeOffset = DateTimeOffset.Now;
            TimeSpan privateTimeSpan;

            [LetsPack]
            Guid privateGuid = Guid.NewGuid();
            string? privateString;

            [IgnorePack]
            public bool publicBool;
            public char publicChar = 'a';

            [IgnorePack]
            public sbyte publicSByte;
            public short publicInt16 = 7;

            [IgnorePack]
            public int publicInt32;
            public long publicInt64 = 8;

            [IgnorePack]
            public byte publicByte;
            public ushort publicUInt16 = 9;

            [IgnorePack]
            public uint publicUInt32;
            public ulong publicUInt64 = 10;

            [IgnorePack]
            public float publicFloat;
            public double publicDouble = 11;

            [IgnorePack]
            public decimal publicDecimal;
            public DateTime publicDateTime = DateTime.Now.AddDays(1);

            [IgnorePack]
            public DateTimeOffset publicDateTimeOffset;
            public TimeSpan publicTimeSpan = TimeSpan.FromHours(1);

            [IgnorePack]
            public Guid publicGuid;
            public string? publicString = "abc";

            [LetsPack]
            bool privateBool_property { get; set; } = false;
            char privateChar_property { get; set; }

            [LetsPack]
            sbyte privateSByte_property { get; set; } = 12;
            short privateInt16_property { get; set; }

            [LetsPack]
            int privateInt32_property { get; set; } = 13;
            long privateInt64_property { get; set; }

            [LetsPack]
            byte privateByte_property { get; set; } = 14;
            ushort privateUInt16_property { get; set; }

            [LetsPack]
            uint privateUInt32_property { get; set; } = 15;
            ulong privateUInt64_property { get; set; }

            [LetsPack]
            float privateFloat_property { get; set; } = 16;
            double privateDouble_property { get; set; }

            [LetsPack]
            decimal privateDecimal_property { get; set; } = 17;
            DateTime privateDateTime_property { get; set; }

            [LetsPack]
            DateTimeOffset privateDateTimeOffset_property { get; set; } = DateTimeOffset.Now.AddHours(-1);
            TimeSpan privateTimeSpan_property { get; set; }

            [LetsPack]
            Guid privateGuid_property { get; set; } = Guid.NewGuid();
            string? privateString_property { get; set; }

            [IgnorePack]
            public bool publicBool_property { get; set; }
            public char publicChar_property { get; set; } = 'b';

            [IgnorePack]
            public sbyte publicSByte_property { get; set; }
            public short publicInt16_property { get; set; } = 18;

            [IgnorePack]
            public int publicInt32_property { get; set; }
            public long publicInt64_property { get; set; } = 19;

            [IgnorePack]
            public byte publicByte_property { get; set; }
            public ushort publicUInt16_property { get; set; } = 20;

            [IgnorePack]
            public uint publicUInt32_property { get; set; }
            public ulong publicUInt64_property { get; set; } = 21;

            [IgnorePack]
            public float publicFloat_property { get; set; }
            public double publicDouble_property { get; set; } = 22;

            [IgnorePack]
            public decimal publicDecimal_property { get; set; }
            public DateTime publicDateTime_property { get; set; } = DateTime.Now.AddMinutes(-1);

            [IgnorePack]
            public DateTimeOffset publicDateTimeOffset_property { get; set; }
            public TimeSpan publicTimeSpan_property { get; set; } = TimeSpan.FromSeconds(-1);

            [IgnorePack]
            public Guid publicGuid_property { get; set; }
            public string? publicString_property { get; set; } = "def";

            public bool Equals(TestClass? other)
            {
                if (this == null && other == null)
                    return true;

                if (this == null || other == null)
                    return false;

                return privateBool == other.privateBool &&
                       privateChar == other.privateChar &&
                       privateSByte == other.privateSByte &&
                       privateInt16 == other.privateInt16 &&
                       privateInt32 == other.privateInt32 &&
                       privateInt64 == other.privateInt64 &&
                       privateByte == other.privateByte &&
                       privateUInt16 == other.privateUInt16 &&
                       privateUInt32 == other.privateUInt32 &&
                       privateUInt64 == other.privateUInt64 &&
                       privateFloat == other.privateFloat &&
                       privateDouble == other.privateDouble &&
                       privateDecimal == other.privateDecimal &&
                       privateDateTime == other.privateDateTime &&
                       privateDateTimeOffset == other.privateDateTimeOffset &&
                       privateTimeSpan == other.privateTimeSpan &&
                       privateGuid == other.privateGuid &&
                       privateString == other.privateString &&

                       publicBool == other.publicBool &&
                       publicChar == other.publicChar &&
                       publicSByte == other.publicSByte &&
                       publicInt16 == other.publicInt16 &&
                       publicInt32 == other.publicInt32 &&
                       publicInt64 == other.publicInt64 &&
                       publicByte == other.publicByte &&
                       publicUInt16 == other.publicUInt16 &&
                       publicUInt32 == other.publicUInt32 &&
                       publicUInt64 == other.publicUInt64 &&
                       publicFloat == other.publicFloat &&
                       publicDouble == other.publicDouble &&
                       publicDecimal == other.publicDecimal &&
                       publicDateTime == other.publicDateTime &&
                       publicDateTimeOffset == other.publicDateTimeOffset &&
                       publicTimeSpan == other.publicTimeSpan &&
                       publicGuid == other.publicGuid &&
                       publicString == other.publicString &&

                       privateBool_property == other.privateBool_property &&
                       privateChar_property == other.privateChar_property &&
                       privateSByte_property == other.privateSByte_property &&
                       privateInt16_property == other.privateInt16_property &&
                       privateInt32_property == other.privateInt32_property &&
                       privateInt64_property == other.privateInt64_property &&
                       privateByte_property == other.privateByte_property &&
                       privateUInt16_property == other.privateUInt16_property &&
                       privateUInt32_property == other.privateUInt32_property &&
                       privateUInt64_property == other.privateUInt64_property &&
                       privateFloat_property == other.privateFloat_property &&
                       privateDouble_property == other.privateDouble_property &&
                       privateDecimal_property == other.privateDecimal_property &&
                       privateDateTime_property == other.privateDateTime_property &&
                       privateDateTimeOffset_property == other.privateDateTimeOffset_property &&
                       privateTimeSpan_property == other.privateTimeSpan_property &&
                       privateGuid_property == other.privateGuid_property &&
                       privateString_property == other.privateString_property &&

                       publicBool_property == other.publicBool_property &&
                       publicChar_property == other.publicChar_property &&
                       publicSByte_property == other.publicSByte_property &&
                       publicInt16_property == other.publicInt16_property &&
                       publicInt32_property == other.publicInt32_property &&
                       publicInt64_property == other.publicInt64_property &&
                       publicByte_property == other.publicByte_property &&
                       publicUInt16_property == other.publicUInt16_property &&
                       publicUInt32_property == other.publicUInt32_property &&
                       publicUInt64_property == other.publicUInt64_property &&
                       publicFloat_property == other.publicFloat_property &&
                       publicDouble_property == other.publicDouble_property &&
                       publicDecimal_property == other.publicDecimal_property &&
                       publicDateTime_property == other.publicDateTime_property &&
                       publicDateTimeOffset_property == other.publicDateTimeOffset_property &&
                       publicTimeSpan_property == other.publicTimeSpan_property &&
                       publicGuid_property == other.publicGuid_property &&
                       publicString_property == other.publicString_property;
            }

            public override bool Equals(object? obj) 
                => obj is TestClass testClass && Equals(testClass);
        }
    }
}
