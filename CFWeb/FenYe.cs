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
    /// <summary>
    /// 功能：前台分页程序
    /// 时间：2013-10-2
    /// 作者：Meric
    /// 
    /// 修正：2013-12-1 Meric 加上获取本页面的顶端值，以修正第一页仍有上一页的链接的问题
    /// 
    /// 使用：该分页程序不能输出页码，只能输出上一页，下一页的页面链接，在构造本类时，需要传递SQL查询参数，不带order by，程序会根据传入的IDName以及页面的p o参数来自动差别数据的正序还是倒序
    /// </summary>
    public class FenYe
    {
        /// <summary>
        /// 是否默认倒序
        /// </summary>
        private bool _defaultDescDirect;
        /// <summary>
        /// 第一页的链接，不带参数
        /// </summary>
        private string _firstLink;
        /// <summary>
        /// 传递过来的页面参数
        /// </summary>
        private NameValueCollection _form;
        /// <summary>
        /// 主键字段名 可以加前缀
        /// </summary>
        private string _IDName;
        /// <summary>
        /// 去掉前缀的主键字段
        /// </summary>
        private string _keyName;
        /// <summary>
        /// 当前搜索条件下的顶端值，多一次查询以修正第一页还有上一页的问题
        /// </summary>
        private long _topIDValue;
        /// <summary>
        /// 下一页的第一条记录ID值
        /// </summary>
        private int _nextID;
        /// <summary>
        /// 下一页的页面链接 
        /// </summary>
        private string _nextLink;
        /// <summary>
        /// 页面请求参数
        /// </summary>
        private string _nextQueryString;
        /// <summary>
        /// 未打开的数据库查询连接
        /// </summary>
        private SqlConnection _notOpenConnection;
        /// <summary>
        /// 上一页链接
        /// </summary>
        private string _preLink;
        /// <summary>
        /// 当前页面的DataRows
        /// </summary>
        private DataRowCollection _rows;
        /// <summary>
        /// 可以查看请求的Sql语句
        /// </summary>
        public string _strSQL;

        private string _originSql;
        /// <summary>
        /// 页面的记录条数
        /// </summary>
        public readonly int Count;
        /// <summary>
        /// 是否倒序
        /// </summary>
        private bool IsDesc;
        /// <summary>
        /// 是否反向显示在页面中
        /// </summary>
        public readonly bool IsFanXiang;
        /// <summary>
        /// 当前页是否是首页
        /// </summary>
        public readonly bool IsFirst;



        #region 构造函数
        /// <summary>
        /// 构造函数 只传入sql语句
        /// </summary>
        /// <param name="strsql"></param>
        public FenYe(string strsql)
        {
            _defaultDescDirect = true;
            _rows = PubFunc.GetSQLRows(strsql, CFConfig.GetNotOpenConnection());
            _nextLink = "";
            _preLink = "";
            IsFanXiang = false;
            IsFirst = false;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="RequestForm">请求表单</param>
        /// <param name="idname">主键</param>
        /// <param name="strSQL">SQL</param>
        /// <param name="defaultCount">每一页的数量</param>
        /// <param name="nextQueryString">页面的请求参数</param>
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
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="RequestForm">请求参数</param>
        /// <param name="idname">主键</param>
        /// <param name="strSQL">sql</param>
        /// <param name="defaultCount">每页显示数量</param>
        /// <param name="nextQueryString">页面请求参数</param>
        /// <param name="keyname">键名</param>
        public FenYe(NameValueCollection RequestForm, string idname, string strSQL, int defaultCount, string nextQueryString, string keyname)
        {
            _defaultDescDirect = true;
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
            _originSql = strSQL;
        }
        #endregion


        /// <summary>
        /// 按索引取出记录
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DataRow GetRow(int i)
        {
            if (IsFanXiang)
            {
                return Rows[(Rows.Count - i) - 1];
            }
            return Rows[i];
        }

        /// <summary>
        /// 是否默认反向
        /// </summary>
        public bool DefaultDescDirect
        {
            get
            {
                return _defaultDescDirect;
            }
            set
            {
                //是否倒序
                IsDesc = value ? _form["o"]!="1" : _form["o"]=="1";
                _defaultDescDirect = value;                
            }
        }

        /// <summary>
        /// 获取当前查询条件下的顶端ID值
        /// 增加 修正 首页有上一页问题
        /// </summary>
        public long TopIDValue
        {
            get 
            {
                if (_topIDValue == 0)
                {
                    var tempsql=_originSql+ " order by " + _IDName + (IsDesc?" desc" : "");
                    tempsql = tempsql.Insert(tempsql.IndexOf("select ") + "select ".Length, " top 1 ");
                    var row = PubFunc.GetSQLSingleRow(tempsql, NotOpenConnection);
                    _topIDValue = PubFunc.GetBigInt(row[_keyName]);
                }
                return _topIDValue; 
            }
        }
        /// <summary>
        /// 首页的链接
        /// </summary>
        public string FirstLink
        {
            get
            {
                if (_firstLink == null)
                {
                    if ((_nextQueryString != null) && _nextQueryString.StartsWith("&"))
                    {
                        _firstLink = "?" + _nextQueryString.Substring(1);
                    }
                    else
                    {
                        _firstLink = "?" + _nextQueryString;
                    }
                    if (DefaultDescDirect)
                    {
                        if (!IsDesc)
                        {
                            _firstLink = _firstLink + "&o=1";
                        }
                    }
                    else if (IsDesc)
                    {
                        _firstLink = _firstLink + "&o=0";
                    }
                }
                return _firstLink;
            }
        }
        /// <summary>
        /// 下一页的链接
        /// </summary>
        public string NextLink
        {
            get
            {
                if (_nextLink == null)
                {
                    if (IsFanXiang)
                    {
                        _nextLink = FirstLink + "&next=" + (IsDesc ? (_nextID+1) : (_nextID-1));
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
        /// <summary>
        /// 分页的数据库连接
        /// </summary>
        public SqlConnection NotOpenConnection
        {
            get
            {
                if (_notOpenConnection == null)
                {
                    _notOpenConnection = CFConfig.GetNotOpenConnection();
                }
                return _notOpenConnection;
            }
            set
            { 
                _notOpenConnection = value;
            }
        }
        /// <summary>
        /// 上一页链接
        /// </summary>
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
                        //增加 修正 首页有上一页问题
                        if (!(IsDesc ^ PubFunc.GetBigInt(Rows[Rows.Count - 1][_keyName]) >= TopIDValue)) {_preLink = "";return "";};
                        
                    }
                    else if (IsFirst)
                    {
                        _preLink = "";
                    }
                    else
                    {
                        if (!(IsDesc ^ _nextID >= TopIDValue)) { _preLink = ""; return ""; };
                        //增加 修正 首页有上一页问题                        
                        _preLink = FirstLink + "&p=1&next=";
                        if (IsDesc)
                        {
                            _preLink = _preLink + (_nextID - 1);
                        }
                        else
                        {
                            _preLink = _preLink + (_nextID + 1);
                        }

                    }
                }
                return _preLink;
            }
        }
        /// <summary>
        /// 数据Rows
        /// </summary>
        public DataRowCollection Rows
        {
            get
            {
                _strSQL = _originSql;
                if (_rows == null)
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
                    _rows = PubFunc.GetSQLRows(_strSQL, NotOpenConnection, Count);
                }
                return _rows;
            }
            set
            {
                _rows = value;
            }
        }

    }
}
