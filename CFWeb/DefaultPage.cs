using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI;

namespace COM.CF.Web
{
    /// <summary>
    /// 功能：CF框架底层页面抽象类
    /// 时间：2013-10-2
    /// 作者：meric
    /// </summary>
    public abstract class DefaultPage:Page
    {
        private NameValueCollection m_Form;

        private CFPageControl m_webForm;
        /// <summary>
        /// RequestForm 根据请求类型（post/get）封装了不同的请求参数，当请求为post时，封装Request.Form对象，get时封装Request.QueryString对象
        /// </summary>
        protected NameValueCollection RequestForm
        {
            get
            {
                if (m_Form==null)
                {
                    if (Request.HttpMethod=="POST")
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
        /// 页面标题
        /// </summary>
        protected new string Title
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        /// <summary>
        /// 页面控制对象
        /// </summary>
        protected CFPageControl WebForm
        {
            get
            {
                if (m_webForm == null)
                {
                    m_webForm = new CFPageControl(Context);
                }
                return m_webForm;
            }
        }

       
        protected DefaultPage()
        {
            base.Load += Page_Load;
        }
        /// <summary>
        /// 入口点函数
        /// </summary>
        protected abstract void EventMain();
        /// <summary>
        /// 页面加载函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (CFConfig.IsBig5)
                {
                    Response.ContentType = "text/html;charset=big5";
                }
                WebForm.WriteHead();
                EventMain();
                WebForm.WriteTail();
            }
            catch (CFException ex)
            {
                if (ex.ErrType==enErrType.NotLogined)
                {
                    WriteLogin();
                    Response.Write("</body></html>");
                    Response.End();
                }
                WebForm.WriteError(ex.ErrType, ex.Message);

            }
        }
        /// <summary>
        /// 输出登录链接
        /// </summary>
        protected void WriteLogin()
        {
            Response.Write("<BR><center><font color=red size=+2>你还没有登录！</font></center><HR><font color=blue>提示</font>：<BR>你可以马上<a href=\"javascript:newin('/cgi-bin/sys/autolog/autolog.asp','_blank',300,300)\">去登录</a>登录成功后可以直接[<a href=javascript:document.location.reload()>Reload</a>]，重发该请求。<BR><BR>或者你现在返回，输入你的注册ID和口令。<BR><BR><BR>如果还没有注册，则请先<a target=_blank href=/cgi-bin/friends/reg0.htm>去注册</a>");
        }
    }
}
