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
        /// Initial buffer size.
        /// </summary>
        private const int DefaultBufferSize = ushort.MaxValue;
        byte[] _buffer;

        /// <summary>
        /// Construct admin socket on specific IP and port.
        /// </summary>
        /// <param name="address">IP address of filezilla server.</param>
        /// <param name="port">Admin port as specified when FileZilla server were installed</param>
        protected SocketCommunication(IPAddress address, int port)
        {
            _ipe = new IPEndPoint(address, port);
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
        /// Connect socket to endpoint
        /// </summary>
        protected void Connect()
        {
            if (IsConnected) throw new ApiException("Already connected");
            Disconnect();

            _buffer = new byte[DefaultBufferSize];
            _socket = new Socket(_ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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

            int bytesRec = _socket.Receive(_buffer, 0, _buffer.Length, SocketFlags.None);
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