using AppAsToy.EasyPack;
using FluentAssertions;
using System;
using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class PackerTests
    {
        [Fact]
        public void Pack_Struct()
        {
            var testStruct = new TestStruct
            (
                true,
                'a',
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
                11,
                DateTime.Now,
                DateTimeOffset.Now,
                TimeSpan.FromSeconds(1),
                Guid.NewGuid(),
                "abc",
                false,
                'b',
                12,
                13,
                14,
                15,
                16,
                17,
                18,
                19,
                20,
                21,
                22,
                DateTime.Now.AddDays(1),
                DateTimeOffset.Now.AddHours(1),
                TimeSpan.FromMinutes(-1),
                Guid.NewGuid(),
                "def",
                true,
                'c',
                23,
                24,
                25,
                26,
                27,
                28,
                29,
                30,
                31,
                32,
                33,
                DateTime.Now.AddMonths(1),
                DateTimeOffset.Now.AddMonths(-1),
                TimeSpan.FromHours(1),
                Guid.NewGuid(),
                "ghi",
                false,
                'd',
                34,
                35,
                36,
                37,
                38,
                39,
                40,
                41,
                42,
                43,
                44,
                DateTime.Now.AddMonths(-2),
                DateTimeOffset.Now.AddMonths(2),
                TimeSpan.FromHours(-1),
                Guid.NewGuid(),
                "jkl"
            );
            {
                using var pack = Packer.Pack(testStruct);
                Packer.Unpack<TestStruct>(pack.ToSpan()).Should().Be(testStruct);
            }
            {
                testStruct.publicSByte = 1;
                using var pack = Packer.Pack(testStruct);
                Packer.Unpack<TestStruct>(pack.ToSpan()).Should().NotBe(testStruct);
            }
        }

        struct TestStruct : IEquatable<TestStruct>
        {
            [LetsPack]
            bool privateBool;
            char privateChar;

            [LetsPack]
            sbyte privateSByte;
            short privateInt16;

            [LetsPack]
            int privateInt32;
            long privateInt64;

            [LetsPack]
            byte privateByte;
            ushort privateUInt16;

            [LetsPack]
            uint privateUInt32;
            ulong privateUInt64;

            [LetsPack]
            float privateFloat;
            double privateDouble;

            [LetsPack]
            decimal privateDecimal;
            DateTime privateDateTime;

            [LetsPack]
            DateTimeOffset privateDateTimeOffset;
            TimeSpan privateTimeSpan;

            [LetsPack]
            Guid privateGuid;
            string? privateString;

            [IgnorePack]
            public bool publicBool;
            public char publicChar;

            [IgnorePack]
            public sbyte publicSByte;
            public short publicInt16;

            [IgnorePack]
            public int publicInt32;
            public long publicInt64;

            [IgnorePack]
            public byte publicByte;
            public ushort publicUInt16;

            [IgnorePack]
            public uint publicUInt32;
            public ulong publicUInt64;

            [IgnorePack]
            public float publicFloat;
            public double publicDouble;

            [IgnorePack]
            public decimal publicDecimal;
            public DateTime publicDateTime;

            [IgnorePack]
            public DateTimeOffset publicDateTimeOffset;
            public TimeSpan publicTimeSpan;

            [IgnorePack]
            public Guid publicGuid;
            public string? publicString;

            public TestStruct(bool privateBool, char privateChar, sbyte privateSByte, short privateInt16, int privateInt32, long privateInt64, byte privateByte, ushort privateUInt16, uint privateUInt32, ulong privateUInt64, float privateFloat, double privateDouble, decimal privateDecimal, DateTime privateDateTime, DateTimeOffset privateDateTimeOffset, TimeSpan privateTimeSpan, Guid privateGuid, string? privateString, bool publicBool, char publicChar, sbyte publicSByte, short publicInt16, int publicInt32, long publicInt64, byte publicByte, ushort publicUInt16, uint publicUInt32, ulong publicUInt64, float publicFloat, double publicDouble, decimal publicDecimal, DateTime publicDateTime, DateTimeOffset publicDateTimeOffset, TimeSpan publicTimeSpan, Guid publicGuid, string? publicString, bool privateBool_property, char privateChar_property, sbyte privateSByte_property, short privateInt16_property, int privateInt32_property, long privateInt64_property, byte privateByte_property, ushort privateUInt16_property, uint privateUInt32_property, ulong privateUInt64_property, float privateFloat_property, double privateDouble_property, decimal privateDecimal_property, DateTime privateDateTime_property, DateTimeOffset privateDateTimeOffset_property, TimeSpan privateTimeSpan_property, Guid privateGuid_property, string? privateString_property, bool publicBool_property, char publicChar_property, sbyte publicSByte_property, short publicInt16_property, int publicInt32_property, long publicInt64_property, byte publicByte_property, ushort publicUInt16_property, uint publicUInt32_property, ulong publicUInt64_property, float publicFloat_property, double publicDouble_property, decimal publicDecimal_property, DateTime publicDateTime_property, DateTimeOffset publicDateTimeOffset_property, TimeSpan publicTimeSpan_property, Guid publicGuid_property, string? publicString_property)
            {
                this.privateBool = privateBool;
                this.privateChar = default;
                this.privateSByte = privateSByte;
                this.privateInt16 = default;
                this.privateInt32 = privateInt32;
                this.privateInt64 = default;
                this.privateByte = privateByte;
                this.privateUInt16 = default;
                this.privateUInt32 = privateUInt32;
                this.privateUInt64 = default;
                this.privateFloat = privateFloat;
                this.privateDouble = default;
                this.privateDecimal = privateDecimal;
                this.privateDateTime = default;
                this.privateDateTimeOffset = privateDateTimeOffset;
                this.privateTimeSpan = default;
                this.privateGuid = privateGuid;
                this.privateString = default;
                this.publicBool = default;
                this.publicChar = publicChar;
                this.publicSByte = default;
                this.publicInt16 = publicInt16;
                this.publicInt32 = default;
                this.publicInt64 = publicInt64;
                this.publicByte = default;
                this.publicUInt16 = publicUInt16;
                this.publicUInt32 = default;
                this.publicUInt64 = publicUInt64;
                this.publicFloat = default;
                this.publicDouble = publicDouble;
                this.publicDecimal = default;
                this.publicDateTime = publicDateTime;
                this.publicDateTimeOffset = default;
                this.publicTimeSpan = publicTimeSpan;
                this.publicGuid = default;
                this.publicString = publicString;
                this.privateBool_property = privateBool_property;
                this.privateChar_property = default;
                this.privateSByte_property = privateSByte_property;
                this.privateInt16_property = default;
                this.privateInt32_property = privateInt32_property;
                this.privateInt64_property = default;
                this.privateByte_property = privateByte_property;
                this.privateUInt16_property = default;
                this.privateUInt32_property = privateUInt32_property;
                this.privateUInt64_property = default;
                this.privateFloat_property = privateFloat_property;
                this.privateDouble_property = default;
                this.privateDecimal_property = privateDecimal_property;
                this.privateDateTime_property = default;
                this.privateDateTimeOffset_property = privateDateTimeOffset_property;
                this.privateTimeSpan_property = default;
                this.privateGuid_property = privateGuid_property;
                this.privateString_property = default;
                this.publicBool_property = default;
                this.publicChar_property = publicChar_property;
                this.publicSByte_property = default;
                this.publicInt16_property = publicInt16_property;
                this.publicInt32_property = default;
                this.publicInt64_property = publicInt64_property;
                this.publicByte_property = default;
                this.publicUInt16_property = publicUInt16_property;
                this.publicUInt32_property = default;
                this.publicUInt64_property = publicUInt64_property;
                this.publicFloat_property = default;
                this.publicDouble_property = publicDouble_property;
                this.publicDecimal_property = default;
                this.publicDateTime_property = publicDateTime_property;
                this.publicDateTimeOffset_property = default;
                this.publicTimeSpan_property = publicTimeSpan_property;
                this.publicGuid_property = default;
                this.publicString_property = publicString_property;
            }

            [LetsPack]
            bool privateBool_property { get; set; }
            char privateChar_property { get; set; }

            [LetsPack]
            sbyte privateSByte_property { get; set; }
            short privateInt16_property { get; set; }

            [LetsPack]
            int privateInt32_property { get; set; }
            long privateInt64_property { get; set; }

            [LetsPack]
            byte privateByte_property { get; set; }
            ushort privateUInt16_property { get; set; }

            [LetsPack]
            uint privateUInt32_property { get; set; }
            ulong privateUInt64_property { get; set; }

            [LetsPack]
            float privateFloat_property { get; set; }
            double privateDouble_property { get; set; }

            [LetsPack]
            decimal privateDecimal_property { get; set; }
            DateTime privateDateTime_property { get; set; }

            [LetsPack]
            DateTimeOffset privateDateTimeOffset_property { get; set; }
            TimeSpan privateTimeSpan_property { get; set; }

            [LetsPack]
            Guid privateGuid_property { get; set; }
            string? privateString_property { get; set; }

            [IgnorePack]
            public bool publicBool_property { get; set; }
            public char publicChar_property { get; set; }

            [IgnorePack]
            public sbyte publicSByte_property { get; set; }
            public short publicInt16_property { get; set; }

            [IgnorePack]
            public int publicInt32_property { get; set; }
            public long publicInt64_property { get; set; }

            [IgnorePack]
            public byte publicByte_property { get; set; }
            public ushort publicUInt16_property { get; set; }

            [IgnorePack]
            public uint publicUInt32_property { get; set; }
            public ulong publicUInt64_property { get; set; }

            [IgnorePack]
            public float publicFloat_property { get; set; }
            public double publicDouble_property { get; set; }

            [IgnorePack]
            public decimal publicDecimal_property { get; set; }
            public DateTime publicDateTime_property { get; set; }

            [IgnorePack]
            public DateTimeOffset publicDateTimeOffset_property { get; set; }
            public TimeSpan publicTimeSpan_property { get; set; }

            [IgnorePack]
            public Guid publicGuid_property { get; set; }
            public string? publicString_property { get; set; }

            public bool Equals(TestStruct other)
            {
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
                => obj is TestStruct testStruct && Equals(testStruct);
        }
    }


}
