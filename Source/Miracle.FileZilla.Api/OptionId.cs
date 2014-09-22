namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Option Id. Se FileZilla documentation for further information.
    /// Node that numeric value is one greater than Settings.Options list
    /// </summary>
    public enum OptionId
    {
        // ReSharper disable InconsistentNaming
#pragma warning disable 1591
        SERVERPORT = 1,
        THREADNUM = 2,
        MAXUSERS = 3,
        TIMEOUT = 4,
        NOTRANSFERTIMEOUT = 5,
        INFXP = 6,
        OUTFXP = 7,
        NOINFXPSTRICT = 8,
        NOOUTFXPSTRICT = 9,
        LOGINTIMEOUT = 10,
        LOGSHOWPASS = 11,
        CUSTOMPASVIPTYPE = 12,
        CUSTOMPASVIP = 13,
        CUSTOMPASVMINPORT = 14,
        CUSTOMPASVMAXPORT = 15,
        WELCOMEMESSAGE = 16,
        ADMINPORT = 17,
        ADMINPASS = 18,
        ADMINIPBINDINGS = 19,
        ADMINIPADDRESSES = 20,
        ENABLELOGGING = 21,
        LOGLIMITSIZE = 22,
        LOGTYPE = 23,
        LOGDELETETIME = 24,
        DISABLE_IPV6 = 25,
        ENABLE_HASH = 26,
        DOWNLOADSPEEDLIMITTYPE = 27,
        UPLOADSPEEDLIMITTYPE = 28,
        DOWNLOADSPEEDLIMIT = 29,
        UPLOADSPEEDLIMIT = 30,
        BUFFERSIZE = 31,
        CUSTOMPASVIPSERVER = 32,
        USECUSTOMPASVPORT = 33,
        MODEZ_USE = 34,
        MODEZ_LEVELMIN = 35,
        MODEZ_LEVELMAX = 36,
        MODEZ_ALLOWLOCAL = 37,
        MODEZ_DISALLOWED_IPS = 38,
        IPBINDINGS = 39,
        IPFILTER_ALLOWED = 40,
        IPFILTER_DISALLOWED = 41,
        WELCOMEMESSAGE_HIDE = 42,
        ENABLESSL = 43,
        ALLOWEXPLICITSSL = 44,
        SSLKEYFILE = 45,
        SSLCERTFILE = 46,
        SSLPORTS = 47,
        SSLFORCEEXPLICIT = 48,
        BUFFERSIZE2 = 49,
        FORCEPROTP = 50,
        SSLKEYPASS = 51,
        SHAREDWRITE = 52,
        NOEXTERNALIPONLOCAL = 53,
        ACTIVE_IGNORELOCAL = 54,
        AUTOBAN_ENABLE = 55,
        AUTOBAN_ATTEMPTS = 56,
        AUTOBAN_TYPE = 57,
        AUTOBAN_BANTIME = 58,
        SERVICE_NAME = 59,
        SERVICE_DISPLAY_NAME = 60,
#pragma warning restore 1591
        // ReSharper restore InconsistentNaming
    }
}