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
    public abstract class DefaultPage:Page
    {
        private NameValueCollection m_Form;

        private CFPageControl m_webForm;

        protected string BgColor
        {
            get
            {
                return"#F5FAF9";
            }
            set
            {
            }
        }


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

        [DebuggerNonUserCode]
        protected DefaultPage()
        {
            base.Load += Page_Load;
            base.Unload += Page_Unload;
        }

        protected abstract void EventMain();

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

        private void Page_Unload(object sender, EventArgs e)
        {
        }

        protected void WriteLogin()
        {
            Response.Write("<BR><center><font color=red size=+2>你还没有登录！</font></center><HR><font color=blue>提示</font>：<BR>你可以马上<a href=\"javascript:newin('/cgi-bin/sys/autolog/autolog.asp','_blank',300,300)\">去登录</a>登录成功后可以直接[<a href=javascript:document.location.reload()>Reload</a>]，重发该请求。<BR><BR>或者你现在返回，输入你的注册ID和口令。<BR><BR><BR>如果还没有注册，则请先<a target=_blank href=/cgi-bin/friends/reg0.htm>去注册</a>");
        }
    }
}
