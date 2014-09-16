using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Base class used for low level communication with FileZilla admin API
    /// Note! BufferSize must be set to a value large enough to handle incomming data from FileZilla server.
    /// </summary>
    public class AdminSocket : IDisposable
    {
        private readonly IPEndPoint _ipe;
        private Socket _socket;
        /// <summary>
        /// Default buffer size. Set BufferSize property to override default. 
        /// </summary>
        public const int DefaultBufferSize = 10*1024*1024;
        byte[] _buffer;
        private int _bufferSize;

        /// <summary>
        /// Construct admin socket on specific IP and port.
        /// </summary>
        /// <param name="address">IP address of filezilla server.</param>
        /// <param name="port">Admin port as specified when FileZilla server were installed</param>
        protected AdminSocket(IPAddress address, int port)
        {
            _ipe = new IPEndPoint(address, port);
            _socket = new Socket(_ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            BufferSize = DefaultBufferSize;
        }

        /// <summary>
        /// Implementation of IDisposable interface
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// The size of the receiving buffer. If this is too small, then returning data from server is discarded, and an exception is thrown.   
        /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
            set
            {
                _bufferSize = value;
                if (IsConnected)
                    _buffer = new byte[BufferSize];
            }
        }

        /// <summary>
        /// Connect socket to endpoint
        /// </summary>
        protected void Connect()
        {
            _buffer = new byte[BufferSize];
            _socket.Connect(_ipe);
        }

        /// <summary>
        /// Disconnect socket
        /// </summary>
        protected void Disconnect()
        {
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
                _buffer = null;
            }
        }

        /// <summary>
        /// Check if socket is connected
        /// </summary>
        public bool IsConnected
        {
            get { return _socket != null && _socket.Connected; }
        }

        /// <summary>
        /// Send data to socket
        /// </summary>
        /// <param name="data">Binary data to send</param>
        protected void Send(byte[] data)
        {

            if (!IsConnected) throw new ApiException("Not connected");
#if DEBUG
            if (Trace)
                TraceData(DateTime.Now + " Send:", data);
#endif
            _socket.Send(data);
        }

        /// <summary>
        /// Receive data from socket.
        /// </summary>
        /// <exception cref="ApiException">
        ///     if BufferSize is too small 
        /// </exception>
        /// <returns>Data from socket</returns>
        protected byte[] Receive()
        {
            if (!IsConnected) throw new ApiException("Not connected");

            // Receive the response from the remote device.
            int bytesRec = _socket.Receive(_buffer);
            if(bytesRec == BufferSize)
                throw new ApiException("Buffer too small. Increase BufferSize parameter");
            var data = _buffer.Take(bytesRec).ToArray();
#if DEBUG
            if (Trace)
                TraceData(DateTime.Now + " Receive:", data);
#endif
            return data;
        }

#if DEBUG
        public bool Trace { get; set; }
        private static void TraceData(string label, IEnumerable<byte> bytes)
        {
            var hex = new StringBuilder(48);
            var ascii = new StringBuilder(16);
            int offset = 0;
            const int rowSize = 16;

            Debug.Print(label);
            foreach (var b in bytes)
            {
                hex.AppendFormat("{0:X2} ", b);
                ascii.Append(b > 31 ? (char)b : '.');

                if (ascii.Length == rowSize)
                {
                    Debug.Print("{0:X4} : {1}{2} {0}", offset, hex, ascii);
                    hex.Clear();
                    ascii.Clear();
                    offset += rowSize;
                }
            }

            if (ascii.Length != 0)
            {
                while (hex.Length < 48) hex.Append(' ');
                while (ascii.Length < 16) ascii.Append(' ');
                Debug.Print("{0:X4} : {1}{2} {0}", offset, hex, ascii);
            }
        }
#endif
    }
}