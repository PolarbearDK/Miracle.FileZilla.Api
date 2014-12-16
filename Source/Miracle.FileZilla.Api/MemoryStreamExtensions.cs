using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Extension methods for memorystream
    /// </summary>
    static class MemoryStreamExtensions
    {
        /// <summary>
        /// Append data to end of memorystream while retaining position
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="data"></param>
        public static void Append(this MemoryStream memoryStream, byte[] data)
        {
            var position = memoryStream.Position;
            memoryStream.Position = memoryStream.Length;
            memoryStream.Write(data,0,data.Length);
            memoryStream.Position = position;
        }
    }
}
