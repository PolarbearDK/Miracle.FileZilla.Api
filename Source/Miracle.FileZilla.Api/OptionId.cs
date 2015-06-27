namespace Miracle.FileZilla.Api
{
    /// <summary>
    /// Option Id. Se FileZilla documentation for further information.
    /// Note that numeric value is one greater than Settings.Options list
    /// Order of elements has been changed as of Protocol V12 (Server version 9.52)
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
        BUFFERSIZE2 = 49,
        FORCEPROTP = 50,
        SHAREDWRITE = 52,
        NOEXTERNALIPONLOCAL = 53,
        ACTIVE_IGNORELOCAL = 54,
        AUTOBAN_ENABLE = 55,
        AUTOBAN_ATTEMPTS = 56,
        AUTOBAN_TYPE = 57,
        AUTOBAN_BANTIME = 58,

        // protocol versions before V12
        V11_INFXP = 6,
        V11_OUTFXP = 7,
        V11_NOINFXPSTRICT = 8,
        V11_NOOUTFXPSTRICT = 9,
        V11_ENABLESSL = 43,
        V11_ALLOWEXPLICITSSL = 44,
        V11_SSLKEYFILE = 45,
        V11_SSLCERTFILE = 46,
        V11_SSLPORTS = 47,
        V11_SSLFORCEEXPLICIT = 48,
        V11_SSLKEYPASS = 51,
        V11_SERVICE_NAME = 59,
        V11_SERVICE_DISPLAY_NAME = 60,

        V11_OPTIONS_NUM = 60,

        // Specific to protocol version V12
        V12_CHECK_DATA_CONNECTION_IP = 6,
        V12_SERVICE_NAME = 7,
        V12_SERVICE_DISPLAY_NAME = 8,
        V12_TLS_REQUIRE_SESSION_RESUMPTION = 9,
        V12_ENABLETLS = 43,
        V12_ALLOWEXPLICITTLS = 44,
        V12_TLSKEYFILE = 45,
        V12_TLSCERTFILE = 46,
        V12_TLSPORTS = 47,
        V12_TLSFORCEEXPLICIT = 48,
        V12_TLSKEYPASS = 51,
        V12_TLS_MINVERSION = 59,

        V12_OPTIONS_NUM = 59

#pragma warning restore 1591
        // ReSharper restore InconsistentNaming
    }
}