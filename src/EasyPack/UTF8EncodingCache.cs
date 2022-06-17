using System.Text;

namespace AppAsToy.EasyPack
{
    internal static class UTF8EncodingCache
    {
        public static readonly UTF8Encoding Encoding = new UTF8Encoding(false);
    }
}
