using System;

namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// Flags enumeration representing the days of the week
    /// </summary>
    [Flags]
    public enum Days: byte
    {
        /// <summary/>
        Monday = 0x1,
        /// <summary/>
        Tuesday = 0x2,
        /// <summary/>
        Wednesday = 0x4,
        /// <summary/>
        Thursday = 0x8,
        /// <summary/>
        Friday = 0x10,
        /// <summary/>
        Saturday = 0x20,
        /// <summary/>
        Sunday = 0x40,
        /// <summary/>
        Workdays = 0x1F,
        /// <summary/>
        Weekend = 0x60,
        /// <summary/>
        All = 0x7F,
    }
}