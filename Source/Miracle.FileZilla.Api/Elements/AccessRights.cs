using System;

namespace Miracle.FileZilla.Api.Elements
{
    /// <summary>
    /// FTP rights to files/folder
    /// </summary>
    [Flags]
    public enum AccessRights : ushort
    {
        /// <summary/>
        DirCreate	= 0x0001,
        /// <summary/>
        DirDelete = 0x0002,
        /// <summary/>
        DirList = 0x0004,
        /// <summary/>
        DirSubdirs = 0x0008,
        /// <summary/>
        FileAppend = 0x0010,
        /// <summary/>
        FileDelete = 0x0020,
        /// <summary/>
        FileRead = 0x0040,
        /// <summary/>
        FileWrite = 0x0080,
        /// <summary/>
        IsHome = 0x0100,
        /// <summary/>
        AutoCreate = 0x0200,
    }
}