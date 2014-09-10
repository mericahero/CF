using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace COM.CF.Web
{
    /// <summary>
    /// 功能：页面的控制类，控制页面的跳转及错误输出等
    /// 时间：2013-10-22
    /// 作者：meric
    /// </summary>
    public class CFPageControl
    {
        #region 定义
        /// <summary>
        /// 当前的页面类型
        /// </summary>
        private enPageType _curpagetype;
        /// <summary>
        /// 当前执行上下文
        /// </summary>
        private readonly HttpContext Context;
        /// <summary>
        /// 当前请求Request
        /// </summary>
        private readonly HttpRequest Request;
        /// <summary>
        /// 当前上下文的返回Response
        /// </summary>
        private readonly HttpResponse Response;

        public enPageType CurPageType
        {
            get
            {
                return _curpagetype;
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context1">请求上下文</param>
        public CFPageControl(HttpContext context1)
        {
            _curpagetype = enPageType.DefaultPage;
            Context = context1;
            Request = Context.Request;
            Response = Context.Response;
        }

        #region 页面跳转
        /// <summary>
        /// 跳转页面
        /// </summary>
        public void AutoGo()
        {
            AutoGo(null, -1);
        }
        /// <summary>
        /// 跳转页面
        /// </summary>
        /// <param name="url">跳转URL</param>
        public void AutoGo(string url)
        {
            AutoGo(url, -1);
        }
        /// <summary>
        /// 延时后跳转到指定URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="delay"></param>
        public void AutoGo(string url, int delay)
        {
            AutoGo(url, delay, null);
        }
        /// <summary>
        /// 延迟后跳转到指定URL，URL可自定义
        /// </summary>
        /// <param name="url">指定跳转URL</param>
        /// <param name="delay">延时时间</param>
        /// <param name="newidStr">自定义NEWID</param>
        public void AutoGo(string url, int delay, string newidStr)
        {
            string temp = Request["autogo"];
            if (!string.IsNullOrWhiteSpace(temp))
            {
                url = temp;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    Response.Write("<center><input type=button value='返回' onclick='history.back()'></center>");
                    return;
                }
            }
            if (newidStr != null)
            {
                url = url.Replace("NEWID", newidStr);
            }
            int defaultInt = PubFunc.GetDefaultInt(Request["delay"], -1);
            //默认的跳转延迟时间
            delay = defaultInt <= -1 ? 3000 : defaultInt;

            temp = Request["reloadwin"];

            if (!string.IsNullOrWhiteSpace(temp))
            {
                Response.Write(string.Format("<script type=\"text/javascript\">if({0}){0}.location.reload();</script>", temp));
            }

            string lower = url.ToLower();
            //自动关闭
            if(lower=="autoclose")
            {
                Response.Write(string.Format("<script>window.setTimeout('window.close()',{0});</script>",delay));
                Response.Write("<center><input type=button value='关闭' onclick='window.close()'></center>");
                Response.Write(string.Concat(Convert.ToString(delay / 1000), "秒后，自动关闭"));
                return;
            }
            //刷新打开页面
            if(lower=="refreshopener")
            {
                Response.Write(string.Format("<script>window.setTimeout('if (window.opener) window.opener.location.reload();window.close()',{0});</script>",delay));
                Response.Write("<center><input type=button value='关闭' onclick='if (window.opener) window.opener.location.reload();window.close()'></center>");
                Response.Write(string.Concat(Convert.ToString(delay /1000), "秒后，自动关闭"));
            }

            if (!url.ToLower().StartsWith("http://") && !url.StartsWith("/"))
            {
                if (Request.UrlReferrer != null)
                {
                    string directoryName = Path.GetDirectoryName(Request.UrlReferrer.AbsolutePath);
                    if (directoryName != null)
                    {
                        url = string.Concat(directoryName.Replace("\\", "/"), "/", url);
                    }
                    else
                    {
                        url = string.Concat("/", url);
                    }
                }
            }
            if (delay == 0)
            {
                Response.Redirect(url, false);
                return;
            }

            if (delay == 1)
            {
                Response.ClearContent();
                Response.Write(string.Concat("<script>location='", url, "'</script>"));
                Response.Write(string.Concat("<center><a href=", url, ">立刻继续</a></center>"));
                return;
            }


            Response.Write(string.Format("<script>window.setTimeout(\"location='{0}'\",{1});</script>", url, delay));
            Response.Write(string.Concat("<center><a href=", url, ">立刻继续</a></center>"));
            Response.Write(string.Concat(Convert.ToString(delay / 1000), " 秒后，自动继续"));
            Response.Write("<center><input type=button value='返回' onclick='history.back()'></center>");

        }
        #endregion




        #region 页面控制
        
        #endregion
        public void Refresh(string win)
        {
            Response.Write(string.Concat("<script>", win, ".document.location.reload()</script>"));
        }

        public void SetJSPage()
        {
            HttpResponse response = Response;
            response.ContentType = "application/x-javascript";
            _curpagetype = enPageType.JSPage; 
        }

        public void SetXMLPage()
        {
            HttpResponse response = Response;
            bool isBig5 = CFConfig.IsBig5;
            if (!isBig5)
            {
                response.Write("<?xml version='1.0' encoding='UTF-8' ?>");
                response.ContentType = "text/xml";
                response.ContentEncoding = Encoding.UTF8;
            }
            else
            {
                response.ContentType = "text/xml;charset=big5";
                response.Write("<?xml version='1.0' encoding='big5' ?>");
            }
            _curpagetype = enPageType.XMLPage;
        }

        public void WirteJSError(enErrType errcode, string errstr)
        {
            Response.Clear();
            Response.Write(string.Concat("//error code=", Convert.ToString(errcode), " str=", errstr));
        }

        public void WriteJSONError(string error, string otip = "")
        {
            Response.Write(string.Concat("{r:'", error, "'"));
            if (otip != "")
            {
                Response.Write(string.Concat(",msg:'", otip, "'"));
            }
            Response.Write("}");
        }

        public void WriteJSONError(int errorcode, string otip = "")
        {
            if (errorcode > 0)
            {
                errorcode = -errorcode;
            }
            WriteJSONError(errorcode.ToString(), otip);
        }

        private void WirteJSError(string errstr, string otip = "", string ohtml = "", string oparam = "")
        {
            Response.Write(string.Concat("var errorObj={error:'", errstr, "',errtype:'sys'"));
            if (otip != "")
            {
                Response.Write(string.Concat(",otip:'", otip, "'"));
            }
            if (ohtml != "")
            {
                Response.Write(string.Concat(",ohtml:", PubFunc.HTML2JSStr(ohtml)));
            }
            if (oparam != "")
            {
                Response.Write(oparam);
            }
            Response.Write("};");
        }



        public void WirteXMLError(enXMLErrorCode errcode, string errstr)
        {
            Response.Clear();
            SetXMLPage();
            Response.Write(CFConfig.GetXMLError(errcode, errstr));
        }

        public void WirteXMLError(string errstr)
        {
            WirteXMLError(enXMLErrorCode.CFError, errstr);
        }

        public void WirteXMLError(enXMLErrorCode errcode, string errstr, enErrType qqerrtype)
        {
            Response.Clear();
            SetXMLPage();
            Response.Write(CFConfig.GetXMLError(errcode, errstr, qqerrtype));
        }

        public void WirteXMLOK(string s)
        {
            Response.Write(string.Concat("<OK>", s, "</OK>"));
        }

        public void WriteError(enErrType errType)
        {
            WriteError(errType, "");
        }

        public void WriteError(enErrType errType, string msg)
        {
            WriteErrorNoEnd(errType, msg);
            Response.End();
        }

        public void WriteErrorMsg(string msg)
        {
            WriteErrorPage("5ilogerror", msg, "", "");
        }

        public void WriteErrorNoEnd(enErrType errType, string msg)
        {
            WriteErrorPage("5ilogerror", msg, "", string.Concat(",errcode:'", errType.ToString(), "',gobacktishi:1"));
        }

        public void WriteErrorPage(string errstr, string otip = "", string ohtml = "", string oparam = "")
        {
            Response.ClearContent();
            switch (CurPageType)
            {
                case enPageType.SelfPage:
                    WriteJSONError(errstr, otip);
                    break;
                case enPageType.XMLPage:
                    WirteXMLError(errstr);
                    break;
                case enPageType.JSPage:
                    WirteJSError(errstr, otip, ohtml, oparam);
                    break;
                default:
                    Response.Write("<script>");
                    WirteJSError(errstr, otip, ohtml, oparam);
                    Response.Write("</script>");
                    Context.Server.Execute("~/res/inc/errpage.aspx");
                    break;
            }
        }

        public void WriteHead()
        {
            WriteHead(enPageType.DefaultPage, "");
        }

        public void WriteHead(enPageType pageType, string title)
        {
            WriteHead(pageType, title, "");
        }
        /// <summary>
        /// 输出页面头部
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="title"></param>
        /// <param name="otherHead"></param>
        public void WriteHead(enPageType pageType, string title, string otherHead)
        {
            Response.Write("<html><head><meta http-equiv='content-type' content='text/html; charset=utf-8 '>");
            Response.Write(string.Concat("<title>", title, "</title>"));
            Response.Write(otherHead);
            Response.Write("</head><body>");
        }
 


        public void WriteOK(string s)
        {
            Response.Write(string.Concat("<center><font color=blue size=+2>", s, "</font></center>"));
        }

        public void WritePageFoot()
        {
            Response.Write("</body></html>");
        }

        public void WriteTail()
        {
            Response.Write("</body></html>");
        }


    }
}
