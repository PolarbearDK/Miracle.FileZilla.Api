using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Miracle.FileZilla.Api.Elements;

namespace Miracle.FileZilla.Api
{

    /// <summary>
    /// FileZilla server API
    /// </summary>
    public class FileZillaApi : AdminSocket, IFileZillaApi
    {
        /// <summary>
        /// Versions used to develop this API
        /// </summary>
        public const uint DevelopmentServerVersion = 0x00094600;
        /// <summary>
        /// Versions used to develop this API
        /// </summary>
        public const uint DevelopmentProtocolVersion = 0x00010F00;
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
        /// Construct FileZillaApi using default IP/port
        /// </summary>
        public FileZillaApi()
            : base(IPAddress.Parse(DefaultIp), DefaultPort)
        {
        }

        /// <summary>
        /// Construct FileZillaApi using specific IP/port
        /// </summary>
        /// <param name="address">IP address of filezilla server.</param>
        /// <param name="port">Admin port as specified when FileZilla server were installed</param>
        public FileZillaApi(IPAddress address, int port) 
            : base(address, port)
        {
        }

        /// <summary>
        /// Connect to FileZilla admin console 
        /// </summary>
        /// <param name="password">FileZilla admin password</param>
        public void Connect(string password)
        {
            Authentication authentication = null;
            Connect();

            Receive(reader =>
            {
                reader.Verify("FZS");
                ServerVersion = reader.ReadLength(reader.ReadBigEndianInt16(),x => x.ReadInt32());
                ProtocolVersion = reader.ReadLength(reader.ReadBigEndianInt16(),x => x.ReadInt32());
                authentication = reader.BaseStream.Length > 15 
                    ? reader.Read<Authentication>() 
                    : null;
            });

            if (authentication != null)
            {
                Authenticate(authentication.HashPassword(password));
            }
        }

        private void Authenticate(byte[] hashedPassword)
        {
            SendCommand(MessageOrigin.Client, MessageType.Authenticate, hashedPassword);
            Receive(MessageOrigin.Server, MessageType.Authenticate);
        }

        /// <summary>
        /// Get state of FileZilla server
        /// </summary>
        /// <returns>FileZilla server state</returns>
        public ServerState GetServerState()
        {
            SendCommand(MessageOrigin.Client, MessageType.ServerState);
            return Receive<ServerState>(MessageOrigin.Server, MessageType.ServerState);
        }

        /// <summary>
        /// Get list af current connections to FileZilla server
        /// </summary>
        /// <returns>List of connections</returns>
        public List<Connection> GetConnections()
        {
            SendCommand(MessageOrigin.Client, MessageType.UserControl, x => x.Write((byte)UserControl.GetList));
            return Receive<List<Connection>>(MessageOrigin.Server, MessageType.UserControl);
        }

        /// <summary>
        /// Kick a connection by id.
        /// </summary>
        /// <param name="connectionId">Connection Id (Use GetConnections to find ConnectionId)</param>
        /// <returns>True if successfull, otherwise false</returns>
        public bool Kick(int connectionId)
        {
            SendCommand(MessageOrigin.Client, MessageType.UserControl, x =>
            {
                x.Write((byte)UserControl.Kick);
                x.Write(connectionId);
            });

            return Receive<bool>(MessageOrigin.Server, MessageType.UserControl);
        }

        /// <summary>
        /// Kick a connection by id, and also ban IP address.
        /// </summary>
        /// <param name="connectionId">Connection Id (Use GetConnections to find ConnectionId)</param>
        /// <returns>True if successfull, otherwise false</returns>
        public bool BanIp(int connectionId)
        {
            SendCommand(MessageOrigin.Client, MessageType.UserControl, x =>
            {
                x.Write((byte)UserControl.BanIp);
                x.Write(connectionId);
            });

            return Receive<bool>(MessageOrigin.Server, MessageType.UserControl);
        }

        /// <summary>
        /// Get account settings including all users and groups
        /// </summary>
        /// <returns>Account settings including all users and groups</returns>
        public AccountSettings GetAccountSettings()
        {
            SendCommand(MessageOrigin.Client, MessageType.AccountSettings);
            return Receive<AccountSettings>(MessageOrigin.Server, MessageType.AccountSettings);
        }

        /// <summary>
        /// Set account settings. 
        /// Note! This replaces ALL users and groups on FileZilla server.
        /// </summary>
        /// <returns>Account settings including all users and groups</returns>
        public bool SetAccountSettings(AccountSettings accountSettings)
        {
            SendCommand(MessageOrigin.Client, MessageType.AccountSettings, accountSettings.Serialize);
            return Receive<bool>(MessageOrigin.Server, MessageType.AccountSettings);
        }

        internal void SendCommand(MessageOrigin messageOrigin, MessageType messageType)
        {
            SendCommand(messageOrigin, messageType, new byte[] { });
        }

        internal void SendCommand(MessageOrigin messageOrigin, MessageType messageType, byte data)
        {
            SendCommand(messageOrigin, messageType, new[] { data });
        }

        internal void SendCommand(MessageOrigin messageOrigin, MessageType messageType, byte[] data)
        {
            SendCommand(messageOrigin, messageType, writer => writer.Write(data));
        }

        internal void SendCommand(MessageOrigin messageOrigin, MessageType messageType, Action<BinaryWriter> action)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    var cmd = (byte)(((int)messageOrigin) | ((byte)messageType << 2));
                    writer.Write(cmd);
                    writer.WriteLength(action);
                }

                Send(stream.ToArray());
            }
        }

        internal void Receive(MessageOrigin messageOrigin, MessageType messageType)
        {
            var message = ReceiveMessage(messageOrigin, messageType);
            if (message.RawData.Length != 0)
                throw new ProtocolException("Expected message with length 0, actual " + message.RawData.Length);
        }

        internal T Receive<T>(MessageOrigin messageOrigin, MessageType messageType)
        {
            var message = ReceiveMessage(messageOrigin, messageType);
            return (T) message.Body;
        }

        internal void Receive(Action<BinaryReader> action)
        {
            var data = Receive();
            using (var memoryStream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memoryStream))
                {
                    action(reader);
                }
            }
        }

        internal Message ReceiveMessage(MessageOrigin messageOrigin, MessageType messageType)
        {
            var messages = ReceiveAndSplitIntoIndividualMessages();
            Message message = null;
            foreach (Message check in messages)
            {
                if (check.MessageOrigin == messageOrigin && check.MessageType == messageType)
                {
                    if (message != null)
                        throw new ProtocolException("Multiple commands matched");
                    message = check;
                }
#if DEBUG
                else
                {
                    TraceData(string.Format("Message ignored: {0}/{1} with length {2}", check.MessageOrigin,check.MessageType,check.RawData.Length), check.RawData);
                }
#endif
            }

            if (message == null)
            {
                // All server messages? Poll again for the desired reply
                if (messages.All(x => x.MessageOrigin == MessageOrigin.ServerMessage))
                    return ReceiveMessage(messageOrigin, messageType);

                throw ProtocolException.Create(messageOrigin, messageType, messages);
            }

            return message;
        }

        internal Message[] ReceiveAndSplitIntoIndividualMessages()
        {
            var data = Receive();
            var list = new List<Message>();
            using (var memoryStream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memoryStream))
                {
                    while (memoryStream.Position < data.Length)
                    {
                        var b = reader.ReadByte();
                        var count = reader.ReadInt32();

                        byte[] payload = reader.ReadBytes(count);
                        var message = new Message((MessageOrigin) (b & 0x3),(MessageType) (b >> 2),payload);
                        list.Add(message);
                    }
                }
            }
            return list.ToArray();
        }
    }
}