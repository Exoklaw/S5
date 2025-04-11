using System.Security.Cryptography;

namespace S5.Utilities
{
    public static class Sha512Utils
    {
        public static byte[] HashData(byte[] data)
        {
#if NET8_0_OR_GREATER
            return SHA512.HashData(data);
#else
            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(data);
            }
#endif
        }
    }
}
