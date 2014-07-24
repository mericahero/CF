using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace COM.CF.Web
{
    public class FenYe
    {
        // Fields
        private bool _DefaultDescDirect;
        private string _FirstLink;
        private NameValueCollection _form;
        private string _IDName;
        private string _keyName;
        private int _nextID;
        private string _nextLink;
        private string _nextQueryString;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SqlConnection _NotOpenConnection;
        private string _preLink;
        private DataRowCollection _Rows;
        public string _strSQL;
        public readonly int Count;
        private bool IsDesc;
        public readonly bool IsFanXiang;
        public readonly bool IsFirst;

        // Methods
        public FenYe(string strsql)
        {
            _DefaultDescDirect = true;
            _Rows = PubFunc.GetSQLRows(strsql, CFConfig.GetNotOpenConnection());
            _nextLink = "";
            _preLink = "";
            IsFanXiang = false;
            IsFirst = false;
        }

        public FenYe(NameValueCollection RequestForm, string idname, string strSQL, int defaultCount, string nextQueryString)
            : this(RequestForm, idname, strSQL, defaultCount, nextQueryString, "")
        {
            int i = idname.IndexOf('.');
            if (i >= 0)
            {
                _keyName = idname.Substring(i + 1);
            }
            else
            {
                _keyName = idname;
            }
        }

        public FenYe(NameValueCollection RequestForm, string idname, string strSQL, int defaultCount, string nextQueryString, string keyname)
        {
            _DefaultDescDirect = true;
            _form = RequestForm;
            IsDesc = RequestForm["o"] != "1";
            IsFanXiang = RequestForm["p"] == "1";
            IsFirst = PubFunc.isNaN(RequestForm["next"], ref _nextID);
            _keyName = keyname;
            _IDName = idname;
            Count = PubFunc.GetDefaultInt(RequestForm["count"], defaultCount);
            if (Count <= 0)
            {
                throw new CFException(enErrType.NormalError, "count必须大于0！");
            }
            _nextQueryString = nextQueryString;
            _strSQL = strSQL;
        }

        public DataRow GetRow(int i)
        {
            if (IsFanXiang)
            {
                return Rows[(Rows.Count - i) - 1];
            }
            return Rows[i];
        }

        // Properties
        public bool DefaultDescDirect
        {
            get
            {
                return _DefaultDescDirect;
            }
            set
            {
                _DefaultDescDirect = value;
                if (value)
                {
                    IsDesc = _form["o"] != "1";
                }
                else
                {
                    IsDesc = _form["o"] == "1";
                }
            }
        }

        public string FirstLink
        {
            get
            {
                if (_FirstLink == null)
                {
                    if ((_nextQueryString != null) && _nextQueryString.StartsWith("&"))
                    {
                        _FirstLink = "?" + _nextQueryString.Substring(1);
                    }
                    else
                    {
                        _FirstLink = "?" + _nextQueryString;
                    }
                    if (DefaultDescDirect)
                    {
                        if (!IsDesc)
                        {
                            _FirstLink = _FirstLink + "&o=1";
                        }
                    }
                    else if (IsDesc)
                    {
                        _FirstLink = _FirstLink + "&o=0";
                    }
                }
                return _FirstLink;
            }
        }

        public string NextLink
        {
            get
            {
                if (_nextLink == null)
                {
                    if (IsFanXiang)
                    {
                        _nextLink = FirstLink + "&next=";
                        if (IsDesc)
                        {
                            _nextLink = _nextLink + Convert.ToString((int)(_nextID + 1));
                        }
                        else
                        {
                            _nextLink = _nextLink + Convert.ToString((int)(_nextID - 1));
                        }
                    }
                    else if (Rows.Count < Count)
                    {
                        _nextLink = "";
                    }
                    else if (FirstLink == "?")
                    {
                        _nextLink = "?next=" + Rows[Rows.Count - 1][_keyName].ToString();
                    }
                    else
                    {
                        _nextLink = FirstLink + "&next=" + Rows[Rows.Count - 1][_keyName].ToString();
                    }
                }
                return _nextLink;
            }
        }

        public SqlConnection NotOpenConnection
        {
            [DebuggerNonUserCode]
            get
            {
                return _NotOpenConnection;
            }
            [DebuggerNonUserCode]
            set
            {
                _NotOpenConnection = value;
            }
        }

        public string PreLink
        {
            get
            {
                if (_preLink == null)
                {
                    if (IsFanXiang)
                    {
                        if (Rows.Count < Count)
                        {
                            _preLink = "";
                        }
                        else
                        {
                            _preLink = FirstLink + "&p=1&next=" + Rows[Rows.Count - 1][_keyName].ToString();
                        }
                    }
                    else if (IsFirst)
                    {
                        _preLink = "";
                    }
                    else
                    {
                        _preLink = FirstLink + "&p=1&next=";
                        if (IsDesc)
                        {
                            _preLink = _preLink + Convert.ToString((int)(_nextID - 1));
                        }
                        else
                        {
                            _preLink = _preLink + Convert.ToString((int)(_nextID + 1));
                        }
                    }
                }
                return _preLink;
            }
        }

        public DataRowCollection Rows
        {
            get
            {
                if (_Rows == null)
                {
                    if (!IsFirst)
                    {
                        if (IsDesc ^ IsFanXiang)
                        {
                            _strSQL = _strSQL + " and " + _IDName + "<" + Convert.ToString(_nextID);
                        }
                        else
                        {
                            _strSQL = _strSQL + " and " + _IDName + ">" + Convert.ToString(_nextID);
                        }
                    }
                    _strSQL = _strSQL + " order by " + _IDName;
                    if (IsDesc ^ IsFanXiang)
                    {
                        _strSQL = _strSQL + " desc";
                    }
                    if (NotOpenConnection == null)
                    {
                        NotOpenConnection = CFConfig.GetNotOpenConnection();
                    }
                    _Rows = PubFunc.GetSQLRows(_strSQL, NotOpenConnection, Count);
                }
                return _Rows;
            }
            set
            {
                _Rows = value;
            }
        }

    }
}
