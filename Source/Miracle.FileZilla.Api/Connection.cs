using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Information about an active FTP connection to FileZilla
    /// </summary>
    public class Connection: Transfer
    {
        /// <summary>
        /// Source IP of connection (IP of user)
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// Source port of connection (IP of user)
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Name of connected user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        protected override void DeserializeChildren(BinaryReader reader, int protocolVersion)
        {
            Ip = reader.ReadText();
            Port = reader.ReadInt32();
            UserName = reader.ReadText();
        }
    }
}
