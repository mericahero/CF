using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace COM.CF.Web
{
    public class CFPageControl
    {
        private enPageType _curpagetype;

        private readonly HttpContext Context;

        private readonly HttpRequest Request;

        private readonly HttpResponse Response;

        public enPageType CurPageType
        {
            get 
            {
                return _curpagetype;
            }
        }

        public CFPageControl(HttpContext context1)
        {
            _curpagetype = enPageType.SelfPage;
            Context = context1;
            Request = Context.Request;
            Response = Context.Response;
        }

        public void AutoGo()
        {
            AutoGo(null, -1);
        }

        public void AutoGo(string url)
        {
            AutoGo(url, -1);
        }

        public void AutoGo(string url, int delay)
        {
            AutoGo(url, delay, null);
        }

        public void AutoGo(string url, int delay, string newidStr)
        {
            string[] str;
            string item = Request["autogo"];
            if (item!= null  && item!="")
            {
                url = item;
            }
            else
            {
                if (url==null)
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
            if (defaultInt!=-1)
            {
                delay = defaultInt;
            }
            else
            {
                if ( delay == -1)
                {
                    delay = 0xbb8;
                }
            }
            string item1 = Request["reloadwin"];
            if (item1!=null)
            {
                Response.Write(string.Format("<script type=\"text/javascript\">if({0}){0}.location.reload();</script>",item1));
            }
            string lower = url.ToLower();
            if (lower != "autoclose")
            {
                if (lower != "refreshopener")
                {
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
                    if (delay!=0)
                    {
                        if (delay!=1)
                        {
                            Response.Write(string.Format("<script>window.setTimeout(\"location='{0}'\",{1});</script>",url,delay));
                            Response.Write(string.Concat("<center><a href=", url, ">立刻继续</a></center>"));
                            Response.Write(string.Concat(Convert.ToString(delay / 0x3e8), " 秒后，自动继续"));
                            Response.Write("<center><input type=button value='返回' onclick='history.back()'></center>");
                        }
                        else
                        {
                            Response.ClearContent();
                            Response.Write(string.Concat("<script>location='", url, "'</script>"));
                            Response.Write(string.Concat("<center><a href=", url, ">立刻继续</a></center>"));
                        }
                    }
                    else
                    {
                        Response.Redirect(url, false);
                    }
                }
                else
                {
                    Response.Write(string.Concat("<script src=http://js.5ilog.com/qq/js/pub.js></script><script>window.setTimeout('if (window.opener) window.opener.location.reload();window.close()',", Convert.ToString(delay), ");</script>"));
                    Response.Write("<center><input type=button value='关闭' onclick='if (window.opener) window.opener.location.reload();window.close()'></center>");
                    Response.Write(string.Concat(Convert.ToString(delay / 0x3e8), "秒后，自动关闭"));
                }
            }
            else
            {
                Response.Write(string.Concat("<script>window.setTimeout('window.close()',", Convert.ToString(delay), ");</script>"));
                Response.Write("<center><input type=button value='关闭' onclick='window.close()'></center>");
                Response.Write(string.Concat(Convert.ToString(delay / 0x3e8), "秒后，自动关闭"));
            }
        }


        //protected override void Dispose()
        //{
        //    Dispose();
        //}

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

        private void WirteJSError(string errstr, string othertishi = "", string otherHTML = "", string otherCanShu = "")
        {
            Response.Write(string.Concat("var errorObj={error:'", errstr, "',errtype:'sys'"));            
            if (othertishi!="")
            {
                Response.Write(string.Concat(",othertishi:'", othertishi, "'"));
            }
            if (otherHTML!="")
            {
                Response.Write(string.Concat(",otherHTML:", PubFunc.HTML2JSStr(otherHTML)));
            }
            if (otherCanShu!="")
            {
                Response.Write(otherCanShu);
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

        public void WriteErrorPage(string errstr, string othertishi = "", string otherHTML = "", string otherCanShu = "")
        {
            Response.ClearContent();
            int curPageType = CurPageType - enPageType.XMLPage;
            switch (curPageType)
            {
                case 0:
                    WirteXMLError(errstr);
                    break;
                case 1:
                    WirteJSError(errstr, othertishi, otherHTML, otherCanShu);
                    break;
                default:
                    Response.Write("<script>");
                    WirteJSError(errstr, othertishi, otherHTML, otherCanShu);
                    Response.Write("</script>");
                    Response.WriteFile(CFConfig.MapPath("/res/inc/errpage.html"));
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

        public void WriteHead(enPageType pageType, string title, string otherHead)
        {
            Response.Write("<html><head><meta http-equiv='content-type' content='text/html; charset=utf-8 '>");
            Response.Write(string.Concat("<title>", title, "</title>"));
            Response.Write(otherHead);
            Response.Write(string.Concat("</head><body ", BodyParams.AryBodyParams[(int)pageType], ">"));
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
            Response.Write("</body></HTML>");
        }


    }
}
