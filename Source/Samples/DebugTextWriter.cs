using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Miracle.FileZilla.Api.Samples
{
    public class DebugTextWriter : StreamWriter
    {
        public DebugTextWriter()
            : base(new DebugOutStream(), Encoding.Unicode, 1024)
        {
            AutoFlush = true;
        }

        class DebugOutStream : Stream
        {
            public override void Write(byte[] buffer, int offset, int count)
            {
                Debug.Write(Encoding.Unicode.GetString(buffer, offset, count));
            }

            public override bool CanRead { get { return false; } }
            public override bool CanSeek { get { return false; } }
            public override bool CanWrite { get { return true; } }
            public override void Flush() { Debug.Flush(); }
            public override long Length { get { throw new InvalidOperationException(); } }
            public override int Read(byte[] buffer, int offset, int count) { throw new InvalidOperationException(); }
            public override long Seek(long offset, SeekOrigin origin) { throw new InvalidOperationException(); }
            public override void SetLength(long value) { throw new InvalidOperationException(); }
            public override long Position
            {
                get { throw new InvalidOperationException(); }
                set { throw new InvalidOperationException(); }
            }
        };
    }
}