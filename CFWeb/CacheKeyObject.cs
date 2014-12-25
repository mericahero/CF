using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace COM.CF.Web
{
    /// <summary>
    /// 功能：CFCache的Key对象
    /// 时间：2013-10-2
    /// 作者：meric
    /// </summary>
    public class CacheKeyObject
    {
        private DateTime _createRQ = DateAndTime.Now;
        private ListDictionary _dict;
        private DateTime _lastaccess;
        private int _n;
        /// <summary>
        /// 判断是否包含Key
        /// </summary>
        /// <param name="key">需要检测的key</param>
        /// <returns>返回是否包含该key</returns>
        public bool HaveKey(object key)
        {
            if (this._dict == null)
            {
                return false;
            }
            return this._dict.Contains(RuntimeHelpers.GetObjectValue(key));
        }

        /// <summary>
        /// 重写Object的ToString()方法
        /// </summary>
        /// <returns>返回重写的ToString()</returns>
        public override string ToString()
        {
            IEnumerator enumerator=null;
            string str = string.Format("KEY OBJECT access={0} LM={1} LA={2}",_n,_createRQ,_lastaccess);
            if (_dict == null)
            {
                return str;
            }
            str +=  "<div class=item>";
            try
            {
                enumerator = this._dict.Keys.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                    if (_dict[RuntimeHelpers.GetObjectValue(objectValue)] == null)
                    {
                        str = str + " " + objectValue.ToString() + "=Nothing";
                    }
                    else
                    {
                        str = str + " " + objectValue.ToString() + "=" + this._dict[RuntimeHelpers.GetObjectValue(objectValue)].ToString();
                    }
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
            return (str + "</div>");
        }
        /// <summary>
        /// 索引器，根据Key得到值
        /// </summary>
        /// <param name="key">要检索的key</param>
        /// <returns>检索到的值</returns>
        public object this[object key]
        {
            get
            {
                _lastaccess = DateAndTime.Now;
                _n++;
                if (_dict == null)
                {
                    return null;
                }
                return _dict[RuntimeHelpers.GetObjectValue(key)];
            }
            set
            {
                if (_dict == null)
                {
                    _dict = new ListDictionary();
                }
                _dict[RuntimeHelpers.GetObjectValue(key)] = RuntimeHelpers.GetObjectValue(value);
            }
        }
    }
}

