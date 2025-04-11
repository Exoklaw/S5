using System;
using System.IO;
using System.Security.Cryptography;

namespace S5
{
    internal static class Windows
    {
        public static void Save(string filePath, byte[] data, Store.Key key)
        {
#pragma warning disable CA1416
            var encryptedData = ProtectedData.Protect(data, key.Value, DataProtectionScope.CurrentUser);
#pragma warning restore CA1416
            File.WriteAllBytes(filePath, encryptedData);
        }

        public static bool TryLoad(string filePath, Store.Key key, out byte[] data)
        {
            data = Array.Empty<byte>();

            if (!File.Exists(filePath)) return false;

            var encryptedData = File.ReadAllBytes(filePath);
            try
            {
#pragma warning disable CA1416
                data = ProtectedData.Unprotect(encryptedData, key.Value, DataProtectionScope.CurrentUser);
#pragma warning restore CA1416
            }
            catch (CryptographicException)
            {
                // Key was probably wrong.
                return false;
            }

            return true;
        }
    }
}
