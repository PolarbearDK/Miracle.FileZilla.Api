namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// Type of speed limit
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
        FixedLimit = 2,
        /// <summary/>
        UseSpeedLimitRules = 3
    }
}