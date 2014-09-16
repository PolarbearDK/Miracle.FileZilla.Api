using System.Collections.Generic;
using System.IO;

namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// Object representing a download or upload speed limit
    /// </summary>
    public class Limit : IBinarySerializable
    {
        /// <summary>
        /// Type of speed limit
        /// </summary>
        public SpeedLimitType SpeedLimitType { get; set; }
        /// <summary>
        /// Does this limit bypasseses server limits.
        /// </summary>
        public TriState BypassServerSpeedLimit { get; set; }
        /// <summary>
        /// Constant speed limit in kB/s (requires SpeedLimitType.FixedLimit to be operational)
        /// </summary>
        public ushort SpeedLimit { get; set; }
        /// <summary>
        /// List of speed limits (requires SpeedLimitType.UseSpeedLimitRules to be operational) 
        /// </summary>
        public List<SpeedLimit> SpeedLimits { get; set; }

        /// <summary>
        /// Default constructor (sets defaults as in FileZilla server interface)
        /// </summary>
        public Limit()
        {
            SpeedLimitType = SpeedLimitType.Default;
            BypassServerSpeedLimit = TriState.Default;
            SpeedLimit = 10;
            SpeedLimits = new List<SpeedLimit>();
        }

        /// <summary>
        /// Default constructor (sets defaults as in FileZilla server interface)
        /// </summary>
        /// <param name="isGroup">True if owning object is a group (different defaults)</param>
        public Limit(bool isGroup)
            : this()
        {
            if (isGroup)
            {
                SpeedLimitType = SpeedLimitType.NoLimit;
                BypassServerSpeedLimit = TriState.No;
            }
        }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        public void Deserialize(BinaryReader reader)
        {
            var options = reader.ReadByte();
            SpeedLimitType = (SpeedLimitType)(options & 0x3);
            BypassServerSpeedLimit = (TriState)(options >> 2);
            SpeedLimit = reader.ReadBigEndianInt16();
            SpeedLimits = reader.ReadList<SpeedLimit>();
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        public void Serialize(BinaryWriter writer)
        {
            var options = (byte)(((byte)SpeedLimitType & 0x3) | ((byte)BypassServerSpeedLimit << 2));
            writer.Write(options);
            writer.WriteBigEndianInt16(SpeedLimit);
            writer.WriteList(SpeedLimits);
        }
    }
}