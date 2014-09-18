using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Miracle.FileZilla.Api
{

#if DEBUG
    public class Hex
    {
        public static void Dump(TextWriter writer, byte[] bytes, int maxLength = int.MaxValue)
        {
            const int rowSize = 16;
            var hex = new StringBuilder(rowSize * 3);
            var ascii = new StringBuilder(rowSize);
            var offset = 0;

            foreach (var b in bytes.Take(maxLength))
            {
                hex.AppendFormat("{0:X2} ", b);
                ascii.Append(Char.IsControl((char)b) ? '.' : (char)b);

                if (ascii.Length == rowSize)
                {
                    writer.WriteLine("{0:X4} : {1}{2} {0}", offset, hex, ascii);
                    hex.Clear();
                    ascii.Clear();
                    offset += rowSize;
                }
            }

            if (ascii.Length != 0)
            {
                while (hex.Length < (rowSize*3)) hex.Append(' ');
                while (ascii.Length < rowSize) ascii.Append(' ');
                writer.WriteLine("{0:X4} : {1}{2} {0}", offset, hex, ascii);
            }

            if (bytes.Length > maxLength)
                writer.WriteLine("(More data... 0x{0:X}({0}))", bytes.Length);
        }

        public static string Dump(byte[] bytes, int maxLength = int.MaxValue)
        {
            var sw = new StringWriter();
            Dump(sw, bytes, maxLength);
            return sw.ToString();
        }
    }
#endif

}
