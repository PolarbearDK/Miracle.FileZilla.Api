namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Protocol version constants.
    /// </summary>
    public static class ProtocolVersions
    {
        /// <summary>
        /// Initial protocol version supported
        /// Server version 0x00094300 - 0x00094700
        /// </summary>
        public const int Initial = 0x00010F00;
        /// <summary>
        /// First version supporting 16M users
        /// Server version 0x00094800 - 0x00095000
        /// </summary>
        public const int User16M = 0x00011000;
        /// <summary>
        /// Protocol changes mostly related to TLS
        /// Server version 0x00095100 - 0x00095300
        /// </summary>
        public const int TLS = 0x00012000;
        /// <summary>
        /// User password with Sha512 hashing
        /// Server version 0x00095400 - ?
        /// </summary>
        public const int Sha512 = 0x00013000;
    }
}