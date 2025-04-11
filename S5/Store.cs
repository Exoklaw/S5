using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using S5.Utilities;

namespace S5
{
    public static class Store
    {
        public static void Save(string filePath, byte[] data, Key key)
        {
            if (data.Length == 0) throw new ArgumentException("Data cannot be empty", nameof(data));

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Windows.Save(filePath, data, key);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Linux.Save(filePath, data, key);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static bool TryLoad(string filePath, Key key, out byte[] data)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Windows.TryLoad(filePath, key, out data);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Linux.TryLoad(filePath, key, out data);
            }

            throw new NotSupportedException();
        }

        public class Key
        {
            public byte[] Value { get; }

            public static Key Generate()
            {
                using (var aes = Aes.Create())
                {
                    aes.GenerateKey();
                    return new Key(aes.Key);
                }
            }

            public static Key DeriveFrom(string password)
            {
                var passwordBytes = Encoding.Unicode.GetBytes(password);
                return DeriveFrom(passwordBytes);
            }

            public static Key DeriveFrom(byte[] data)
            {
                var hashedData = Sha512Utils.HashData(data);

                using (var aes = Aes.Create())
                {
                    var maxKeyLength = aes.LegalKeySizes.Max(k => k.MaxSize) / 8;
                    var truncatedHashedData = hashedData.Take(maxKeyLength).ToArray();
                    return new Key(truncatedHashedData);
                }
            }

            public Key(byte[] value)
            {
                Value = value;
            }
        }
    }
}
