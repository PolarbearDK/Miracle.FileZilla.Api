using System;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Class used to receive an Error response from FileZilla server
    /// </summary>
    public class Error: IBinarySerializable
    {
        /// <summary>
        /// Mystery value that always seems to always have the value 1. AdminSocket.cpp::SendCommand has this as int nTextType. 
        /// </summary>
        public byte TextType { get; set; }
        /// <summary>
        /// The error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public void Deserialize(BinaryReader reader, int protocolVersion, int index)
        {
            TextType = reader.ReadByte();
            Message = reader.ReadRemainingAsText();
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
