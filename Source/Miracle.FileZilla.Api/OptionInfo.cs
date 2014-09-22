namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Description about an option
    /// </summary>
    public class OptionInfo
    {
        /// <summary>
        /// Text related to an option
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// Type of option
        /// </summary>
        public OptionType OptionType { get; private set; }
        /// <summary>
        /// Is this property changable from non local address (127.0.0.1/::1)
        /// </summary>
        public bool NotRemotelyChangeable { get; private set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="notRemotelyChangeable"></param>
        internal OptionInfo(string text, int type, bool notRemotelyChangeable)
        {
            Text = text;
            OptionType = (OptionType)type;
            NotRemotelyChangeable = notRemotelyChangeable;
        }
    }
}