using System;
using System.Security.Cryptography;
using Mono.Unix;
using S5.Utilities.Extensions;

namespace S5
{
    internal static class Linux
    {
        public static void Save(string filePath, byte[] data, Store.Key key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key.Value;

                var fileInfo = new UnixFileInfo(filePath);
                using (var fileStream = fileInfo.Create())
                {
                    fileInfo.FileAccessPermissions = FileAccessPermissions.UserRead | FileAccessPermissions.UserWrite;

                    var iv = aes.IV;
                    fileStream.Write(iv, 0, iv.Length);

                    using (var cryptoStream =
                           new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                    }
                }
            }
        }

        public static bool TryLoad(string filePath, Store.Key key, out byte[] data)
        {
            data = Array.Empty<byte>();

            var fileInfo = new UnixFileInfo(filePath);
            if (!fileInfo.Exists) return false;

            using (var aes = Aes.Create())
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    if (!fileStream.TryReadExactly(aes.IV.Length, out var iv)) return false;

                    using (var cryptoStream = new CryptoStream(
                               fileStream,
                               aes.CreateDecryptor(key.Value, iv),
                               CryptoStreamMode.Read))
                    {
                        try
                        {
                            data = cryptoStream.ReadAllBytes();
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
        }
    }
}
