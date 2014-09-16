namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Enumeration that specifies the origin of a FileZilla command
    /// </summary>
    public enum CommandOrigin : byte
    {
        /// <summary>
        /// Command originated from Client
        /// </summary>
        Client = 0,
        /// <summary>
        /// Command originated from Server
        /// </summary>
        Server = 1,
    }
}