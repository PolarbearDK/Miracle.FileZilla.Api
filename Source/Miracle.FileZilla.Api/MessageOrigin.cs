namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Enumeration that specifies the origin of a FileZilla message
    /// </summary>
    public enum MessageOrigin : byte
    {
        /// <summary>
        /// Message is a request from client
        /// </summary>
        ClientRequest = 0,
        /// <summary>
        /// Message is a server reply to a client request
        /// </summary>
        ServerReply = 1,
        /// <summary>
        /// Message is a status update from server
        /// </summary>
        ServerMessage = 2,
    }
}