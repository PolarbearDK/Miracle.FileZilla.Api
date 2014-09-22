using System;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Server message informing about amount of data currently being transfered.
    /// </summary>
    public class TransferInfo : IBinarySerializable
    {
        /// <summary>
        /// Type of event
        /// </summary>
        public TransferType Type { get; set; }
        /// <summary>
        /// Number of bytes transfered
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Deserialize(BinaryReader reader, int protocolVersion)
        {
            Type = (TransferType)reader.ReadByte();
            Count = reader.ReadInt32();
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