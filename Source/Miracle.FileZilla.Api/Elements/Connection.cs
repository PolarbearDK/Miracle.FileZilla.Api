using System;
using System.IO;

namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// Class representing an active connection to FileZilla
    /// </summary>
    public class Connection: IBinarySerializable
    {
        /// <summary>
        /// Connection Id. This is the value used to kick/ban connections.
        /// </summary>
        public int ConnectionId { get; set; }
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
        /// Transfer mode
        /// TODO! Decode this into enum.
        /// </summary>
        public byte TransferMode { get; set; }
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
        public void Deserialize(BinaryReader reader)
        {
            ConnectionId = reader.ReadInt32();
            Ip = reader.ReadText();
            Port = reader.ReadInt32();
            UserName = reader.ReadText();
            TransferMode = reader.ReadByte();

            if (TransferMode != 0)
            {
                PhysicalFile = reader.ReadText();
                LogicalFile = reader.ReadText();

                // Bit 5 and 6 indicate presence of currentOffset and totalSize.
                if ((TransferMode & 0x20) != 0) CurrentOffset = reader.ReadInt64();
                if ((TransferMode & 0x40) != 0) TotalSize = reader.ReadInt64();
            }
        }
        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
