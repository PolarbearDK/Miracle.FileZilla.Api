namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// "User" (actually connection) control.
    /// Sub command of MessageType.UserControl 
    /// </summary>
    public enum UserControl : byte
    {
        /// <summary>
        /// Get list of connections
        /// </summary>
        GetList = 0,
        /// <summary>
        /// Connection operation. See ConnOp for sub types
        /// </summary>
        ConnOp = 1, 
        /// <summary>
        /// Kick user connection
        /// </summary>
        Kick = 2,
        /// <summary>
        /// Ban user IP (and kick)
        /// </summary>
        BanIp = 3,
    }
}