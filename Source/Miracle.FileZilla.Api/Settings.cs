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
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public void Deserialize(BinaryReader reader, int protocolVersion, int index)
        {
            Options = reader.ReadList16<Option>(protocolVersion);
            DownloadSpeedLimitRule = reader.ReadList16<SpeedLimitRule>(protocolVersion);
            UploadSpeedLimitRule = reader.ReadList16<SpeedLimitRule>(protocolVersion);
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public void Serialize(BinaryWriter writer, int protocolVersion, int index = 0)
        {
            writer.WriteList16(Options, protocolVersion);
            writer.WriteList16(DownloadSpeedLimitRule, protocolVersion);
            writer.WriteList16(UploadSpeedLimitRule, protocolVersion);
        }

        /// <summary>
        /// Get Option from OPtionId enumeration 
        /// </summary>
        /// <param name="optionId"></param>
        /// <returns></returns>
        public Option GetOption(OptionIdPreV12 optionId)
        {
           return Options[(int)optionId -1]; // Off by one
        }

        /// <summary>
        /// Get Option from OPtionId enumeration 
        /// </summary>
        /// <param name="optionId"></param>
        /// <returns></returns>
        public Option GetOption(OptionIdPostV11 optionId)
        {
           return Options[(int)optionId -1]; // Off by one
        }
    }
}