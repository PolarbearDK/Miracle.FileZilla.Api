using System.Collections.Generic;
using System.IO;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Class representing a FileZilla group
    /// </summary>
    public class Group : IBinarySerializable
    {
        /// <summary>
        /// Group name. On Users, this is a reference to an existing group name
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// Connection limit per IP. 0=no limit
        /// </summary>
        public int IpLimit { get; set; }
        /// <summary>
        /// Connection limit per group/user. 0=no limit
        /// </summary>
        public int UserLimit { get; set; }
        /// <summary>
        /// Bypass userlimit on server
        /// </summary>
        public TriState BypassUserLimit
        {
            get { return _bypassUserLimit; }
            set
            {
                if(value == TriState.Default && GetType() == typeof(Group))
                    throw new ApiException("Groups are not allowed to use TriState." + TriState.Default);
                _bypassUserLimit = value;
            }
        }
        private TriState _bypassUserLimit;
        /// <summary>
        /// Is user/group enabled
        /// </summary>
        public TriState Enabled
        {
            get { return _enabled; }
            set
            {
                if (value == TriState.Default && GetType() == typeof(Group))
                    throw new ApiException("Groups are not allowed to use TriState." + TriState.Default);
                _enabled = value;
            }
        }
        private TriState _enabled;
        /// <summary>
        /// List of disallowed IP's (IP Filter)
        /// </summary>
        public List<string> DisallowedIPs { get; set; }
        /// <summary>
        /// List of allowed IP's (IP Filter)
        /// </summary>
        public List<string> AllowedIPs { get; set; }
        /// <summary>
        /// Use 8+3 filenames? This property is'nt available in the admin interface.
        /// Better leave it alone... Removed in later versions
        /// </summary>
        protected bool EightPlusThree { get; set; }
        /// <summary>
        /// List of permissions
        /// </summary>
        public List<SharedFolder> SharedFolders { get; set; }
        /// <summary>
        /// Download speed limit
        /// </summary>
        public SpeedLimit DownloadSpeedLimit { get; set; }
        /// <summary>
        /// Upload speed limit
        /// </summary>
        public SpeedLimit UploadSpeedLimit { get; set; }
        /// <summary>
        /// Comment for user/group
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Force the use of SSL
        /// </summary>
        public TriState ForceSsl { get; set; }

        /// <summary>
        /// Default constructor (sets defaults as in FileZilla server interface)
        /// </summary>
        public Group()
        {
            BypassUserLimit = TriState.No;
            Enabled = TriState.Yes;
            DisallowedIPs = new List<string>();    
            AllowedIPs = new List<string>();    
            SharedFolders = new List<SharedFolder>();
            DownloadSpeedLimit = new SpeedLimit(GetType() == typeof(Group));
            UploadSpeedLimit = new SpeedLimit(GetType() == typeof(Group));
	        ForceSsl = TriState.Default;
        }

        /// <summary>
        /// Deserialise FileZilla binary data into object
        /// </summary>
        /// <param name="reader">Binary reader to read data from</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public virtual void Deserialize(BinaryReader reader, int protocolVersion, int index)
        {
            GroupName = reader.ReadText();
            IpLimit = reader.ReadInt32();
            UserLimit = reader.ReadInt32();
            var options = reader.ReadByte();
            BypassUserLimit = (TriState)(options & 0x3);
            Enabled = (TriState)((options >> 2) & 0x3);
            DisallowedIPs = reader.ReadTextList();
            AllowedIPs = reader.ReadTextList();
            if (protocolVersion < ProtocolVersions.Sha512)
            {
                EightPlusThree = reader.ReadBoolean();
            }
            SharedFolders = reader.ReadList16<SharedFolder>(protocolVersion);
            DownloadSpeedLimit = reader.Read<SpeedLimit>(protocolVersion);
            UploadSpeedLimit = reader.Read<SpeedLimit>(protocolVersion);
            Comment = reader.ReadText();
	        var forceSsl = reader.ReadByte();
			ForceSsl = (TriState)(forceSsl & 0x3);
        }

        /// <summary>
        /// Serialise object into FileZilla binary data
        /// </summary>
        /// <param name="writer">Binary writer to write data to</param>
        /// <param name="protocolVersion">Current FileZilla protocol version</param>
        /// <param name="index">The 0 based index of this item in relation to any parent list</param>
        public virtual void Serialize(BinaryWriter writer, int protocolVersion, int index)
        {
            // Switch user Enabled 'Default' to 'Yes' when not assigned a group.
            if (Enabled == TriState.Default && GetType() == typeof(User) && string.IsNullOrEmpty(GroupName))
                Enabled=TriState.Yes;

            writer.WriteText(GroupName);
            writer.Write(IpLimit);
            writer.Write(UserLimit);
            byte options = (byte)(((byte)BypassUserLimit & 0x3) | ((byte)Enabled << 2));
            writer.Write(options);
            writer.WriteTextList(DisallowedIPs);
            writer.WriteTextList(AllowedIPs);
            if (protocolVersion < ProtocolVersions.Sha512)
            {
                writer.Write(EightPlusThree);
            }
            writer.WriteList16(SharedFolders, protocolVersion);
            writer.Write(DownloadSpeedLimit, protocolVersion);
            writer.Write(UploadSpeedLimit, protocolVersion);
            writer.WriteText(Comment);
            writer.Write((byte)ForceSsl);
        }
    }
}
