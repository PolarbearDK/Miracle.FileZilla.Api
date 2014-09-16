namespace Miracle.FileZilla.Api
{
    internal enum UserControl : byte
    {
        /// <summary>
        /// Get list of connections
        /// </summary>
        GetList = 0,
        /// <summary>
        /// Huh?!?
        /// </summary>
        ConNop = 1, 
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