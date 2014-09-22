using System;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Class used to transfer a single option to/from FileZilla server
    /// </summary>
    public class Option : IBinarySerializable
    {
        /// <summary>
        /// OptionType of option
        /// </summary>
        public OptionType OptionType { get; set; }

        /// <summary>
        /// Text value
        /// </summary>
        public string TextValue { get; set; }

        /// <summary>
        /// Text value
        /// </summary>
        public long NumericValue { get; set; }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Deserialize(BinaryReader reader, int protocolVersion)
        {
            OptionType = (OptionType)reader.ReadByte();
            switch (OptionType)
            {
                case OptionType.Text:
                    TextValue = reader.ReadText24();
                    break;
                case OptionType.Numeric:
                    NumericValue = reader.ReadInt64();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Serialize(BinaryWriter writer, int protocolVersion)
        {
            writer.Write((byte)OptionType);
            switch (OptionType)
            {
                case OptionType.Text:
                    writer.WriteText24(TextValue);
                    break;
                case OptionType.Numeric:
                    writer.Write(NumericValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Helper collection with info about options. Index of this matches index of Settings.Options.
        /// </summary>
        public static readonly OptionInfo[] OptionInfos =
        {
            new OptionInfo("Serverports", 0, false),
            new OptionInfo("Number of Threads", 1, false),
            new OptionInfo("Maximum user count", 1, false),
            new OptionInfo("Timeout", 1, false),
            new OptionInfo("No Transfer Timeout", 1, false),
            new OptionInfo("Allow Incoming FXP", 1, false),
            new OptionInfo("Allow outgoing FXP", 1, false),
            new OptionInfo("No Strict In FXP", 1, false),
            new OptionInfo("No Strict Out FXP", 1, false),
            new OptionInfo("Login Timeout", 1, false),
            new OptionInfo("Show Pass in Log", 1, false),
            new OptionInfo("Custom PASV IP type", 1, false),
            new OptionInfo("Custom PASV IP", 0, false),
            new OptionInfo("Custom PASV min port", 1, false),
            new OptionInfo("Custom PASV max port", 1, false),
            new OptionInfo("Initial Welcome Message", 0, false),
            new OptionInfo("Admin port", 1, true),
            new OptionInfo("Admin Password", 0, true),
            new OptionInfo("Admin IP Bindings", 0, true),
            new OptionInfo("Admin IP Addresses", 0, true),
            new OptionInfo("Enable logging", 1, false),
            new OptionInfo("Logsize limit", 1, false),
            new OptionInfo("Logfile type", 1, false),
            new OptionInfo("Logfile delete time", 1, false),
            new OptionInfo("Disable IPv6", 1, false),
            new OptionInfo("Enable HASH", 1, false),
            new OptionInfo("Download Speedlimit OptionType", 1, false),
            new OptionInfo("Upload Speedlimit OptionType", 1, false),
            new OptionInfo("Download Speedlimit", 1, false),
            new OptionInfo("Upload Speedlimit", 1, false),
            new OptionInfo("Buffer Size", 1, false),
            new OptionInfo("Custom PASV IP server", 0, false),
            new OptionInfo("Use custom PASV ports", 1, false),
            new OptionInfo("Mode Z Use", 1, false),
            new OptionInfo("Mode Z min level", 1, false),
            new OptionInfo("Mode Z max level", 1, false),
            new OptionInfo("Mode Z allow local", 1, false),
            new OptionInfo("Mode Z disallowed IPs", 0, false),
            new OptionInfo("IP Bindings", 0, false),
            new OptionInfo("IP Filter Allowed", 0, false),
            new OptionInfo("IP Filter Disallowed", 0, false),
            new OptionInfo("Hide Welcome Message", 1, false),
            new OptionInfo("Enable SSL", 1, false),
            new OptionInfo("Allow explicit SSL", 1, false),
            new OptionInfo("SSL Key file", 0, false),
            new OptionInfo("SSL Certificate file", 0, false),
            new OptionInfo("Implicit SSL ports", 0, false),
            new OptionInfo("Force explicit SSL", 1, false),
            new OptionInfo("Network Buffer Size", 1, false),
            new OptionInfo("Force PROT P", 1, false),
            new OptionInfo("SSL Key Password", 0, false),
            new OptionInfo("Allow shared write", 1, false),
            new OptionInfo("No External IP On Local", 1, false),
            new OptionInfo("Active ignore local", 1, false),
            new OptionInfo("Autoban enable", 1, false),
            new OptionInfo("Autoban attempts", 1, false),
            new OptionInfo("Autoban type", 1, false),
            new OptionInfo("Autoban time", 1, false),
            new OptionInfo("Service name", 0, true),
            new OptionInfo("Service display name", 0, true)
        };
    }
}