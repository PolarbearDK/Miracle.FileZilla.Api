using System;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// A connection has been removed
    /// </summary>
    public class ConnectionRemoved : IBinarySerializable
    {
        /// <summary>
        /// Connection Id
        /// </summary>
        public int ConnectionId { get; set; }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Deserialize(BinaryReader reader, int protocolVersion)
        {
            ConnectionId = reader.ReadInt32();
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Serialize(BinaryWriter writer, int protocolVersion)
        {
            throw new NotImplementedException();
        }
    }
}