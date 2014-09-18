using System.Collections.Generic;
using System.IO;

namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// Class used to transfer all users and groups to/from FileZilla server
    /// </summary>
    public class AccountSettings: IBinarySerializable
    {
        /// <summary>
        /// All groups
        /// </summary>
        public List<Group> Groups { get; set; }
        /// <summary>
        /// All users
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// Default constructor (sets defaults as in FileZilla server interface)
        /// </summary>
        public AccountSettings()
        {
            Groups = new List<Group>();
            Users = new List<User>();
        }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Deserialize(BinaryReader reader, int protocolVersion)
        {
            Groups = reader.ReadList<Group>(protocolVersion);
            Users = reader.ReadList<User>(protocolVersion);
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        public void Serialize(BinaryWriter writer, int protocolVersion)
        {
            writer.WriteList(Groups, protocolVersion);
            writer.WriteList(Users, protocolVersion);
        }
    }
}