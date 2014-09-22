using System;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Class used to transfer server settings to/from FileZilla server
    /// </summary>
    public class Event : IBinarySerializable
    {
        /// <summary>
        /// Type of event
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// Time of the event
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Text associated with the event
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Deserialize(BinaryReader reader, int protocolVersion)
        {
            Type = reader.ReadByte();

            var fileTimeHigh = reader.ReadUInt32();
            var fileTimeLow = reader.ReadUInt32();
            var fileTime = ((ulong) fileTimeHigh) << 32 | fileTimeLow;
            DateTime fromFileTimeUtc = DateTime.FromFileTimeUtc((long)fileTime);

            // This is not really a UTC time but a SystemTime converted to a FileTime.
            // The next statement assumes that FileZilla Server and this machine is in same time zone. 
            Time = new DateTime(fromFileTimeUtc.Ticks);
            Text = reader.ReadRemainingAsText();
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