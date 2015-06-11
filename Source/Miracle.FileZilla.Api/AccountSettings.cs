using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Miracle.FileZilla.Api
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
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public void Deserialize(BinaryReader reader, int protocolVersion, int index)
        {
            Groups = protocolVersion < ProtocolVersions.User16M
                ? reader.ReadList16<Group>(protocolVersion)
                : reader.ReadList24<Group>(protocolVersion);
            Users = protocolVersion < ProtocolVersions.User16M
                ? reader.ReadList16<User>(protocolVersion)
                : reader.ReadList24<User>(protocolVersion);
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public void Serialize(BinaryWriter writer, int protocolVersion, int index = 0)
        {
            // Check Uniqueness of groups
            if(Groups.GroupBy(x=>x.GroupName).Any(x => x.Count() > 1))
                throw new ApiException("Group names must be unique");

            if(protocolVersion < ProtocolVersions.User16M)
                writer.WriteList16(Groups, protocolVersion);
            else
                writer.WriteList24(Groups, protocolVersion);

            // Check Uniqueness of users
            if (Users.GroupBy(x => x.UserName).Any(x => x.Count() > 1))
                throw new ApiException("User names must be unique");

            if (protocolVersion < ProtocolVersions.User16M)
                writer.WriteList16(Users, protocolVersion);
            else
                writer.WriteList24(Users, protocolVersion);
        }
    }
}