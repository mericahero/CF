using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using COM.CF;


namespace CWS
{
    /// <summary>
    /// 功能：Common Web Site 的一系列属性设置
    /// 时间：2013-10-22
    /// 作者：meric
    /// </summary>
    public class CWConfig
    {
        // 定义CommonWeb的系列静态属性
        private static NameValueCollection _appset;
        public static string WWWHost;
        public static string Img0Host;
        public static string Img80Host;
        public static string Img1Host;
        public static string JSHost;
        public static string LoginHost;
        public static string AdminHost;
        public static string ServiceHost;
        public static string PayHost;
    
        public static DB SessionDB;
        public static DB XHDB;
        public static DB UserDB;




        //设置配置变量
        public static void SetConfig(NameValueCollection s)
        {
            _appset = s;
            SessionDB = new DB(s["SessionConnectionString"]);
            UserDB = new DB(s["UserConnectionString"]);
            XHDB = new DB(s["XHConnectionString"]);
            WWWHost = PubFunc.GetDefaultStr(s["WWWHost"], "http://www.xianhuo365.com");
            JSHost = PubFunc.GetDefaultStr(s["JSHost"], "http://www.xianhuo365.com");
            Img0Host = PubFunc.GetDefaultStr(s["Img0Host"], "http://www.xianhuo365.com");
            Img80Host = PubFunc.GetDefaultStr(s["Img80Host"], "http://www.xianhuo365.com");
            Img1Host = PubFunc.GetDefaultStr(s["Img1Host"], "http://www.xianhuo365.com");
            LoginHost = PubFunc.GetDefaultStr(s["LoginHost"], "http://www.xianhuo365.com");
            PayHost = PubFunc.GetDefaultStr(s["PayHost"], "http://www.xianhuo365.com");
            AdminHost = PubFunc.GetDefaultStr(s["HtadminHost"], "http://www.xianhuo365.com");
            ServiceHost = PubFunc.GetDefaultStr(s["ServiceHost"], "http://www.xianhuo365.com");        
        }

        // Properties
        public static NameValueCollection Appset
        {
            get
            {
                return _appset;
            }
        }
    }
}
