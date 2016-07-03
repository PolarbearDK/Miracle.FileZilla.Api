using System;
using System.Collections.Generic;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// FileZilla server API
    /// </summary>
    public interface IFileZillaApi: IDisposable
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
        /// Set state of FileZilla server
        /// </summary>
        /// <param name="serverState">The new state</param>
        /// <returns></returns>
        ServerState SetServerState(ServerState serverState);

        /// <summary>
        /// Get state of FileZilla server
        /// </summary>
        /// <returns>FileZilla server options</returns>
        Settings GetSettings();

        /// <summary>
        /// Get state of FileZilla server
        /// </summary>
        /// <returns>True if Success</returns>
        bool SetSettings(Settings settings);

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
        /// <returns>True if Success</returns>
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
        /// Check if socket is connected
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// How many times to try to get a message of a particular type before giving up.
        /// </summary>
        int ReceiveMessageRetryCount { get; set; }
    }
}