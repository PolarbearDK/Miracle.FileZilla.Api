using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Common interface used to serialize to/from FileZilla binary wire format
    /// </summary>
    public interface IBinarySerializable
    {
        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        void Deserialize(BinaryReader reader, int protocolVersion, int index = 0);

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        void Serialize(BinaryWriter writer, int protocolVersion, int index = 0);
    }
}