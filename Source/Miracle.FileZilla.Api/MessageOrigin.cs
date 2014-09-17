namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Enumeration that specifies the origin of a FileZilla message
    /// </summary>
    public enum MessageOrigin : byte
    {
        /// <summary>
        /// Message originated from Client
        /// </summary>
        Client = 0,
        /// <summary>
        /// Message originated from Server
        /// </summary>
        Server = 1,
        /// <summary>
        /// Message originated from Server
        /// </summary>
        ServerMessage = 2,
    }
}