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
        private static NameValueCollection _appset;
        /// <summary>
        /// 网站的www 服务器
        /// </summary>
        public static string WWWHost;
        /// <summary>
        /// 网站的img0 服务器，存放商品主附图
        /// </summary>
        public static string Img0Host;
        /// <summary>
        /// 网站的img80 服务器，存放UBB编辑器及其它富文本的图片
        /// </summary>
        public static string Img80Host;
        /// <summary>
        /// 网站的img1 服务器，预留图片服务器
        /// </summary>
        public static string Img1Host;
        /// <summary>
        /// 网站的js 服务器 存放js、css等静态文件
        /// </summary>
        public static string JSHost;
        /// <summary>
        /// 网站的login 服务器 处理登录逻辑
        /// </summary>
        public static string LoginHost;
        /// <summary>
        /// 网站admin 服务器 后台管理
        /// </summary>
        public static string AdminHost;
        /// <summary>
        /// 网站的service服务器 提供统计等功能
        /// </summary>
        public static string ServiceHost;
        /// <summary>
        /// 网站的支付服务器
        /// </summary>
        public static string PayHost;
        /// <summary>
        /// 网站的后台管理目录
        /// </summary>
        public static string AdminRoot;
        /// <summary>
        /// Session数据库
        /// </summary>
        public static DB SessionDB;
        /// <summary>
        /// 网站数据库
        /// </summary>
        public static DB XHDB;
        /// <summary>
        /// 用户数据库
        /// </summary>
        public static DB UserDB;




        /// <summary>
        /// 设置配置变量
        /// </summary>
        /// <param name="s">键值对，一般是webconfig里的AppSettings</param>
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
            AdminHost = PubFunc.GetDefaultStr(s["AdminHost"], "http://www.xianhuo365.com");
            ServiceHost = PubFunc.GetDefaultStr(s["ServiceHost"], "http://www.xianhuo365.com");
            AdminRoot = PubFunc.GetDefaultStr(s["AdminRoot"], "admin");
        }

        /// <summary>
        /// 网站的AppSetting
        /// </summary>
        public static NameValueCollection Appset
        {
            get
            {
                return _appset;
            }
        }
    }
}
