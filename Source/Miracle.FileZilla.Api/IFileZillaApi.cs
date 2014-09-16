using System.Collections.Generic;
using Miracle.FileZilla.Api.Elements;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// FileZilla server API
    /// </summary>
    public interface IFileZillaApi
    {
        /// <summary>
        /// Connect to FileZilla admin console 
        /// </summary>
        /// <param name="password">FileZilla admin password</param>
        void Connect(string password);
        /// <summary>
        /// Get state of FileZilla server
        /// </summary>
        /// <returns>FileZilla server state</returns>
        ServerState GetServerState();
        /// <summary>
        /// Get list af current connections to FileZilla server
        /// </summary>
        /// <returns>List of connections</returns>
        List<Connection> GetConnections();
        /// <summary>
        /// Kick a connection by id.
        /// </summary>
        /// <param name="connectionId">Connection Id (Use GetConnections to find ConnectionId)</param>
        /// <returns>True if successfull, otherwise false</returns>
        bool Kick(int connectionId);
        /// <summary>
        /// Kick a connection by id, and also ban IP address.
        /// </summary>
        /// <param name="connectionId">Connection Id (Use GetConnections to find ConnectionId)</param>
        /// <returns>True if successfull, otherwise false</returns>
        bool BanIp(int connectionId);
        /// <summary>
        /// Get account settings including all users and groups
        /// </summary>
        /// <returns>Account settings including all users and groups</returns>
        AccountSettings GetAccountSettings();
        /// <summary>
        /// Set account settings. 
        /// Note! This replaces ALL users and groups on FileZilla server.
        /// </summary>
        /// <returns>Account settings including all users and groups</returns>
        bool SetAccountSettings(AccountSettings accountSettings);
        /// <summary>
        /// FileZilla Server version.
        /// </summary>
        int ServerVersion { get; }
        /// <summary>
        /// FileZilla protocol version.
        /// </summary>
        int ProtocolVersion { get; }
        /// <summary>
        /// The size of the receiving buffer. If this is too small, then returning data from server is discarded, and an exception is thrown.   
        /// </summary>
        int BufferSize { get; set; }
        /// <summary>
        /// Check if socket is connected
        /// </summary>
        bool IsConnected { get; }
    }
}