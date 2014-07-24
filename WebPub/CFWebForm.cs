using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF.Web;
using System.Web;
using COM.CF;
using System.IO;
using System.Runtime.InteropServices;

namespace CFTL
{
    public class CFWebForm
{
    // Fields
    private enPageType _curpagetype = enPageType.SelfPage;
    private CFPageControl _qqpagectrl;
    private readonly HttpContext Context;
    private readonly HttpRequest Request;
    private readonly HttpResponse Response;


    // Properties
    public enPageType CurPageType
    {
        get
        {
            return _curpagetype;
        }
    }

    private CFPageControl WebForm
    {
        get
        {
            if (_qqpagectrl == null)
            {
                _qqpagectrl = new CFPageControl(Context);
            }
            return _qqpagectrl;
        }
    }



    // Methods
    public CFWebForm(HttpContext context1)
    {
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
        string g = Request["autogo"];
        if ((g == null) || (g == ""))
        {
            if (url == null)
            {
                Response.Write("<center><input type=button value='返回' onclick='history.back()'></center>");
                return;
            }
        }
        else
        {
            url = g;
        }
        if (newidStr != null)
        {
            url = url.Replace("NEWID", newidStr);
        }
        int d = PubFunc.GetDefaultInt(Request["delay"], -1);
        if (d == -1)
        {
            if (delay == -1)
            {
                delay = 0xbb8;
            }
        }
        else
        {
            delay = d;
        }
        string reloadwin = Request["reloadwin"];
        if (reloadwin != null)
        {
            Response.Write("<script>if (" + reloadwin + ")" + reloadwin + ".location.reload()</script>");
        }
        switch (url.ToLower())
        {
            case "autoclose":
                Response.Write("<script>window.setTimeout('window.close()'," + Convert.ToString(delay) + ");</script>");
                Response.Write("<center><input type=button value='关闭' onclick='window.close()'></center>");
                Response.Write(Convert.ToString((int) (delay / 0x3e8)) + "秒后，自动关闭");
                return;

            case "refreshopener":
                Response.Write("<script>window.setTimeout('if (window.opener) window.opener.location.reload();window.close()'," + Convert.ToString(delay) + ");</script>");
                Response.Write("<center><input type=button value='关闭' onclick='if (window.opener) window.opener.location.reload();window.close()'></center>");
                Response.Write(Convert.ToString((int) (delay / 0x3e8)) + "秒后，自动关闭");
                return;

            default:
                if (((!url.ToLower().StartsWith("http://") && !url.ToLower().StartsWith("https://")) && !url.StartsWith("/")) && (Request.UrlReferrer != null))
                {
                    string d1 = Path.GetDirectoryName(Request.UrlReferrer.AbsolutePath);
                    if (d1 == null)
                    {
                        url = "/" + url;
                    }
                    else
                    {
                        url = d1.Replace('\\', '/') + "/" + url;
                    }
                }
                if (delay == 0)
                {
                    Response.Redirect(url, false);
                }
                else if (delay == 1)
                {
                    Response.ClearContent();
                    Response.Write("<script>location='" + url + "'</script>");
                    Response.Write("<center><a href=" + url + ">立刻继续</a></center>");
                }
                else
                {

                    Response.Write("<script>window.setTimeout(\"location='" + url + "'\"," + Convert.ToString(delay) + ");</script>"
                        +"<center>"
                        + Convert.ToString((int)(delay / 0x3e8)) + " 秒后，自动继续<br/>"
                        //+"<a href=" + url + ">立刻继续</a>"                        
                        +"<input type=button value='立即继续' onclick='location.href='" + url + "'>"
                        +"<input type=button value='返回' onclick='history.back()'></center>");
                }
                return;
        }
    }

    public void SetJSPage()
    {
        WebForm.SetJSPage();
    }

    public void SetXMLPage()
    {
        WebForm.SetXMLPage();
    }

    public void WriteErrorMsg(string msg)
    {
        WriteErrorPage("error", msg, "", "");
    }

    public void WriteErrorNoEnd(enErrType errType, string msg)
    {
        WriteErrorPage("error", msg, "", ",errcode:'" + errType.ToString() + "',gobacktishi:1");
    }

    public void WriteErrorPage(string errstr,  string othertishi="",  string otherHTML="", string otherCanShu="")
    {
        WebForm.WriteErrorPage(errstr, othertishi, otherHTML, otherCanShu);
    }

    public void WriteHead()
    {
        WriteHead("", "", "");
    }

    public void WriteHead(string title)
    {
        WriteHead(title, "", "");
    }

    public void WriteHead(string title, string bodyClass, string otherHead)
    {
        //Response.WriteFile(Context.Server.MapPath("/inc/head.txt"));
        Response.Write("<html><head><meta http-equiv='content-type' content='text/html; charset=utf-8 '>");
        Response.Write("<title>" + title + "</title>");
        Response.Write(otherHead);
        Response.Write("</head>");
        if (bodyClass != "")
        {
            Response.Write("<body class='" + bodyClass + "'>");
        }
        else
        {
            Response.Write("<body>");
        }
    }

    public void WriteLogin()
    {
        WriteErrorPage("notlogin", "", "", "");
    }

    public void WriteOK(string s)
    {
        Response.Write("<center><font color=blue size=+2>" + s + "</font></center>");
    }

    public void WritePageFoot()
    {
        //Response.WriteFile(Context.Server.MapPath("/inc/foot.txt"));
        Response.Write("</body></html>");
    }

    public void WriteTail()
    {
        //Response.WriteFile(Context.Server.MapPath("/inc/tail.txt"));
        Response.Write("</body></html>");
    }

    public void WriteXMLError(string errmsg)
    {
        WebForm.WirteXMLError(enXMLErrorCode.CFError, errmsg);
    }


}

 
 


 

 

}
