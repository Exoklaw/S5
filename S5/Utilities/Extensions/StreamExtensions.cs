using System.IO;

namespace S5.Utilities.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static bool TryReadExactly(this Stream stream, int count, out byte[] data)
        {
            data = new byte[count];
            var totalNumBytesRead = 0;

            while (totalNumBytesRead < count)
            {
                var numBytesRead = stream.Read(data, totalNumBytesRead, count - totalNumBytesRead);
                if (numBytesRead <= 0) return false;

                totalNumBytesRead += numBytesRead;
            }

            return true;
        }
    }
}
