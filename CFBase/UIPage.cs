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

        protected ILoginUsr UsrLogin { get; set; }

        protected UIPage()
        {
            this.Error += new EventHandler(JScriptPagePage_Error);
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

        public string DelQueryName(string delkey)
        {
            return PubFunc.DelQueryName(Request.Url.Query, delkey); ;
        }

        public string DelQueryNameAndNext(string delkey)
        {
            return PubFunc.DelQueryName(PubFunc.DelQueryName(Request.Url.Query, delkey), "next");
        }






        protected int GetPathID()
        {
            string pathInfo = Request.PathInfo;
            if (pathInfo == "") return PubFunc.GetInt(RequestForm["id"]);
            return pathInfo.IndexOf('.') < 0 ? 0 : PubFunc.GetInt(pathInfo.Substring(1,pathInfo.IndexOf('.')-1));
        }


        protected string GetPathStr()
        {
            string pathInfo = Request.PathInfo;
            if (pathInfo == "") return "";

            return pathInfo.LastIndexOf('.') < 0 ? pathInfo.Substring(1) : pathInfo.Substring(1, pathInfo.LastIndexOf('.') - 1);
        }

        protected abstract void HandleException(CFException e);

        private void JScriptPagePage_Error(object sender, EventArgs e)
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
