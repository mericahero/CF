using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Caching;
using GeniusTek;
using System.Data;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace COM.CF
{
    public class CFConfig
    {
        public static string AppPath
        {
            get
            {
                return m_appPath;
            }
        }

        public static string ConnectionString
        {
            get
            {
                return m_connectionString;
            }
            set
            {
                m_connectionString = value;
            }
        }

        public static string DefaultChengXuJiName
        {
            get
            {
                return m_defaultChengXuJiName;
            }
        }

        public static string DefaultNameSpace
        {
            get
            {
                return m_defaultNameSpace;
            }
        }

        public static enAnalyzeType JieXiType
        {
            get
            {
                return (enAnalyzeType)m_jieXiType;
            }
        }

        // Nested Types
        public enum enPageCacheType
        {
            enServerMemJingQue,
            enServerMemDingShi,
            enSQUID,
            enNone
        }



        public static enCacheSave CacheDefaultType = enCacheSave.inFile;
        public static int CacheDependKeySlidMinutes = 60;
        public static bool CacheEnable = true;
        public static string CacheFileDir = @"C:\CFCache";
        public static int CacheFileDirNumber = 0x100;
        public static CacheItemPriority CacheItemDefaultPriority = CacheItemPriority.Normal;
        public static CacheItemPriority CacheKeyDefaultPriority = CacheItemPriority.Normal;
        public static enCacheLogJiBie CacheLogJiBie = enCacheLogJiBie.OnlyUnderused;

        public static bool Enable304 = true;
        public static enErrorLogJiBie ErrorLogJiBie = enErrorLogJiBie.Normal;
        
        public static byte HomeID;
        public static bool IsBig5 = false;
        public static string logFileDir = @"f:\log\";

        private static string m_connectionString;
        private static string m_appPath;
        private static string m_defaultChengXuJiName;
        private static string m_defaultNameSpace;
        private static int m_jieXiType;

        public static string WWWDomain;
        public static string CFTestHost = "127.0.0.1";
        public static string RQFormatStr = "yyyy-MM-dd HH:mm";
        public static string RQNoTimeFormatStr = "yyyy-MM-dd";       






        public static void CheckHome(byte homeid)
        {
            if (homeid != HomeID)
            {
                HttpContext current = HttpContext.Current;
                string str2 = current.Request.Url.Host.ToLower();
                if (str2 != CFTestHost)
                {
                    string homeHost = GetHomeHost(homeid);
                    if (homeHost != str2)
                    {
                        if (IsBig5)
                        {
                            homeHost = homeHost + ":81";
                        }
                        if (current.Request.UrlReferrer != null)
                        {
                            current.Response.Write("<a href=http://" + homeHost + current.Request.UrlReferrer.AbsolutePath + current.Request.UrlReferrer.Query + ">正确连接</a><BR>");
                            throw new CFException(enErrType.NormalError, "当前请求提交到的主机不对！请尝试使用上面连接重新提交请求！");
                        }
                        throw new CFException(enErrType.NormalError, "当前请求提交到的主机不对！<BR><a href=/cgi-bin/>QQ首页</a>");
                    }
                    current = null;
                }
            }
        }

        private static void ConfigWeb()
        {
            ServerClass.Init(DefaultNameSpace, JieXiType, DefaultChengXuJiName);
        }



        public static SqlConnection GetConnection()
        {
            SqlConnection connection2 = new SqlConnection(ConnectionString);
            connection2.Open();
            return connection2;
        }



        public static string GetHomeHost(byte homeid)
        {
            if (HttpContext.Current != null)
            {
                string str3;
                string str2 = HttpContext.Current.Request.Url.Host.ToLower();
                int index = str2.IndexOf(".");
                if (index >= 0)
                {
                    str3 = str2.Substring(0, index);
                }
                else
                {
                    str3 = str2;
                }
                if ((str3.Length == 2) && (str3.StartsWith("d") || str3.StartsWith("t")))
                {
                    return (str3.Substring(0, 1) + Convert.ToString(homeid) + "." + WWWDomain);
                }
            }
            return ("w" + Convert.ToString(homeid) + "." + WWWDomain);
        }



        public static SqlConnection GetNotOpenConnection()
        {
            return new SqlConnection(ConnectionString);
        }


        public static string GetStr(NameValueCollection form, string name)
        {
            if (IsBig5)
            {
                return PubTypes.DelBadUnionCodeString(form[name]);
            }
            return PubTypes.DelBadUnionCodeString(form[name]);
        }

        public static string GetStr(NameValueCollection form, string name, int maxlength)
        {
            string str = GetStr(form, name);
            if (str.Length > maxlength)
            {
                return str.Substring(0, maxlength);
            }
            return str;
        }

        public static string GetStr(NameValueCollection form, string name, int maxlength, bool isVarchar)
        {
            string str = GetStr(form, name);
            if (Encoding.Default.GetByteCount(str) > maxlength)
            {
                throw new CFException(enErrType.NormalError, "字段[" + name + "]字符超长!最多" + Conversions.ToString(maxlength) + "字节");
            }
            return str;
        }

        public static string GetXMLError(enErrType errType, string errstr)
        {
            return ("<error errcode='" + (int)errType + "'>" + HttpUtility.HtmlEncode(errstr) + "</error>");
        }

        public static string GetXMLError(enErrType errType, string errstr, enErrType qqerrtype)
        {
            return ("<error errcode='" + (int)errType + "' errtype='" + Enum.GetName(typeof(enErrType), qqerrtype) + "'>" + HttpUtility.HtmlEncode(errstr) + "</error>");
        }


        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void InitConfig()
        {
            m_connectionString = WebConfigurationManager.AppSettings["ConnectionString"];
            m_defaultNameSpace = PubFunc.GetDefaultValue(WebConfigurationManager.AppSettings, "DefaultNameSpace", "");
            m_jieXiType = PubFunc.GetDefaultInt(WebConfigurationManager.AppSettings["JieXiType"], 3);
            m_defaultChengXuJiName = PubFunc.GetDefaultValue(WebConfigurationManager.AppSettings, "DefaultChengXuJiName", "");
            logFileDir = PubFunc.GetDefaultValue(WebConfigurationManager.AppSettings, "logFileDir", logFileDir);
            HomeID = PubFunc.GetByte(WebConfigurationManager.AppSettings["HomeID"]);
            WWWDomain = WebConfigurationManager.AppSettings["WWWDomain"] ?? "cf.com";
            ConfigWeb();
        }



        public static void SysLog(string s)
        {
            StreamWriter writer = new StreamWriter(logFileDir + Conversions.ToString(HomeID) + @"\sys.log", true, Encoding.UTF8);
            try
            {
                writer.WriteLine(Conversions.ToString(DateTime.Now) + " " + s);
            }
            finally
            {
                writer.Close();
            }
        }

        public static void WrtieLog(string logfile, string logStr)
        {
            StreamWriter writer = new StreamWriter(logFileDir + Conversions.ToString(HomeID) + @"\" + logfile + ".log", true, Encoding.UTF8);
            try
            {
                writer.Write(DateTime.Now);
                writer.WriteLine(" URL=" + HttpContext.Current.Request.RawUrl);
                writer.WriteLine(logStr);
            }
            finally
            {
                writer.Close();
            }
        }
    }
}
