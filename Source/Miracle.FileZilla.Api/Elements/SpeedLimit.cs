using System;
using System.IO;

namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// 
    /// </summary>
    public class SpeedLimit : IBinarySerializable
    {
        /// <summary>
        /// Speed limit in kB/s
        /// </summary>
        public uint Speed { get; set; }
        /// <summary>
        /// Speed limit active on date
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Speed limit active from time
        /// </summary>
        public TimeSpan? FromTime { get; set; }
        /// <summary>
        /// Speed limit active to time
        /// </summary>
        public TimeSpan? ToTime { get; set; }
        /// <summary>
        /// Speed limit active on weekdays
        /// </summary>
        public Days Days { get; set; }

        /// <summary>
        /// Default constructor (sets defaults as in FileZilla server interface)
        /// </summary>
        public SpeedLimit()
        {
            Speed = 8;
            Days = Days.All;
        }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        public void Deserialize(BinaryReader reader)
        {
            Speed = reader.ReadBigEndianInt32();
            Date = reader.ReadDate();
            FromTime = reader.ReadTime();
            ToTime = reader.ReadTime();
            Days = (Days)reader.ReadByte();
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        public void Serialize(BinaryWriter writer)
        {
             writer.WriteBigEndianInt32(Speed);
             writer.WriteDate(Date);
             writer.WriteTime(FromTime);
             writer.WriteTime(ToTime);
             writer.Write((byte)Days);
        }
    }
}