namespace Miracle.FileZilla.Api
{
    internal enum CommandType : byte
    {
        Authenticate = 0x0,
        Error = 0x1,
        ServerState = 0x2,
        UserControl = 0x3,
        Options = 0x5,
        Permissions = 0x6,
    }
}