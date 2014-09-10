using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF
{
    /// <summary>
    /// 功能：CF框架的一般性枚举
    /// 时间：2013-10-20
    /// 作者：meric
    /// </summary>

    /// <summary>
    /// 缓存日志级别
    ///     不记录
    ///     使用时记录
    ///     都记录
    /// </summary>
    public enum enCacheLogJiBie
    {
        NoLog,
        OnlyUnderused,
        All
    }
    /// <summary>
    /// 缓存方式
    ///     内存缓存
    ///     文件缓存
    /// </summary>
    public enum enCacheSave
    {
        inMem,
        inFile
    }
    /// <summary>
    /// 日志记录级别
    /// </summary>
    public enum enErrorLogJiBie
    {
        Normal,
        NoBusy,
        NoLog,
        Direct2Usr
    }
    /// <summary>
    /// 错误类型
    /// </summary>
    public enum enErrType
    {
        NotLogined = -102,
        UidNotFind = -101,
        NormalError = -100,
        DevelopError = -99
    }

    /// <summary>
    /// xml错误码
    /// </summary>
    public enum enXMLErrorCode
    {
        CFError = -100,
        SystemError = -110,
        SystemBusy = -120,
        UnKonwError = -150

    }
}
