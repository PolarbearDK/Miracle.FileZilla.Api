namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Connection operation (sub command of UserControl.ConnOp)
    /// </summary>
    public enum ConnOp
    {
        /// <summary>
        /// Connection added
        /// </summary>
        Add = 0,
        /// <summary>
        /// User changed in active connection
        /// </summary>
        ChangeUser = 1,
        /// <summary>
        /// Connection removed
        /// </summary>
        Remove = 2,
        /// <summary>
        /// Transfer started or stopped
        /// </summary>
        TransferInit = 3,
        /// <summary>
        /// File offset for each active file transfer
        /// </summary>
        TransferOffsets = 4,
    }
}