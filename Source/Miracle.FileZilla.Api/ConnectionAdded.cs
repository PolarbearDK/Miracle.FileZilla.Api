using System;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// A connection has been added
    /// </summary>
    public class ConnectionAdded : IBinarySerializable
    {
        /// <summary>
        /// Connection Id
        /// </summary>
        public int ConnectionId { get; set; }
        /// <summary>
        /// Connection IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// Connection Port
        /// </summary>
        public uint Port { get; set; }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public void Deserialize(BinaryReader reader, int protocolVersion, int index)
        {
            ConnectionId = reader.ReadInt32();
            Ip = reader.ReadText();
            Port = reader.ReadUInt32();
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public void Serialize(BinaryWriter writer, int protocolVersion, int index)
        {
            throw new NotImplementedException();
        }
    }
}