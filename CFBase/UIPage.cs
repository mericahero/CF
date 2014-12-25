using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using COM.CF.Web;

namespace COM.CF
{
    /// <summary>
    /// 功能：抽象类UIPage，定义页面的基本属性和方法，所有CF框架类的基类CFPage继承此抽象类
    /// 时间：2013-10-22
    /// 作者：meric
    /// </summary>
    public abstract class UIPage : Page
    {
        private NameValueCollection m_Form;
        /// <summary>
        /// RequestForm 请求参数封装
        /// </summary>
        protected NameValueCollection RequestForm
        {
            get
            {
                if (m_Form == null)
                {
                    if (Request.HttpMethod == "POST")
                    {
                        m_Form = Request.Form;
                    }
                    else
                    {
                        m_Form = Request.QueryString;
                    }
                }
                return m_Form;
            }
        }
        /// <summary>
        /// 登录用户 
        /// </summary>
        protected ILoginUsr UsrLogin { get; set; }
        /// <summary>
        /// 构造参数
        /// </summary>
        protected UIPage()
        {
            this.Error += new EventHandler(Page_Error);
        }


        private DynamicViewDataDictionary _dynamicViewData;
        /// <summary>
        /// ViewBag和ViewData为用户在使用页面时可自定义自增加的属性
        /// </summary>
        public dynamic ViewBag
        {
            get
            {
                return _dynamicViewData = _dynamicViewData ?? new DynamicViewDataDictionary();
            }
            set
            {
                _dynamicViewData = value;
            }
        }

        private IDictionary<object, object> _viewData;
        /// <summary>
        /// ViewBag和ViewData为用户在使用页面时可自定义自增加的属性
        /// </summary>
        public IDictionary<object, object> ViewData
        {
            get
            {
                return _viewData = _viewData ?? new Dictionary<object, object>();
            }
            set
            {
                _viewData = value;
            }
        }
        /// <summary>
        /// 从QueryString中删除指定的键
        /// </summary>
        /// <param name="delkey">需要删除的键</param>
        /// <returns>返回删除键后的QueryString</returns>
        public string DelQueryName(string delkey)
        {
            return PubFunc.DelQueryName(Request.Url.Query, delkey); ;
        }
        /// <summary>
        /// 从QueryString中删除指定的键
        /// </summary>
        /// <param name="delkey">需要删除的键</param>
        /// <returns>返回删除键后的QueryString</returns>
        public string DelQueryNameAndNext(string delkey)
        {
            return PubFunc.DelQueryName(PubFunc.DelQueryName(Request.Url.Query, delkey), "next");
        }

        /// <summary>
        /// 获取URL的PathID
        /// </summary>
        /// <returns>返回请求的PathID</returns>
        protected int GetPathID()
        {
            string pathInfo = Request.PathInfo;
            if (pathInfo == "") return PubFunc.GetInt(RequestForm["id"]);
            return pathInfo.IndexOf('.') < 0 ? 0 : PubFunc.GetInt(pathInfo.Substring(1,pathInfo.IndexOf('.')-1));
        }

        /// <summary>
        /// 获得Path字符串
        /// </summary>
        /// <returns>返回Path字符串</returns>
        protected string GetPathStr()
        {
            string pathInfo = Request.PathInfo;
            if (pathInfo == "") return "";

            return pathInfo.LastIndexOf('.') < 0 ? pathInfo.Substring(1) : pathInfo.Substring(1, pathInfo.LastIndexOf('.') - 1);
        }
        /// <summary>
        /// 抽象方法，具体类里实现，处理页面异常
        /// </summary>
        /// <param name="e"></param>
        protected abstract void HandleException(CFException e);

        /// <summary>
        /// 页面错误接管
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Error(object sender, EventArgs e)
        {
            Exception error = HttpContext.Current.Error;
            if (error is CFException)
            {
                HandleException((CFException)error);
            }
            else
            {
                COM.CF.Web.ErrorLog.UnControlException(new CFPageControl(Context), error);
                HttpContext.Current.ClearError();
            }
        }
    }
}
