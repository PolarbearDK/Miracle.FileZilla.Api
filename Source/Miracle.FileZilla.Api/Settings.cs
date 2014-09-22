using System.Collections.Generic;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Class used to transfer server settings to/from FileZilla server
    /// </summary>
    public class Settings : IBinarySerializable
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Option> Options { get; set; }
        /// <summary>
        /// Download speed limit
        /// </summary>
        public List<SpeedLimitRule> DownloadSpeedLimitRule { get; set; }
        /// <summary>
        /// Upload speed limit
        /// </summary>
        public List<SpeedLimitRule> UploadSpeedLimitRule { get; set; }

        /// <summary>
        /// Default constructor (sets defaults as in FileZilla server interface)
        /// </summary>
        public Settings()
        {
            Options = new List<Option>();
        }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Deserialize(BinaryReader reader, int protocolVersion)
        {
            Options = reader.ReadList<Option>(protocolVersion);
            DownloadSpeedLimitRule = reader.ReadList<SpeedLimitRule>(protocolVersion);
            UploadSpeedLimitRule = reader.ReadList<SpeedLimitRule>(protocolVersion);
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Serialize(BinaryWriter writer, int protocolVersion)
        {
            writer.WriteList(Options, protocolVersion);
            writer.WriteList(DownloadSpeedLimitRule, protocolVersion);
            writer.WriteList(UploadSpeedLimitRule, protocolVersion);
        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="optionId"></param>
        /// <returns></returns>
        public Option GetOption(OptionId optionId)
        {
           return Options[(int)optionId -1]; // Off by one
        }
    }
}