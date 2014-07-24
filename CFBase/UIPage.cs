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

        public string DelQueryName(string delkey)
        {
            return PubFunc.DelQueryName(Request.Url.Query, delkey); ;
        }

        public string DelQueryNameAndNext(string delkey)
        {
            return PubFunc.DelQueryName(PubFunc.DelQueryName(Request.Url.Query, delkey), "next");
        }

        /// <summary>
        /// 获取左侧字符
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="n">左侧字符数</param>
        /// <returns></returns>
        protected string GetLeftStr(string s, int n)
        {
            if (s == null) return null;
            if (s.Length <= n) return s;
            return String.Format("<span title=\"{0}\">{1}...</span>",s,s.Substring(0,n));

        }

        /// <summary>
        /// 获取左侧字符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        protected string GetLeftStr(string s, int n, string link)
        {
            return  GetLeftStr(s, n, link, "");
        }
        /// <summary>
        /// 获取左侧字符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        /// <param name="link"></param>
        /// <param name="othercanshu"></param>
        /// <returns></returns>
        protected string GetLeftStr(string s, int n, string link, string othercanshu="")
        {
            if(s==null) return "";
            return String.Format("<a href=\"{0}\" {1} {2}>{3}</a>",link,othercanshu,s.Length<=n ? "" : "title="+ s,s.Length<=n ? s : s.Substring(0,n) +"...");
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

        protected int RequestInt(string name)
        {
            int num = 0;
            if (PubFunc.isNaN(RequestForm[name], ref num)) throw new CFException(enErrType.NormalError, string.Concat(name, " 必须提供！"));
            return num;

        }
    }
}
