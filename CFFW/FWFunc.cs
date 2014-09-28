using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.VisualBasic.CompilerServices;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Data;
using System.Web.Caching;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;

namespace COM.CF
{
    /// <summary>
    /// 功能：网络，爬虫，页面相关公共类
    /// 时间：2013-10-20
    /// 作者：meric
    /// </summary>
    public class FWFunc
    {
        #region 基本定义
        private static Random _r = new Random();
        private static int[] _randAry = new int[] { 20, 15, 15, 10, 10 };
        private static string[] anQuanbotAry = new string[] { "baiduspider", "googlebot", "yahoo", "qqbot", "sogou", "msnbot", "yodaobot", "yisou" };
        private static string[] botAry = new string[] { 
            "google", "isaac", "bot", "slurp", "spider", "baidu", "archiver", "p.arthur", "crawler", "java", "gather", "sleipnir", "yahoo", "3721", "yisou", "sohu", 
            "openfind", "aol"
         };
        #endregion
       

        // Methods


        public static bool CheckNeiRong2(string rules, string text)
        {
            if (rules != "")
            {
                if (text == null)
                {
                    text = "";
                }
                if (Regex.IsMatch(text, rules, RegexOptions.Singleline | RegexOptions.IgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }




        public static string EncodeUrl(string s)
        {
            return HttpUtility.UrlEncode(s, Encoding.UTF8);
        }


   
        /// <summary>
        /// 获得访问ip
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            HttpRequest request = HttpContext.Current.Request;
            string input = request.ServerVariables["HTTP_CDN_SRC_IP"];
            if (input == null)
            {
                input = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (input == null)
                {
                    return request.UserHostAddress;
                }
            }
            input = input.Replace("127.0.0.1,", "").Replace("127.0.0.1:", "").Trim();
            if ((input != "") && (input.IndexOf("unknown") <= 0))
            {
                input = input.Split(new char[] { ',', ':', ' ' })[0];
                if (Regex.IsMatch(input, @"^\d{1,3}(\.\d{1,3}){3}$"))
                {
                    return input;
                }
            }
            return request.UserHostAddress;
        }

        /// <summary>
        /// 获得IIS版本
        /// </summary>
        /// <returns></returns>
        public static double GetIisVersion()
        {
            double r = -1;

            Version ver = System.Environment.OSVersion.Version;

            if (ver.Major == 4 && ver.Minor == 0)
            {
                r = 4.0;
            }
            else if (ver.Major == 5)
            {
                if (ver.Minor == 0)
                {
                    r = 5.0;
                }
                else if (ver.Minor == 1)
                {
                    r = 5.1;
                }
                else if (ver.Minor == 2)
                {
                    r = 6.0;
                }
            }
            else if (ver.Major == 6)
            {
                if (ver.Minor == 0)
                {
                    r = 7.0;
                }
                else if (ver.Minor == 1)
                {
                    r = 7.5;
                }
            }

            return r;
        }


        /// <summary>
        /// 根据后缀名获得mime类型
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string GetMIME(string ext)
        {
            switch (ext.ToLower())
            {
                case ".jpg":
                case ".jpge":
                case ".jpe":
                    return "image/jpeg";

                case ".png":
                case ".gif":
                case ".bmp":
                    return ("image/" + ext.Substring(1));

                case ".zip":
                    return "application/x-zip-compressed";

                case ".doc":
                    return "application/msword";

                case ".ram":
                case ".ra":
                    return "audio/x-pn-realaudio";

                case ".mp3":
                    return "audio/mpeg";

                case ".mpg":
                case ".mpa":
                case ".mpeg":
                case ".mpe":
                    return "video/mpeg";

                case ".rm":
                    return "application/vnd.rn-realmedia";

                case ".htm":
                case ".html":
                    return "text/html";

                case ".aif":
                    return "audio/x-aiff";

                case ".au":
                    return "audio/basic";

                case ".wave":
                    return "audio/wav";

                case ".asf":
                    return "video/x-ms-asf";

                case ".mid":
                case ".rmi":
                    return "audio/mid";

                case ".torrent":
                    return "application/x-bittorrent";
            }
            return "application/octet-stream";
        }














        public static bool IsBot()
        {
            return IsBot(HttpContext.Current.Request.UserAgent);
        }

        public static bool IsBot(string b)
        {
            return IsSafeBot(b);
            //if (b == null)
            //{
            //    return true;
            //}
            //b = b.ToLower();
            //foreach (string str in botAry)
            //{
            //    if (b.Contains(str))
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        public static bool IsBotQQ()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (IsBot(request.UserAgent))
            {
                return true;
            }
            return (request.Cookies["QQbot"] != null);
        }

        public static bool IsSafeBot()
        {
            bool flag=false;
            HttpRequest request = HttpContext.Current.Request;
            if (IsSafeBot(request.UserAgent) || (request.QueryString["qqbot"] == "1"))
            {
                return true;
            }
            request = null;
            return flag;
        }

        public static bool IsSafeBot(string b)
        {
            if (b == null)
            {
                return true;
            }
            b = b.ToLower();
            foreach (string str in anQuanbotAry)
            {
                if (b.Contains(str))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
