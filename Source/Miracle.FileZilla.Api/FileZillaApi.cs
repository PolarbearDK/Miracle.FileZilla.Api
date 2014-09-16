using System;
using System.Collections.Generic;
using System.IO;
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
                ServerVersion = reader.ReadShortLength(x => x.ReadInt32());
                ProtocolVersion = reader.ReadShortLength(x => x.ReadInt32());
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
            SendCommand(CommandOrigin.Client, CommandType.Authenticate, hashedPassword);
            Receive(CommandOrigin.Server, CommandType.Authenticate);
        }

        /// <summary>
        /// Get state of FileZilla server
        /// </summary>
        /// <returns>FileZilla server state</returns>
        public ServerState GetServerState()
        {
            SendCommand(CommandOrigin.Client, CommandType.ServerState);
            return Receive(CommandOrigin.Server, CommandType.ServerState, reader => (ServerState)reader.ReadBigEndianInt16());
        }

        /// <summary>
        /// Get list af current connections to FileZilla server
        /// </summary>
        /// <returns>List of connections</returns>
        public List<Connection> GetConnections()
        {
            SendCommand(CommandOrigin.Client, CommandType.UserControl, x => x.Write((byte)UserControl.GetList));
            return Receive(CommandOrigin.Server, CommandType.UserControl, reader =>
            {
                reader.Verify((byte) UserControl.GetList);
                return reader.ReadList<Connection>();
            });
        }

        /// <summary>
        /// Kick a connection by id.
        /// </summary>
        /// <param name="connectionId">Connection Id (Use GetConnections to find ConnectionId)</param>
        /// <returns>True if successfull, otherwise false</returns>
        public bool Kick(int connectionId)
        {
            SendCommand(CommandOrigin.Client, CommandType.UserControl, x =>
            {
                x.Write((byte)UserControl.Kick);
                x.Write(connectionId);
            });

            return Receive(CommandOrigin.Server, CommandType.UserControl, reader =>
            {
                reader.Verify((byte)UserControl.Kick);
                return reader.ReadByte() == 0;
            });
        }

        /// <summary>
        /// Kick a connection by id, and also ban IP address.
        /// </summary>
        /// <param name="connectionId">Connection Id (Use GetConnections to find ConnectionId)</param>
        /// <returns>True if successfull, otherwise false</returns>
        public bool BanIp(int connectionId)
        {
            SendCommand(CommandOrigin.Client, CommandType.UserControl, x =>
            {
                x.Write((byte)UserControl.BanIp);
                x.Write(connectionId);
            });

            return Receive(CommandOrigin.Server, CommandType.UserControl, reader =>
            {
                reader.Verify((byte)UserControl.BanIp);
                return reader.ReadByte() == 0;
            });
        }

        /// <summary>
        /// Get account settings including all users and groups
        /// </summary>
        /// <returns>Account settings including all users and groups</returns>
        public AccountSettings GetAccountSettings()
        {
            SendCommand(CommandOrigin.Client, CommandType.Permissions);
            return Receive(CommandOrigin.Server, CommandType.Permissions, reader => reader.Read<AccountSettings>());
        }

        /// <summary>
        /// Set account settings. 
        /// Note! This replaces ALL users and groups on FileZilla server.
        /// </summary>
        /// <returns>Account settings including all users and groups</returns>
        public bool SetAccountSettings(AccountSettings accountSettings)
        {
            SendCommand(CommandOrigin.Client, CommandType.Permissions, accountSettings.Serialize);
            return Receive(CommandOrigin.Server, CommandType.Permissions, reader => reader.ReadByte() == 0);
        }

        internal void SendCommand(CommandOrigin commandOrigin, CommandType commandType)
        {
            SendCommand(commandOrigin, commandType, new byte[] { });
        }

        internal void SendCommand(CommandOrigin commandOrigin, CommandType commandType, byte data)
        {
            SendCommand(commandOrigin, commandType, new[] { data });
        }

        internal void SendCommand(CommandOrigin commandOrigin, CommandType commandType, byte[] data)
        {
            SendCommand(commandOrigin, commandType, writer => writer.Write(data));
        }

        internal void SendCommand(CommandOrigin commandOrigin, CommandType commandType, Action<BinaryWriter> action)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    var cmd = (byte)(((int)commandOrigin) | ((byte)commandType << 2));
                    writer.Write(cmd);
                    writer.WriteLength(action);
                }

                Send(stream.ToArray());
            }
        }

        internal void Receive(CommandOrigin commandOrigin, CommandType commandType)
        {
            var data = Receive();
            using (var memoryStream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memoryStream))
                {
                    reader.Verify(commandOrigin, commandType);
                    reader.Verify((int)0); // Verify length 0
                }
            }
        }

        internal T Receive<T>(CommandOrigin commandOrigin, CommandType commandType, Func<BinaryReader, T> action)
        {
            var data = Receive();
            using (var memoryStream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(memoryStream))
                {
                    reader.Verify(commandOrigin, commandType);
                    return reader.ReadLongLength(action);
                }
            }
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
    }
}