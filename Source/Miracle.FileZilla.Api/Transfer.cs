using System;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Class containing information about a FTP transfer
    /// </summary>
    public class Transfer : IBinarySerializable
    {
        /// <summary>
        /// Connection Id. This is the value used to kick/ban connections.
        /// </summary>
        public int ConnectionId { get; set; }
        /// <summary>
        /// Transfer mode
        /// </summary>
        public TransferMode TransferMode { get; set; }
        /// <summary>
        /// The physical filename being transfered.
        /// </summary>
        public string PhysicalFile { get; set; }
        /// <summary>
        /// The logical filename being transfered.
        /// </summary>
        public string LogicalFile { get; set; }
        /// <summary>
        /// The current offset in the file being transfered.
        /// </summary>
        public long? CurrentOffset { get; set; }
        /// <summary>
        /// The total size of the file being transfered.
        /// </summary>
        public long? TotalSize { get; set; }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public void Deserialize(BinaryReader reader, int protocolVersion, int index)
        {
            ConnectionId = reader.ReadInt32();
            DeserializeChildren(reader, protocolVersion);
            var flags = reader.ReadByte();
            TransferMode = (TransferMode)(flags & 0x3);

            if (flags != 0)
            {
                PhysicalFile = reader.ReadText();
                LogicalFile = reader.ReadText();

                // Bit 5 and 6 indicate presence of currentOffset and totalSize.
                if ((flags & 0x20) != 0) CurrentOffset = reader.ReadInt64();
                if ((flags & 0x40) != 0) TotalSize = reader.ReadInt64();
            }
        }

        /// <summary>
        /// Let any inheriters serialize additional properties
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        protected virtual void DeserializeChildren(BinaryReader reader, int protocolVersion)
        {
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