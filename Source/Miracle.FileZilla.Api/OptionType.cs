namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Type of option: Text or number
    /// </summary>
    public enum OptionType: byte
    {
        /// <summary>
        /// This is a text option
        /// </summary>
        Text = 0,
        /// <summary>
        /// This is a numeric option
        /// </summary>
        Numeric = 1,
    }
}