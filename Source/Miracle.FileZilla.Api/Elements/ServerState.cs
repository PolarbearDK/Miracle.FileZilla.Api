using System;

namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// FileZilla server state
    /// </summary>
    [Flags]
    public enum ServerState : ushort
    {
        /// <summary>
        /// Server is online
        /// </summary>
        Online = 0x01,
        /// <summary>
        /// Go offline immediately
        /// </summary>
        GoOfflineNow = 0x02,
        /// <summary>
        /// Go offline at logout?
        /// </summary>
        GoOfflineLogout = 0x04,
        /// <summary>
        /// Go offline after all transfers are done?
        /// </summary>
        GoOfflineWaitTransfer = 0x08,
        /// <summary>
        /// Lock server
        /// </summary>
        Locked = 0x10,
    }
}