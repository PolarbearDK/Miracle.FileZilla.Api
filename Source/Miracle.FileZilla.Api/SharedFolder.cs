using System.Collections.Generic;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Class representing permissions to a shared folder
    /// </summary>
    public class SharedFolder : IBinarySerializable
    {
        /// <summary>
        /// Directory path
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Optional aliases
        /// </summary>
        public List<string> Aliases { get; set; }

        /// <summary>
        /// Access rights in the directory
        /// </summary>
        public AccessRights AccessRights { get; set; }

        /// <summary>
        /// Default constructor (sets defaults as in FileZilla server interface)
        /// </summary>
        public SharedFolder()
        {
            Aliases = new List<string>();    
            AccessRights = AccessRights.FileRead | AccessRights.DirList | AccessRights.DirSubdirs;
        }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Deserialize(BinaryReader reader, int protocolVersion)
        {
            Directory = reader.ReadText();
            Aliases = reader.ReadTextList();
            AccessRights = (AccessRights)reader.ReadBigEndianInt16();
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Serialize(BinaryWriter writer, int protocolVersion)
        {
            writer.WriteText(Directory);
            writer.WriteTextList(Aliases);
            writer.WriteBigEndianInt16((ushort)AccessRights);
        }
    }
}