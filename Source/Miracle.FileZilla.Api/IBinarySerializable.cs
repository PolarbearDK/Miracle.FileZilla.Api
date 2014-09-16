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
        void Deserialize(BinaryReader reader);
        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        void Serialize(BinaryWriter writer);
    }
}