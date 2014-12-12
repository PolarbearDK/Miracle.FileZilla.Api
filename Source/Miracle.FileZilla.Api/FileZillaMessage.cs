using System;
using System.Linq;

namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// FileZilla message
    /// </summary>
    public class FileZillaMessage
    {
        /// <summary>
        /// Origin of message
        /// </summary>
        public MessageOrigin MessageOrigin { get; private set; }
        /// <summary>
        /// Type of message
        /// </summary>
        public MessageType MessageType { get; private set; }
        /// <summary>
        /// The raw data bytes of FileZilla Message
        /// </summary>
        public byte[] RawData { get; private set; }
        /// <summary>
        /// Serialized version of RawData
        /// </summary>
        public object Body { get { return _body.Value; } }
        private readonly Lazy<object> _body;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageOrigin">Origin of message</param>
        /// <param name="messageType">Type of message</param>
        /// <param name="rawData">The raw data bytes of FileZilla Message</param>
        /// <param name="protocolVersion">server protocol version</param>
        public FileZillaMessage(MessageOrigin messageOrigin, MessageType messageType, byte[] rawData, int protocolVersion)
        {
            MessageOrigin = messageOrigin;
            MessageType = messageType;
            RawData = rawData;
            _body = new Lazy<object>(() => Deserialize(protocolVersion));

#if DEBUG
            // Force immediate load of body during debugging
            var dummy = _body.Value;
#endif
        }

        private object Deserialize(int protocolVersion)
        {
            switch (MessageOrigin)
            {
                case MessageOrigin.ClientRequest:
                    return null;
                case MessageOrigin.ServerReply:
                case MessageOrigin.ServerMessage:
                    switch (MessageType)
                    {
                        case MessageType.Authenticate:
                            return null;
                        case MessageType.Error:
                            return RawData.Read<Error>(protocolVersion);
                        case MessageType.ServerState:
                            return RawData.Read(reader => (ServerState) reader.ReadBigEndianInt16());
                        case MessageType.UserControl:
                            var userControl = (UserControl)RawData[0];
                            var data = RawData.Skip(1).ToArray();
                            switch (userControl)
                            {
                                case UserControl.GetList:
                                    return data.Read(reader => reader.ReadList16<Connection>(protocolVersion));
                                case UserControl.ConnOp:
                                    var connOpType = (ConnOp)data[0];
                                    data = data.Skip(1).ToArray();
                                    switch (connOpType)
                                    {
                                        case ConnOp.Add:
                                            return data.Read<ConnectionAdded>(protocolVersion);
                                        case ConnOp.ChangeUser:
                                            return data.Read<ConnectionUserChanged>(protocolVersion);
                                        case ConnOp.Remove:
                                            return data.Read<ConnectionRemoved>(protocolVersion);
                                        case ConnOp.TransferInit:
                                            return data.Read<Transfer>(protocolVersion);
                                        case ConnOp.TransferOffsets:
                                            return data.ReadArray<ConnectionTransferOffsets>(protocolVersion);
                                        default:
                                            throw new ApiException("Unknown ConnOp: " + connOpType);
                                    }
                                case UserControl.Kick:
                                    return data.Read(reader => reader.ReadByte() == 0);
                                case UserControl.BanIp:
                                    return data.Read(reader => reader.ReadByte() == 0);
                                default:
                                    throw new ApiException("Unknown UserControl: " + userControl);
                            }
                        case MessageType.Event:
                            return RawData.Read<Event>(protocolVersion);
                        case MessageType.Settings:
                            return RawData.Length == 1 
                                ? (object) (RawData[0] == 0) 
                                : RawData.Read<Settings>(protocolVersion);
                        case MessageType.Transfer:
                            return RawData.Read<TransferInfo>(protocolVersion);
                        case MessageType.AccountSettings:
                            return RawData.Length == 1 
                                ? (object) (RawData[0] == 0) 
                                : RawData.Read<AccountSettings>(protocolVersion);
                        case MessageType.Loopback:
                            return null;
                        default:
                            throw new ApiException("Unknown MessageType: " + MessageType);
                    }
                default:
                    throw new ApiException("Unknown MessageOrigin: " + MessageOrigin);
            }
        }
    }
}