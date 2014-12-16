using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Class used for low level communication with FileZilla admin interface
    /// Note! BufferSize must be set to a value large enough to handle incomming data from FileZilla server.
    /// </summary>
    public class FileZillaServerProtocol : SocketCommunication
    {
        /// <summary>
        /// Versions used to develop this API
        /// </summary>
        public readonly int[] SupportedProtocolVersions =
        {
            ProtocolVersions.Initial,
            ProtocolVersions.User16M
        };
        /// <summary>
        /// Defailt IP
        /// </summary>
        public const string DefaultIp = "127.0.0.1";
        /// <summary>
        /// Default port
        /// </summary>
        public const int DefaultPort = 14147;
        /// <summary>
        /// Server version. Populated by Connect method.
        /// </summary>
        public int ServerVersion { get; private set; }

        /// <summary>
        /// Protocol version. Populated by Connect method.
        /// </summary>
        public int ProtocolVersion { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">IP address of FileZilla server</param>
        /// <param name="port">Port number that admin interface is listening to</param>
        public FileZillaServerProtocol(IPAddress address, int port)
            : base(address, port)
        {
        }

        /// <summary>
        /// Send command to FileZilla
        /// </summary>
        /// <param name="messageType">Type of FileZilla message to send</param>
        public void SendCommand(MessageType messageType)
        {
            SendCommand(messageType, new byte[] { });
        }

        /// <summary>
        /// Send command with byte data to FileZilla
        /// </summary>
        /// <param name="messageType">Type of FileZilla message to send</param>
        /// <param name="data">Byte data to send with command</param>
        public void SendCommand(MessageType messageType, byte data)
        {
            SendCommand(messageType, new[] { data });
        }

        /// <summary>
        /// Send command with bytes to FileZilla
        /// </summary>
        /// <param name="messageType">Type of FileZilla message to send</param>
        /// <param name="data">Byte data to send with command</param>
        public void SendCommand(MessageType messageType, byte[] data)
        {
            SendCommand(messageType, writer => writer.Write(data));
        }

        /// <summary>
        /// Send command to FileZilla using an dataAction method
        /// </summary>
        /// <param name="messageType">Type of FileZilla message to send</param>
        /// <param name="dataAction">Method that writes the raw data to the command. Length is automatically generated for the data written by dataAction</param>
        public void SendCommand(MessageType messageType, Action<BinaryWriter> dataAction)
        {
            var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream))
            {
                var cmd = (byte)(((int)MessageOrigin.ClientRequest) | ((byte)messageType << 2));
                writer.Write(cmd);
                writer.WriteLength(dataAction);
            }

            Send(stream.ToArray());
        }

        /// <summary>
        /// Receive FileZilla of specific message type, and expect an empty (length=0) response.
        /// </summary>
        /// <param name="messageType">Type of FileZilla message to receive</param>
        /// <exception cref="ProtocolException">If length other than 0</exception>
        public void Receive(MessageType messageType)
        {
            var message = ReceiveMessage(messageType);
            if (message.RawData.Length != 0)
                throw new ProtocolException("Expected message with length 0, actual " + message.RawData.Length);
        }

        /// <summary>
        /// Receive FileZilla of specific message type, and expect a message body of generic type T.
        /// </summary>
        /// <param name="messageType">Type of FileZilla message to receive</param>
        /// <typeparam name="T">The expected budy type</typeparam>
        /// <returns>The body of the message</returns>
        public T Receive<T>(MessageType messageType)
        {
            var message = ReceiveMessage(messageType);
            return (T)message.Body;
        }

        private void Receive(Action<BinaryReader> action)
        {
            var data = Receive();
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                action(reader);
            }
        }

        /// <summary>
        /// Receive specific message matching MessageType. Note! This filters away all ServerMessages to get to that particular message.
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns>FileZilla message matching MessageType</returns>
        public FileZillaMessage ReceiveMessage(MessageType messageType)
        {
            var messages = ReceiveMessages();
            FileZillaMessage fileZillaMessage = null;
            foreach (FileZillaMessage check in messages)
            {
                if (check.MessageOrigin == MessageOrigin.ServerReply && check.MessageType == messageType)
                {
                    if (fileZillaMessage != null)
                        throw new ProtocolException("Multiple commands matched");
                    fileZillaMessage = check;
                }
#if DEBUG
                else
                {
                    LogData(string.Format("Message ignored: {0}/{1} with length {2}", check.MessageOrigin, check.MessageType, check.RawData.Length), check.RawData);
                }
#endif
            }

            if (fileZillaMessage == null)
            {
                // All server messages? Poll again for the desired reply 
                if (messages.All(x => x.MessageOrigin == MessageOrigin.ServerMessage))
                    // Jedi mind trick: This is not the message you are looking for
                    return ReceiveMessage(messageType);

                throw ProtocolException.Create(messageType, messages);
            }

            return fileZillaMessage;
        }

        /// <summary>
        /// Get parsed messages from FileZilla admin interface
        /// </summary>
        /// <returns></returns>
        public FileZillaMessage[] ReceiveMessages()
        {
            var data = Receive();
            var list = new List<FileZillaMessage>();
            var memoryStream = new MemoryStream();
            memoryStream.Append(data);
            using (var reader = new BinaryReader(memoryStream))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var b = reader.ReadByte();
                    var count = reader.ReadInt32();

                    while ((reader.BaseStream.Length - reader.BaseStream.Position) < count)
                    {
                        var buffer = Receive();
                        memoryStream.Append(buffer);
                    }

                    byte[] payload = reader.ReadBytes(count);
                    var message = new FileZillaMessage((MessageOrigin)(b & 0x3), (MessageType)(b >> 2), payload, ProtocolVersion);
                    list.Add(message);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Connect to FileZilla admin interface 
        /// </summary>
        /// <param name="password">FileZilla admin password</param>
        public void Connect(string password)
        {
            Authentication authentication = null;
            Connect();

            Receive(reader =>
            {
                reader.Verify("FZS");

                ServerVersion = reader.ReadLength(reader.ReadBigEndianInt16(), x => x.ReadInt32());
                ProtocolVersion = reader.ReadLength(reader.ReadBigEndianInt16(), x => x.ReadInt32());

                // Verify protocol version 
                if (!SupportedProtocolVersions.Contains(ProtocolVersion))
                {
                    if(ProtocolVersion < ProtocolVersions.Initial)
                        throw new ApiException(string.Format("FileZilla server is too old. Install FileZilla Server 0.9.46 or later"));

                    throw new ApiException(string.Format("Unsupported FileZilla protocol version:{0}. Report issue on https://github.com/PolarbearDK/Miracle.FileZilla.Api.", FormatVersion(ProtocolVersion)));
                }

                authentication = reader.Read<Authentication>(ProtocolVersion);
            });

            if (!authentication.NoPasswordRequired)
            {
                SendCommand(MessageType.Authenticate, authentication.HashPassword(password));
                Receive(MessageType.Authenticate);
            }
        }

        private string FormatVersion(int serverVersion)
        {
            return string.Format("{0:X}.{1:X}.{2:X}.{3:X}",
                (serverVersion >> 24) & 0xFF,
                (serverVersion >> 16) & 0xFF,
                (serverVersion >> 8) & 0xFF,
                (serverVersion >> 0) & 0xFF);
        }
    }
}