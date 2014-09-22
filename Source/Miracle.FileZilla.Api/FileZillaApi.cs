using System.Collections.Generic;
using System.Net;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// FileZilla server API
    /// </summary>
    public class FileZillaApi : FileZillaServerProtocol, IFileZillaApi
    {
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
        /// Get state of FileZilla server
        /// </summary>
        /// <returns>FileZilla server state</returns>
        public ServerState GetServerState()
        {
            SendCommand(MessageType.ServerState);
            return Receive<ServerState>(MessageType.ServerState);
        }

        /// <summary>
        /// Set state of FileZilla server
        /// </summary>
        /// <param name="serverState">The new state</param>
        /// <returns></returns>
        public ServerState SetServerState(ServerState serverState)
        {
            SendCommand(MessageType.ServerState, x=>x.WriteBigEndianInt16((ushort)serverState));
            return Receive<ServerState>(MessageType.ServerState);
        }

        /// <summary>
        /// Get state of FileZilla server
        /// </summary>
        /// <returns>FileZilla server state</returns>
        public Settings GetSettings()
        {
            SendCommand(MessageType.Settings);
            return Receive<Settings>(MessageType.Settings);
        }

        /// <summary>
        /// Get state of FileZilla server
        /// </summary>
        /// <returns>True if Success</returns>
        public bool SetSettings(Settings settings)
        {
            // Check options
            if(settings.Options.Count != (int)OptionId.OPTIONS_NUM)
                throw new ApiException("Bad option count");
            
            // Before serializing, give Admin Password option special treatment. 
            // If password is null, then set it to invalid ("*").
            // That way server can distinguish between 
            //   - Not set (Null, serialized as "*")
            //   - Blank password ("")
            //   - Actual password
            var adminPassword = settings.GetOption(OptionId.ADMINPASS);
            if (adminPassword.TextValue == null) adminPassword.TextValue = "*";

            SendCommand(MessageType.Settings, writer => settings.Serialize(writer, ProtocolVersion));
            return Receive<bool>(MessageType.Settings);
        }

        /// <summary>
        /// Get list af current connections to FileZilla server
        /// </summary>
        /// <returns>List of connections</returns>
        public List<Connection> GetConnections()
        {
            SendCommand(MessageType.UserControl, x => x.Write((byte)UserControl.GetList));
            return Receive<List<Connection>>(MessageType.UserControl);
        }

        /// <summary>
        /// Kick a connection by id.
        /// </summary>
        /// <param name="connectionId">Connection Id (Use GetConnections to find ConnectionId)</param>
        /// <returns>True if successfull, otherwise false</returns>
        public bool Kick(int connectionId)
        {
            SendCommand(MessageType.UserControl, x =>
            {
                x.Write((byte)UserControl.Kick);
                x.Write(connectionId);
            });

            return Receive<bool>(MessageType.UserControl);
        }

        /// <summary>
        /// Kick a connection by id, and also ban IP address.
        /// </summary>
        /// <param name="connectionId">Connection Id (Use GetConnections to find ConnectionId)</param>
        /// <returns>True if successfull, otherwise false</returns>
        public bool BanIp(int connectionId)
        {
            SendCommand(MessageType.UserControl, x =>
            {
                x.Write((byte)UserControl.BanIp);
                x.Write(connectionId);
            });

            return Receive<bool>(MessageType.UserControl);
        }

        /// <summary>
        /// Get account settings including all users and groups
        /// </summary>
        /// <returns>Account settings including all users and groups</returns>
        public AccountSettings GetAccountSettings()
        {
            SendCommand(MessageType.AccountSettings);
            return Receive<AccountSettings>(MessageType.AccountSettings);
        }

        /// <summary>
        /// Set account settings. 
        /// Note! This replaces ALL users and groups on FileZilla server.
        /// </summary>
        /// <returns>True if Success</returns>
        public bool SetAccountSettings(AccountSettings accountSettings)
        {
            SendCommand(MessageType.AccountSettings, writer => accountSettings.Serialize(writer, ProtocolVersion));
            return Receive<bool>(MessageType.AccountSettings);
        }
    }
}