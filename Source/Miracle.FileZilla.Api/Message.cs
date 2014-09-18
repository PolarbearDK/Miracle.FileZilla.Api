using System;
using System.Linq;
using Miracle.FileZilla.Api.Elements;

namespace Miracle.FileZilla.Api
{
    internal class Message
    {
        public MessageOrigin MessageOrigin { get; private set; }
        public MessageType MessageType { get; private set; }
        public byte[] RawData { get; private set; }
        public object Body;

        public Message(MessageOrigin messageOrigin, MessageType messageType, byte[] rawData, int protocolVersion)
        {
            MessageOrigin = messageOrigin;
            MessageType = messageType;
            RawData = rawData;
            Body = ParseMessage(messageOrigin, messageType, rawData, protocolVersion);
        }

        private static object ParseMessage(MessageOrigin messageOrigin, MessageType messageType, byte[] data, int protocolVersion)
        {
            switch (messageOrigin)
            {
                case MessageOrigin.Client:
                    break;
                case MessageOrigin.Server:
                    switch (messageType)
                    {
                        case MessageType.Authenticate:
                            break;
                        case MessageType.Error:
                            return data.Read<Error>(protocolVersion);
                        case MessageType.ServerState:
                            return data.Read(reader => (ServerState) reader.ReadBigEndianInt16());
                        case MessageType.UserControl:
                            var userControl = (UserControl)data[0];
                            data = data.Skip(1).ToArray();
                            switch (userControl)
                            {
                                case UserControl.GetList:
                                    return data.Read(reader => reader.ReadList<Connection>(protocolVersion));
                                case UserControl.ConNop:
                                    break;
                                case UserControl.Kick:
                                    return data.Read(reader => reader.ReadByte() == 0);
                                case UserControl.BanIp:
                                    return data.Read(reader => reader.ReadByte() == 0);
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case MessageType.NotImplemented4:
                            break;
                        case MessageType.Options:
                            break;
                        case MessageType.NotImplemented7:
                            break;
                        case MessageType.AccountSettings:
                            return data.Length == 1 
                                ? (object) (data[0] == 0) 
                                : data.Read<AccountSettings>(protocolVersion);
                        default:
                            throw new ArgumentOutOfRangeException("messageType");
                    }
                    break;
                case MessageOrigin.ServerMessage:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }
    }
}