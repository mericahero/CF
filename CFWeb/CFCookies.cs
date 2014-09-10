
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Web;
namespace COM.CF.Web
{
    /// <summary>
    /// 功能：CF框架Cookie处理
    /// 时间：2013-10-20
    /// 作者：meric
    /// </summary>
    public class CFCookies
    {
        private CFCookies()
        {
        }

        /// <summary>
        /// 清除Cookie，可指定Cookie路径
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public static void ClearCookie(string name, string path ="")
        {
            HttpCookie cookie = new HttpCookie(name);
            HttpContext current = HttpContext.Current;
            cookie.Expires = DateTime.Now.AddDays(-1);
            if (current.Request.Url.Host.ToLower().EndsWith(CFConfig.WWWDomain))
            {
                cookie.Domain = CFConfig.WWWDomain;
            }
            if (!string.IsNullOrWhiteSpace(path))
            {
                cookie.Path = path;
            }
            current.Response.Cookies.Add(cookie);
            current = null;
        }
        /// <summary>
        /// 根据名称获得cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(name);
            if (cookie == null)
            {
                return null;
            }
            return cookie.Value;
        }
        
        /// <summary>
        /// 获得整数表示的Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetIntCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(name);
            if (cookie == null)
            {
                return -1;
            }
            return PubFunc.GetDefaultInt(cookie.Value, -1);
        }

        /// <summary>
        /// 设置cookie，有效期为3个月
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetCookie(string name, string value)
        {
            SetCookie(name, value, DateTime.Now.AddMonths(3));
        }

        /// <summary>
        /// 设置键为name值为value的Cookie值，并设置有效期
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string name, string value, DateTime expires,string path="")
        {
            string curdomain=null;
            HttpContext current = HttpContext.Current;
            if (current.Request.Url.Host.ToLower().EndsWith(CFConfig.WWWDomain))
            {
                curdomain = CFConfig.WWWDomain;
            }
            HttpCookie cookie = current.Response.Cookies[name];
            cookie.Value = value;
            if (curdomain != null)
            {
                cookie.Domain = curdomain;
            }
            if (!string.IsNullOrWhiteSpace(path))
            {
                cookie.Path = path;
            }
            if (expires != DateTime.MaxValue)
            {
                cookie.Expires = expires;
            }            
            cookie = null;
            current = null;
        }


        /// <summary>
        /// 设置Cookie，永不过期
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetCookieNoExpires(string name, string value)
        {
            SetCookie(name, value, DateTime.MaxValue);
        }




    }
}

