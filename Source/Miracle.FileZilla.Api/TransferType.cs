namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Direction of a FTP transfer
    /// </summary>
    public enum TransferType
    {
        /// <summary>
        /// Data is flowing from client to server
        /// </summary>
        Receive = 0,
        /// <summary>
        /// Data is flowing from server to client
        /// </summary>
        Send = 1,
    }
}