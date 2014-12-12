namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Protocol version constants.
    /// </summary>
    public static class ProtocolVersions
    {
        /// <summary>
        /// Initial protocol version supported
        /// </summary>
        public const int Initial = 0x00010F00; // Server version 0x00094600
        /// <summary>
        /// First version supporting 16M users
        /// </summary>
        public const int User16M = 0x00011000; // Server version 0x00094800
    }
}