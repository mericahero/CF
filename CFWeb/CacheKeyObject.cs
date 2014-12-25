using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace COM.CF.Web
{
    public class CacheKeyObject
    {
        private DateTime _createRQ = DateAndTime.Now;
        private ListDictionary _dict;
        private DateTime _lastaccess;
        private int _n;

        public bool HaveKey(object key)
        {
            if (this._dict == null)
            {
                return false;
            }
            return this._dict.Contains(RuntimeHelpers.GetObjectValue(key));
        }

        public override string ToString()
        {
            IEnumerator enumerator=null;
            string str = "KEY OBJECT access=" + Conversions.ToString(this._n) + " LM=" + Conversions.ToString(this._createRQ) + " LA=" + Conversions.ToString(this._lastaccess);
            if (this._dict == null)
            {
                return str;
            }
            str = str + "<div class=item>";
            try
            {
                enumerator = this._dict.Keys.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                    if (this._dict[RuntimeHelpers.GetObjectValue(objectValue)] == null)
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

        public object this[object key]
        {
            get
            {
                this._lastaccess = DateAndTime.Now;
                this._n++;
                if (this._dict == null)
                {
                    return null;
                }
                return this._dict[RuntimeHelpers.GetObjectValue(key)];
            }
            set
            {
                if (this._dict == null)
                {
                    this._dict = new ListDictionary();
                }
                this._dict[RuntimeHelpers.GetObjectValue(key)] = RuntimeHelpers.GetObjectValue(value);
            }
        }
    }
}

