namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// FileZilla message type
    /// </summary>
    public enum MessageType : byte
    {
        /// <summary>
        /// Authendicate against FileZilla (done in Connect method)
        /// </summary>
        Authenticate = 0x0,
        /// <summary>
        /// Error response
        /// </summary>
        Error = 0x1,
        /// <summary>
        /// Get/Set server state
        /// </summary>
        ServerState = 0x2,
        /// <summary>
        /// User(connection) control
        /// </summary>
        UserControl = 0x3,
        /// <summary>
        /// Event Server message
        /// </summary>
        Event = 0x4,
        /// <summary>
        /// Get/Set settings
        /// </summary>
        Settings = 0x5,
        /// <summary>
        /// Get set accountsettings
        /// </summary>
        AccountSettings = 0x6,
        /// <summary>
        /// Transfer server message 
        /// </summary>
        Transfer = 0x7,
        /// <summary>
        /// Loopback message to/from server
        /// </summary>
        Loopback = 0x8,
    }
}