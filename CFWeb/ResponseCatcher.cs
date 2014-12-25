using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Web.Caching;
using COM.CF;
using System.Web;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace COM.CF.Web
{

    /// <summary>
    /// 功能：CF框架的基础缓存
    /// 时间：2014-10-1
    /// 作者：merci
    /// </summary>
    internal class ResponseCatcher : Stream
    {        
        private string _charset;
        private string _contentType;
        private long _position;
        private Stream _sink;
        private readonly DateTime absoluteExpiration;
        private ArrayList Ary;
        public string CacheControl;
        private CacheDependency dependencies;
        private FileStream fs;        
        private readonly bool isRequestAuth;
        private readonly string[] keys;
        private DateTime lastAccess;
        private readonly string LastModify;
        private string m_cacheFileName;
        private readonly enCacheSave m_Save;
        private int m_send304;
        private int m_sendSave;
        private readonly string mykey;
        private readonly CacheItemPriority priority;
        private readonly DateTime rq;
        private readonly TimeSpan slidingExpiration;
        private bool isExcept;
        
        private ResponseCatcher(string query, bool isauth)
        {
            this.isExcept = false;
            this.absoluteExpiration = Cache.NoAbsoluteExpiration;
            this.slidingExpiration = Cache.NoSlidingExpiration;
            this.priority = CFConfig.CacheItemDefaultPriority;
            this.m_Save = CFConfig.CacheDefaultType;
            this.CacheControl = "public";
            this.isRequestAuth = isauth;
            HttpContext current = HttpContext.Current;
            this._sink = current.Response.Filter;
            current.Response.Filter = this;
            this.rq = DateTime.Now;
            this.LastModify = this.rq.ToUniversalTime().ToString("r");
            current.Response.AppendHeader("Last-Modified", this.LastModify);
            current = null;
            this.mykey = query;
        }

        public ResponseCatcher(string query, bool isAuth, string[] dependfiles, string[] keys)
            : this(query, isAuth)
        {
            this.dependencies = new CacheDependency(dependfiles, keys);
            this.keys = keys;
        }

        public ResponseCatcher(string query, bool isAuth, string[] dependfiles, string[] keys, DateTime expire)
            : this(query, isAuth, dependfiles, keys)
        {
            this.absoluteExpiration = expire;
        }

        public ResponseCatcher(string query, bool isAuth, string[] dependfiles, string[] keys, TimeSpan slid)
            : this(query, isAuth, dependfiles, keys)
        {
            this.slidingExpiration = slid;
        }

        public ResponseCatcher(string query, bool isAuth, string[] dependfiles, string[] keys, DateTime expire, enCacheSave saveType)
            : this(query, isAuth, dependfiles, keys)
        {
            this.absoluteExpiration = expire;
            this.m_Save = saveType;
        }

        public ResponseCatcher(string query, bool isAuth, string[] dependfiles, string[] keys, TimeSpan slid, CacheItemPriority priority)
            : this(query, isAuth, dependfiles, keys, slid)
        {
            this.priority = priority;
        }

        public ResponseCatcher(string query, bool isAuth, string[] dependfiles, string[] keys, DateTime expire, TimeSpan sliding, enCacheSave saveType)
            : this(query, isAuth, dependfiles, keys)
        {
            this.absoluteExpiration = expire;
            this.slidingExpiration = sliding;
            this.m_Save = saveType;
        }

        public override void Close()
        {
            this.finish();
            this._sink.Close();
            this._sink = null;
        }

        private void finish()
        {
            if (HttpContext.Current.Response.ContentType != "text/vnd.wap.wml")
            {
                if (this.m_Save == enCacheSave.inFile)
                {
                    this.fs.Close();
                    this.fs = null;
                    if (!this.isExcept)
                    {
                        HttpContext.Current.Cache.Insert(this.mykey, this, this.dependencies, this._AbsoluteExpiration, this.slidingExpiration, this.priority, new CacheItemRemovedCallback(this.RemovedCallback));
                    }
                }
                else if (!this.isExcept)
                {
                    if (CFConfig.CacheLogJiBie == enCacheLogJiBie.NoLog)
                    {
                        HttpContext.Current.Cache.Insert(this.mykey, this, this.dependencies, this._AbsoluteExpiration, this.slidingExpiration, this.priority, null);
                    }
                    else
                    {
                        HttpContext.Current.Cache.Insert(this.mykey, this, this.dependencies, this._AbsoluteExpiration, this.slidingExpiration, this.priority, new CacheItemRemovedCallback(CFCache.RemovedCallback));
                    }
                }
            }
            this.dependencies = null;
            HttpContext current = HttpContext.Current;
            this.SendCacheControl(current.Response);
            if (DateTime.Compare(this.absoluteExpiration, Cache.NoAbsoluteExpiration) != 0)
            {
                CFCache.SetExpires(current.Response, absoluteExpiration);
            }
            this._contentType = current.Response.ContentType;
            this._charset = current.Response.Charset;
            current = null;
        }

        public override void Flush()
        {
            this._sink.Flush();
        }

        public override int Read(byte[] MyBuffer, int offset, int count)
        {
            //int num;
            return _sink.Read(MyBuffer, offset, count);
            //return num;
        }

        private void ReGetKeys()
        {
            if (this.keys != null)
            {
                Cache cache = HttpContext.Current.Cache;
                foreach (string str in this.keys)
                {
                    cache.Get(str);
                }
                cache = null;
            }
        }

        private void RemovedCallback(string k, object v, CacheItemRemovedReason r)
        {
            CFCache.RemovedCallback(k, RuntimeHelpers.GetObjectValue(v), r);
            if (this.m_Save == enCacheSave.inFile)
            {
                File.Delete(this.m_cacheFileName);
            }
        }

        private void Save2File(byte[] MyBuffer, int offset, int count)
        {
            if (this.m_cacheFileName == null)
            {
                Random random = new Random();
                this.m_cacheFileName = CFConfig.CacheFileDir + @"\" + random.Next(CFConfig.CacheFileDirNumber).ToString("x") + @"\";
                Directory.CreateDirectory(this.m_cacheFileName);
                while (true)
                {
                    string str = random.Next().ToString("x");
                    try
                    {
                        this.fs = File.Open(this.m_cacheFileName + str, FileMode.CreateNew, FileAccess.Write);
                        this.m_cacheFileName = this.m_cacheFileName + str;
                        break;
                    }
                    catch (IOException exception1)
                    {
                        ProjectData.SetProjectError(exception1);
                        IOException exception = exception1;
                        ProjectData.ClearProjectError();
                    }
                }
            }
            this.fs.Write(MyBuffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin direction)
        {
            return this._sink.Seek(offset, direction);
        }

        private void Send304(HttpResponse response)
        {
            this.m_send304++;
            this.SendCacheControl(response);
            HttpResponse response2 = response;
            response2.ClearContent();
            response2.StatusCode = 0x130;
            response2.Charset = this._charset;
            response2.ContentType = this._contentType;
            CFCache.SetLastModify(response, this.LastModify);
            response2 = null;
        }

        public bool SendAuthCache()
        {
            return this.SendAuthCache(true);
        }

        public bool SendAuthCache(bool isEnd)
        {
            if ((!TimeSpan.Equals(this.slidingExpiration, Cache.NoSlidingExpiration) && (DateTime.Compare(this.absoluteExpiration, Cache.NoAbsoluteExpiration) != 0)) && (DateTime.Compare(DateTime.Now, this.absoluteExpiration) > 0))
            {
                HttpContext.Current.Cache.Remove(this.mykey);
                return false;
            }
            this.lastAccess = DateTime.Now;
            this.ReGetKeys();
            HttpContext current = HttpContext.Current;
            if (CFConfig.Enable304 && (this.LastModify == CFCache.GetHeadIfModifiedSince()))
            {
                this.Send304(current.Response);
            }
            else
            {
                this.SendSaved(current.Response);
            }
            if (isEnd)
            {
                current.Response.End();
            }
            current = null;
            return true;
        }

        public void SendCache()
        {
            this.SendCache(true);
        }

        public bool SendCache(bool isEnd)
        {
            if (this.isRequestAuth)
            {
                return false;
            }
            return this.SendAuthCache(isEnd);
        }

        private void SendCacheControl(HttpResponse response)
        {
            HttpResponse response2 = response;
            if (!this.isRequestAuth && (this.CacheControl == "public"))
            {
                response2.AppendHeader("Cache-Control", this.CacheControl);
            }
            response2 = null;
        }

        private void SendSaved(HttpResponse response)
        {
            this.m_sendSave++;
            HttpResponse response2 = response;
            response2.ClearContent();
            response2.ContentType = this._contentType;
            response2.Charset = this._charset;
            if (DateTime.Compare(this.absoluteExpiration, Cache.NoAbsoluteExpiration) != 0)
            {
                CFCache.SetExpires(response, absoluteExpiration);
            }
            CFCache.SetLastModify(response, LastModify);
            this.SendCacheControl(response);
            if (this.m_Save == enCacheSave.inMem)
            {
                IEnumerator enumerator = null;
                try
                {
                    enumerator = this.Ary.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        byte[] current = (byte[])enumerator.Current;
                        response2.BinaryWrite(current);
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }
            }
            else
            {
                response2.WriteFile(this.m_cacheFileName, false);
            }
            response2 = null;
        }

        public static void SetExcept()
        {
            if (HttpContext.Current.Response.Filter is ResponseCatcher)
            {
                ((ResponseCatcher)HttpContext.Current.Response.Filter).isExcept = true;
            }
        }

        public override void SetLength(long length)
        {
            this._sink.SetLength(length);
        }

        public override string ToString()
        {
            string str = "";
            if (this.m_sendSave > 0)
            {
                str = this.m_sendSave.ToString();
            }
            if (this.m_send304 > 0)
            {
                str = str + ":" + Conversions.ToString(this.m_send304);
            }
            if (str != "")
            {
                str = "<font color=red>" + str + "</font>";
            }
            if (this.isRequestAuth)
            {
                str = str + " <font color=red>Auth</font>";
            }
            return ("Send:304=" + str + " " + Enum.GetName(typeof(enCacheSave), this.m_Save) + " size=" + Conversions.ToString(this.TotleSize()) + " LM=" + Conversions.ToString(this.rq) + " LA=" + Conversions.ToString(this.lastAccess));
        }

        private int TotleSize()
        {
            if (this.m_Save == enCacheSave.inMem)
            {
                IEnumerator enumerator = null;
                int num2 = 0;
                try
                {
                    enumerator = this.Ary.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        byte[] current = (byte[])enumerator.Current;
                        num2 += current.Length;
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }
                return num2;
            }
            return Convert.ToInt32(new FileInfo(this.m_cacheFileName).Length);
        }

        public override void Write(byte[] MyBuffer, int offset, int count)
        {
            if (this.m_Save == enCacheSave.inMem)
            {
                byte[] dst = new byte[(count - 1) + 1];
                Buffer.BlockCopy(MyBuffer, offset, dst, 0, count);
                if (this.Ary == null)
                {
                    this.Ary = new ArrayList(8);
                }
                this.Ary.Add(dst);
            }
            else
            {
                this.Save2File(MyBuffer, offset, count);
            }
            this._sink.Write(MyBuffer, offset, count);
        }

        // Properties
        private DateTime _AbsoluteExpiration
        {
            get
            {
                if ((DateTime.Compare(this.absoluteExpiration, Cache.NoAbsoluteExpiration) != 0) && !TimeSpan.Equals(this.slidingExpiration, Cache.NoSlidingExpiration))
                {
                    return Cache.NoAbsoluteExpiration;
                }
                return this.absoluteExpiration;
            }
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return 0;
            }
        }

        public override long Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }
    }





}
