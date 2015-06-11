namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Option Id up to protocol V11. Se FileZilla documentation for further information.
    /// Node that numeric value is one greater than Settings.Options list
    /// </summary>
    public enum OptionIdPreV12
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

        OPTIONS_NUM = 60
#pragma warning restore 1591
        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Option Id for protocol V12. Se FileZilla documentation for further information.
    /// Node that numeric value is one greater than Settings.Options list
    /// </summary>
    public enum OptionIdPostV11
    {
        // ReSharper disable InconsistentNaming
#pragma warning disable 1591
        OPTION_SERVERPORT = 1,
        OPTION_THREADNUM = 2,
        OPTION_MAXUSERS = 3,
        OPTION_TIMEOUT = 4,
        OPTION_NOTRANSFERTIMEOUT = 5,
        OPTION_CHECK_DATA_CONNECTION_IP = 6,
        OPTION_SERVICE_NAME = 7,
        OPTION_SERVICE_DISPLAY_NAME = 8,
        OPTION_TLS_REQUIRE_SESSION_RESUMPTION = 9,
        OPTION_LOGINTIMEOUT = 10,
        OPTION_LOGSHOWPASS = 11,
        OPTION_CUSTOMPASVIPTYPE = 12,
        OPTION_CUSTOMPASVIP = 13,
        OPTION_CUSTOMPASVMINPORT = 14,
        OPTION_CUSTOMPASVMAXPORT = 15,
        OPTION_WELCOMEMESSAGE = 16,
        OPTION_ADMINPORT = 17,
        OPTION_ADMINPASS = 18,
        OPTION_ADMINIPBINDINGS = 19,
        OPTION_ADMINIPADDRESSES = 20,
        OPTION_ENABLELOGGING = 21,
        OPTION_LOGLIMITSIZE = 22,
        OPTION_LOGTYPE = 23,
        OPTION_LOGDELETETIME = 24,
        OPTION_DISABLE_IPV6 = 25,
        OPTION_ENABLE_HASH = 26,
        OPTION_DOWNLOADSPEEDLIMITTYPE = 27,
        OPTION_UPLOADSPEEDLIMITTYPE = 28,
        OPTION_DOWNLOADSPEEDLIMIT = 29,
        OPTION_UPLOADSPEEDLIMIT = 30,
        OPTION_BUFFERSIZE = 31,
        OPTION_CUSTOMPASVIPSERVER = 32,
        OPTION_USECUSTOMPASVPORT = 33,
        OPTION_MODEZ_USE = 34,
        OPTION_MODEZ_LEVELMIN = 35,
        OPTION_MODEZ_LEVELMAX = 36,
        OPTION_MODEZ_ALLOWLOCAL = 37,
        OPTION_MODEZ_DISALLOWED_IPS = 38,
        OPTION_IPBINDINGS = 39,
        OPTION_IPFILTER_ALLOWED = 40,
        OPTION_IPFILTER_DISALLOWED = 41,
        OPTION_WELCOMEMESSAGE_HIDE = 42,
        OPTION_ENABLETLS = 43,
        OPTION_ALLOWEXPLICITTLS = 44,
        OPTION_TLSKEYFILE = 45,
        OPTION_TLSCERTFILE = 46,
        OPTION_TLSPORTS = 47,
        OPTION_TLSFORCEEXPLICIT = 48,
        OPTION_BUFFERSIZE2 = 49,
        OPTION_FORCEPROTP = 50,
        OPTION_TLSKEYPASS = 51,
        OPTION_SHAREDWRITE = 52,
        OPTION_NOEXTERNALIPONLOCAL = 53,
        OPTION_ACTIVE_IGNORELOCAL = 54,
        OPTION_AUTOBAN_ENABLE = 55,
        OPTION_AUTOBAN_ATTEMPTS = 56,
        OPTION_AUTOBAN_TYPE = 57,
        OPTION_AUTOBAN_BANTIME = 58,
        OPTION_TLS_MINVERSION = 59,

        OPTIONS_NUM = 59
#pragma warning restore 1591
        // ReSharper restore InconsistentNaming
    }
}