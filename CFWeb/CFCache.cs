using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using System.Web;
using System.Web.Caching;
using Microsoft.VisualBasic.CompilerServices;
using System.Runtime.CompilerServices;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace COM.CF.Web
{

    public class CFCache
    {
        /// <summary>
        /// 定时清理时间间隔
        /// </summary>
        private const int DingShiRefreshExpireSeconds = 0xbb8;


        private CFCache()
        {
        }

        public static void CheckAuthCache()
        {
            CheckAuthCache(null);
        }

        public static void CheckAuthCache(string selfQuery)
        {
            if (CFConfig.CacheEnable)
            {
                ResponseCatcher catcher = (ResponseCatcher) HttpContext.Current.Cache.Get(GetCacheKey(selfQuery));
                if (catcher == null)
                {
                    HEADEnd();
                }
                else
                {
                    catcher.SendAuthCache();
                }
            }
        }

        public static bool CheckAuthCacheNotEnd()
        {
            return CheckAuthCacheNotEnd(null);
        }

        public static bool CheckAuthCacheNotEnd(string selfQuery)
        {
            if (!CFConfig.CacheEnable)
            {
                return false;
            }
            ResponseCatcher catcher = (ResponseCatcher) HttpContext.Current.Cache.Get(GetCacheKey(selfQuery));
            if (catcher == null)
            {
                return IsHEAD();
            }
            return catcher.SendAuthCache(false);
        }

        public static void CheckCache()
        {
            CheckCache(null);
        }

        public static void CheckCache(string selfQuery)
        {
            if (CFConfig.CacheEnable)
            {
                ResponseCatcher catcher = (ResponseCatcher) HttpContext.Current.Cache.Get(GetCacheKey(selfQuery));
                if (catcher == null)
                {
                    HEADEnd();
                }
                else
                {
                    catcher.SendCache();
                }
            }
        }

        public static bool CheckCacheNotEnd()
        {
            return CheckCacheNotEnd(null);
        }

        public static bool CheckCacheNotEnd(string selfQuery)
        {
            if (!CFConfig.CacheEnable)
            {
                return false;
            }
            ResponseCatcher catcher = (ResponseCatcher) HttpContext.Current.Cache.Get(GetCacheKey(selfQuery));
            if (catcher == null)
            {
                return IsHEAD();
            }
            return catcher.SendCache(false);
        }

        public static void ClearCache(string[] keys)
        {
            Cache cache = HttpContext.Current.Cache;
            foreach (string str in keys)
            {
                cache.Remove(str);
            }
            cache = null;
        }

        public static void ClearCache(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        private static void Depend(string dependkey)
        {
            if (dependkey != null)
            {
                GetKeyObject(dependkey);
            }
        }

        private static void Depend(string[] dependkeys)
        {
            if (dependkeys != null)
            {
                foreach (string str in dependkeys)
                {
                    GetKeyObject(str);
                }
            }
        }


        public static int DingShiRefresh()
        {

            return DingShiRefresh(DateTime.Now.AddSeconds(3000.0));

        }

        public static int DingShiRefresh(DateTime expire)
        {

            return DingShiRefresh(null, expire, null, null);

        }

        public static int DingShiRefresh(DateTime expire, enCacheSave savetype)
        {

            return DingShiRefresh(null, expire, null, null, savetype);

        }

        public static int DingShiRefresh(DateTime expire, string[] dependfiles, string[] dependkeys)
        {
            return DingShiRefresh(null, expire, dependfiles, dependkeys);
        }

        public static int DingShiRefresh(string selfQuery, DateTime expire, string[] dependfiles, string[] dependkeys)
        {
            int num=0;
            if (!CFConfig.CacheEnable)
            {
                return 0;
            }
            Depend(dependkeys);
            ResponseCatcher catcher = new ResponseCatcher(GetCacheKey(selfQuery), false, dependfiles, dependkeys, expire);
            return num;
        }

        public static int DingShiRefresh(string selfQuery, int DurationSec, string[] dependfiles, string[] dependkeys)
        {

            Depend(dependkeys);
            DateTime time = DateTime.Now.AddSeconds((double) DurationSec);
            DateTime time2 = DateTime.Today.AddDays(1.0);
            if (DateTime.Compare(time, time2) > 0)
            {
                return DingShiRefresh(selfQuery, time2, dependfiles, dependkeys);
            }
            return DingShiRefresh(selfQuery, time, dependfiles, dependkeys);
        }

        public static int DingShiRefresh(string selfQuery, DateTime expire, string[] dependfiles, string[] dependkeys, enCacheSave saveType)
        {
            int num=0;
            if (!CFConfig.CacheEnable)
            {
                return 0;
            }
            Depend(dependkeys);
            ResponseCatcher catcher = new ResponseCatcher(GetCacheKey(selfQuery), false, dependfiles, dependkeys, expire, saveType);
            return num;
        }

        public static int DingShiRefresh(string selfQuery, int DurationSec, string[] dependfiles, string[] dependkeys, enCacheSave saveType)
        {
            Depend(dependkeys);
            DateTime time = DateTime.Now.AddSeconds((double) DurationSec);
            DateTime time2 = DateTime.Today.AddDays(1.0);
            if (DateTime.Compare(time, time2) > 0)
            {
                return DingShiRefresh(selfQuery, time2, dependfiles, dependkeys, saveType);
            }
            return DingShiRefresh(selfQuery, time, dependfiles, dependkeys, saveType);

        }

        public static void DisableCurrentCache()
        {
            ResponseCatcher.SetExcept();
        }

        public static string GetCacheKey()
        {
            return ("QQ.URL=" + HttpContext.Current.Request.RawUrl);
        }

        public static string GetCacheKey(string selfkey)
        {
            if (selfkey == null)
            {
                return GetCacheKey();
            }
            return selfkey;
        }

        public static string GetHeadIfModifiedSince()
        {
            string str2 = HttpContext.Current.Request.Headers.Get("If-Modified-Since");
            if (str2 == null)
            {
                return null;
            }
            int index = str2.IndexOf(';');
            if (index >= 0)
            {
                return str2.Substring(0, index);
            }
            return str2;
        }

        public static CacheKeyObject GetKeyObject(string key)
        {
            Cache cache = HttpContext.Current.Cache;
            CacheKeyObject obj2 = (CacheKeyObject) cache.Get(key);
            if (obj2 == null)
            {
                obj2 = new CacheKeyObject();
                if (CFConfig.CacheLogJiBie == enCacheLogJiBie.NoLog)
                {
                    cache.Insert(key, obj2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes((double)CFConfig.CacheDependKeySlidMinutes), CFConfig.CacheKeyDefaultPriority, null);
                }
                else
                {
                    cache.Insert(key, obj2, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes((double)CFConfig.CacheDependKeySlidMinutes), CFConfig.CacheKeyDefaultPriority, new CacheItemRemovedCallback(CFCache.RemovedCallback));
                }
            }
            cache = null;
            return obj2;
        }

        public static void HEADEnd()
        {
            if (HttpContext.Current.Request.HttpMethod == "HEAD")
            {
                HttpContext.Current.Response.End();
            }
        }

        public static bool IsHEAD()
        {
            return (HttpContext.Current.Request.HttpMethod == "HEAD");
        }

        public static bool IsNoCache()
        {
            return HttpContext.Current.Request.Headers["Cache-Control"] == "no-cache";
        }

        public static bool IsRequested()
        {
            HttpContext current = HttpContext.Current;
            SetLastModify(current.Response);
            if (current.Request.Headers["Cache-Control"] == "no-cache")
            {
                return true;
            }
            return (GetHeadIfModifiedSince() != null);
        }

        public static bool IsRequested(int m)
        {
            DateTime time;
            HttpContext current = HttpContext.Current;
            string headIfModifiedSince = GetHeadIfModifiedSince();
            if (headIfModifiedSince == null)
            {
                SetExpires(current.Response, m);
                SetLastModify(current.Response);
                return (current.Request.Headers["Cache-Control"] == "no-cache");
            }
            try
            {
                time = DateTime.Parse(headIfModifiedSince);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                SetExpires(current.Response, m);
                SetLastModify(current.Response);
                return false;

            }
            if (DateTime.Compare(time, DateTime.Now.AddMinutes((double) (0 - m))) < 0)
            {
                SetExpires(current.Response, m);
                SetLastModify(current.Response);
                return false;
            }
            return true;
        }

        public static bool IsRequestedNotSet(int m)
        {
            DateTime time;
            HttpContext current = HttpContext.Current;
            string headIfModifiedSince = GetHeadIfModifiedSince();
            if (headIfModifiedSince == null)
            {
                return false;
            }
            try
            {
                time = DateTime.Parse(headIfModifiedSince);
            }
            catch
            {
                return false;

            }
            if (DateTime.Compare(time, DateTime.Now.AddMinutes((double) (0 - m))) < 0)
            {
                return false;
            }
            return true;
        }

        public static void JingQueDepend(string dependkey)
        {
            JingQueDepend(new string[] { dependkey });
        }

        public static void JingQueDepend(string[] dependkeys)
        {
            JingQueDepend(null, false, null, dependkeys);
        }

        public static void JingQueDepend(string[] dependkeys, DateTime expire)
        {
            if (CFConfig.CacheEnable)
            {
                Depend(dependkeys);
                ResponseCatcher catcher = new ResponseCatcher(GetCacheKey(), false, null, dependkeys, expire) {
                    CacheControl = "private"
                };
            }
        }

        public static void JingQueDepend(string dependkey, DateTime expire)
        {
            JingQueDepend(new string[] { dependkey }, expire);
        }

        public static void JingQueDepend(string selfQuery, bool isAuth, string[] dependfiles, string[] dependkeys)
        {
            JingQueDepend(GetCacheKey(selfQuery), isAuth, dependfiles, dependkeys, TimeSpan.FromHours(1.0));
        }

        public static void JingQueDepend(string selfQuery, bool isAuth, string[] dependfiles, string[] dependkeys, TimeSpan slid)
        {
            if (CFConfig.CacheEnable)
            {
                Depend(dependkeys);
                ResponseCatcher catcher = new ResponseCatcher(GetCacheKey(selfQuery), isAuth, dependfiles, dependkeys, slid) {
                    CacheControl = "private"
                };
            }
        }

        public static void JingQueDepend(string selfQuery, bool isAuth, string[] dependfiles, string[] dependkeys, TimeSpan slid, CacheItemPriority pri)
        {
            if (CFConfig.CacheEnable)
            {
                Depend(dependkeys);
                ResponseCatcher catcher = new ResponseCatcher(GetCacheKey(selfQuery), isAuth, dependfiles, dependkeys, slid, pri) {
                    CacheControl = "private"
                };
            }
        }

        public static void JingTaiPage()
        {
            if (CFConfig.CacheEnable)
            {
                ResponseCatcher catcher = new ResponseCatcher(GetCacheKey(), false, null, null, TimeSpan.FromHours(1.0));
            }
        }

        public static void ObjectJingQueDepend(string key, object o, string[] dependkeys)
        {
            Depend(dependkeys);
            HttpContext.Current.Cache.Insert(key, RuntimeHelpers.GetObjectValue(o), new CacheDependency(null, dependkeys));
        }

        public static void RemovedCallback(string k, object v, CacheItemRemovedReason r)
        {
            switch (CFConfig.CacheLogJiBie)
            {
                case enCacheLogJiBie.NoLog:
                    return;

                case enCacheLogJiBie.OnlyUnderused:
                    if (r == CacheItemRemovedReason.Underused)
                    {
                        break;
                    }
                    return;
            }
            StreamWriter writer = new StreamWriter(CFConfig.logFileDir + "cache.log", true, Encoding.UTF8);
            try
            {
                writer.Write(Conversions.ToString(DateTime.Now) + " " + Enum.GetName(typeof(CacheItemRemovedReason), r) + " " + k + " = ");
                writer.WriteLine(v.ToString());
            }
            finally
            {
                writer.Close();
            }
        }

        public static void Send301(string url, bool endResponse=false)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Status = "301 Moved Permanently";
            response.AddHeader("Location", url);
            response.Write("<head><title>Document Moved</title></head><body><h1>Object Moved</h1>This document may be found <a HREF='" + url + "'>here</a></body>");
            if (endResponse)
            {
                response.End();
            }
            response = null;
        }

        internal static void Send304(string lastModified)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.ClearContent();
            response.StatusCode = 0x130;
            SetLastModify(lastModified);
            response.End();
            response = null;
        }

        public static void Send304NotEnd()
        {
            HttpContext current = HttpContext.Current;
            SetLastModify(GetHeadIfModifiedSince());
            HttpResponse response = current.Response;
            response.ClearContent();
            response.StatusCode = 0x130;
            response = null;
            current = null;
        }

        public static void SendTimeOut304()
        {
            HttpContext current = HttpContext.Current;
            string headIfModifiedSince = GetHeadIfModifiedSince();
            if (headIfModifiedSince != null)
            {
                Send304(headIfModifiedSince);
                current = null;
            }
        }

        public static void SetCacheServer()
        {
            SetCacheServer(240);
        }

        public static void SetCacheServer(int minute)
        {
            HttpContext current = HttpContext.Current;
            current.Response.AppendHeader("Cache-Control", "public");
            SetExpires(current.Response, minute);
            SetLastModify(current.Response);
            current = null;
        }

        public static void SetExpires(HttpResponse response, DateTime expires)
        {
            response.AppendHeader("Expires", expires.ToUniversalTime().ToString("r"));
        }

        public static void SetExpires(HttpResponse response, int m)
        {
            SetExpires(response, DateTime.Now.AddMinutes((double) m).ToUniversalTime().ToString("r"));
        }

        public static void SetExpires(HttpResponse response, string expires)
        {
            response.AppendHeader("Expires", expires);
        }

        public static void SetLastModify()
        {
            SetLastModify(HttpContext.Current.Response);
        }

        public static void SetLastModify(string lastModified)
        {
            SetLastModify(HttpContext.Current.Response, lastModified);
        }

        public static void SetLastModify(HttpResponse response)
        {
            SetLastModify(response, DateTime.Now.ToUniversalTime().ToString("r"));
        }

        public static void SetLastModify(HttpResponse response, string lastModified)
        {
            response.AppendHeader("Last-Modified", lastModified);
        }

        public static void SetPageCount(int id)
        {
            SetLastModify();
            if (!IsRequestedNotSet(240))
            {
                SqlCommand command = new SqlCommand("update pubcount set number=number+1 where id=" + Conversions.ToString(id) + " if @@rowcount=0 insert into pubcount(id)values(" + Conversions.ToString(id) + ")", CFConfig.GetConnection());
                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        public static int SlidingEx(int dursec, DateTime expire, string[] dependfiles)
        {
            return SlidingEx(TimeSpan.FromSeconds((double)dursec), expire, null, false, dependfiles, null, CFConfig.CacheDefaultType);
        }

        public static int SlidingEx(TimeSpan sliding, DateTime expire, string selfQuery, bool isauth, string[] dependfiles, string[] dependkeys, enCacheSave saveType)
        {
            int num=0;
            Depend(dependkeys);
            DateTime time = DateTime.Today.AddDays(1.0);
            if (DateTime.Compare(expire, time) > 0)
            {
                expire = time;
            }
            ResponseCatcher catcher = new ResponseCatcher(GetCacheKey(selfQuery), isauth, dependfiles, dependkeys, expire, sliding, saveType);
            return num;
        }


    }

 
 

}
