namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// OptionType of speed limit
    /// </summary>
    public enum SpeedLimitType: byte
    {
        /// <summary>
        /// Use default/inherited. Not allowed anywhere in Group object graph.
        /// </summary>
        Default = 0,
        /// <summary/>
        NoLimit = 1,
        /// <summary/>
        ConstantSpeedLimit = 2,
        /// <summary/>
        UseSpeedLimitRules = 3
    }
}