using Xunit;

namespace AppAsToy.EasyPack.Tests
{
    partial class PackerTests
    {
        enum _byte : byte { _1 = 1 }
        enum _ushort : ushort { _1 = 1 }
        enum _uint : uint { _1 = 1 }
        enum _ulong : ulong { _1 = 1 }
        enum _sbyte : sbyte { _1 = -1 }
        enum _short : short { _1 = -1 }
        enum _int : int { _1 = -1 }
        enum _long : long { _1 = -1 }

        [Fact]
        public void Pack_Enum()
        {
            Test_Pack_Unpack(_byte._1);
            Test_Pack_Unpack(_ushort._1);
            Test_Pack_Unpack(_uint._1);
            Test_Pack_Unpack(_ulong._1);
            Test_Pack_Unpack(_sbyte._1);
            Test_Pack_Unpack(_short._1);
            Test_Pack_Unpack(_int._1);
            Test_Pack_Unpack(_long._1);
        }
    }
}
