using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Base class used for communication with a socket
    /// </summary>
    public class SocketCommunication : IDisposable
    {
        private readonly IPEndPoint _ipe;
        private Socket _socket;
        /// <summary>
        /// Default buffer size. Set BufferSize property to override default. 
        /// </summary>
        public const int DefaultBufferSize = 10*1024*1024;
        byte[] _buffer;
        private int _bufferSize;
#if DEBUG
        private int _maxBufferUsed;
#endif

        /// <summary>
        /// Construct admin socket on specific IP and port.
        /// </summary>
        /// <param name="address">IP address of filezilla server.</param>
        /// <param name="port">Admin port as specified when FileZilla server were installed</param>
        protected SocketCommunication(IPAddress address, int port)
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose object
        /// </summary>
        /// <param name="disposing">Dispose unmanaged resources?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
            }
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
#if DEBUG
                var percent = _maxBufferUsed/(double)_bufferSize*100;
                Console.WriteLine("Disconnected. A maximum of {0}({1}%) buffer bytes were used.\r\n", _maxBufferUsed, Math.Round(percent,1));
#endif
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

            LogData("Send", data);
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

           int bytesRec = _socket.Receive(_buffer);
#if DEBUG
            _maxBufferUsed = Math.Max(_maxBufferUsed,bytesRec);
#endif

            if(bytesRec == BufferSize)
                throw new ApiException("Buffer too small. Increase BufferSize parameter");
            var data = _buffer.Take(bytesRec).ToArray();
            LogData("Receive", data);
            return data;
        }

        /// <summary>
        /// Log for debugging purposes
        /// </summary>
        public TextWriter Log { get; set; }

        /// <summary>
        /// Write data to log as hex dump. (Set Log parameter to activate)
        /// </summary>
        /// <param name="text">Label to write before hex dump</param>
        /// <param name="bytes">Data bytes to hex dump</param>
        protected void LogData(string text, byte[] bytes)
        {
            if (Log != null)
            {
                Log.WriteLine("{0}: {1}", DateTime.Now.TimeOfDay, text);
                Hex.Dump(Log, bytes, 1024);
            }
        }
    }
}