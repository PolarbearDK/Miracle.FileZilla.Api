namespace Miracle.FileZilla.Api
{
    internal enum MessageType : byte
    {
        Authenticate = 0x0,
        Error = 0x1,
        ServerState = 0x2,
        UserControl = 0x3,
        NotImplemented4 = 0x4,
        Options = 0x5,
        NotImplemented7 = 0x7,
        AccountSettings = 0x6,
    }
}