
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Web;
namespace COM.CF.Web
{
    public class CFCookies
    {
        private CFCookies()
        {
        }

        public static void ClearCookie(string name, string path ="")
        {
            HttpCookie cookie = new HttpCookie(name);
            HttpContext current = HttpContext.Current;
            cookie.Expires = new DateTime(0x8c1220247e44000L);
            if (current.Request.Url.Host.ToLower().EndsWith(CFConfig.WWWDomain))
            {
                cookie.Domain = CFConfig.WWWDomain;
            }
            if (path != null)
            {
                cookie.Path = path;
            }
            current.Response.Cookies.Add(cookie);
            current = null;
        }

        public static string GetCookie(string cookie)
        {
            HttpCookie cookie2 = HttpContext.Current.Request.Cookies.Get(cookie);
            if (cookie2 == null)
            {
                return null;
            }
            return cookie2.Value;
        }

        public static int GetCookieCID(bool autocreate = true)
        {
            int intCookie = GetIntCookie("CID");
            if ((intCookie == -1) && autocreate)
            {
                return GetNewCid();
            }
            return intCookie;
        }

        public static int GetCookieUUID()
        {
            return GetIntCookie("uuid");
        }

        public static int GetIntCookie(string cookie)
        {
            HttpCookie cookie2 = HttpContext.Current.Request.Cookies[cookie];
            if (cookie2 == null)
            {
                return -1;
            }
            return PubFunc.GetDefaultInt(cookie2.Value, -1);
        }

        private static int GetNewCid()
        {
            return GetNewCid(0);
        }

        public static int GetNewCid(int cid)
        {
            string userAgent = HttpContext.Current.Request.UserAgent;
            if (FWFunc.IsBot(userAgent))
            {
                return -1;
            }
            SqlCommand command = new SqlCommand("p_new_cookie", CFConfig.GetConnection());
            try
            {
                int num3;
                command.CommandType = CommandType.StoredProcedure;
                SqlParameterCollection parameters = command.Parameters;
                parameters.Add(new SqlParameter("@cid", SqlDbType.Int));
                parameters.Add(new SqlParameter("@browser", SqlDbType.VarChar, 0xff)).Value = userAgent;
                parameters = null;
                Random random = new Random();
                int num2 = 0;
                do
                {
                    if (cid == 0)
                    {
                        cid = random.Next() + 1;
                    }
                    try
                    {
                        command.Parameters[0].Value = cid;
                        command.ExecuteNonQuery();
                        goto Label_00F4;
                    }
                    catch (Exception exception1)
                    {
                        ProjectData.SetProjectError(exception1);
                        Exception exception2 = exception1;
                        cid = 0;
                        ProjectData.ClearProjectError();
                    }
                    num2++;
                    num3 = 10;
                }
                while (num2 <= num3);
            }
            finally
            {
                command.Connection.Close();
            }
        Label_00F4:
            if (cid != 0)
            {
                SetCookieTmpCID(cid);
                return cid;
            }
        CFException e = new CFException(enErrType.NormalError, "产生Cookies错误！");
            ErrorLog.WriteLog(e);
            throw e;
        }

        private static int ReSetCookieUUID(int uuid)
        {
            string qQDomain=null;
            HttpContext current = HttpContext.Current;
            if (current.Request.Url.Host.ToLower().EndsWith(CFConfig.WWWDomain))
            {
                qQDomain = CFConfig.WWWDomain;
            }
            HttpCookie cookie = current.Response.Cookies["uuid"];
            cookie.Value = uuid.ToString();
            if (qQDomain != null)
            {
                cookie.Domain = qQDomain;
            }
            cookie = null;
            current = null;
            return uuid;
        }

        public static void SetCookie(string name, string value)
        {
            SetCookie(name, value, DateTime.Now.AddMonths(3));
        }

        public static void SetCookie(string name, string value, DateTime expires)
        {
            string qQDomain=null;
            HttpContext current = HttpContext.Current;
            if (current.Request.Url.Host.ToLower().EndsWith(CFConfig.WWWDomain))
            {
                qQDomain = CFConfig.WWWDomain;
            }
            HttpCookie cookie = current.Response.Cookies[name];
            cookie.Value = value;
            cookie.Expires = expires;
            if (qQDomain != null)
            {
                cookie.Domain = qQDomain;
            }
            cookie = null;
            current = null;
        }

        public static void SetCookieCID(int cid)
        {
            SetCookie("CID", cid.ToString());
            string cookie = GetCookie("QC");
            if (cookie != null)
            {
                SetCookie("QC", cookie);
            }
        }

        public static void SetCookieNoExpires(string name, string value)
        {
            string qQDomain=null;
            HttpContext current = HttpContext.Current;
            if (current.Request.Url.Host.ToLower().EndsWith(CFConfig.WWWDomain))
            {
                qQDomain = CFConfig.WWWDomain;
            }
            HttpCookie cookie = current.Response.Cookies[name];
            cookie.Value = value;
            if (qQDomain != null)
            {
                cookie.Domain = qQDomain;
            }
            cookie = null;
            current = null;
        }

        public static void SetCookieTmpCID(int cid)
        {
            SetCookie("CID", cid.ToString(), DateAndTime.Now.AddHours(24.0));
        }

        public static int SetCookieUUID()
        {
            return ReSetCookieUUID(new Random().Next() + 1);
        }
    }
}

