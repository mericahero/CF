using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.IO;

namespace COM.CF.Web
{
    public class ErrorLog
    {
        [DebuggerNonUserCode]
        public ErrorLog()
        {
        }

        public static void UnControlException(CFPageControl webForm, Exception e)
        {
            ErrorLog.UnControlException(webForm, e, false);
        }

        public static void UnControlException(CFPageControl webForm, Exception e, bool isXMLPage)
        {
            if (e == null)
                return;
            
            if (e is SqlException)
            {
                SqlException sqlException = (SqlException)e;
                if (sqlException.Number == -2)
                {
                    if (CFConfig.ErrorLogJiBie == enErrorLogJiBie.Normal)
                    {
                        ErrorLog.WrtieBusy(sqlException);
                    }
                    if (!isXMLPage)
                    {
                        webForm.WriteErrorPage(enErrType.DevelopError, "开发者错误，请联系开发人员");
                    }
                    else
                    {
                        webForm.WirteXMLError( "系统繁忙，请稍后再试。谢谢合作");
                    }
                    return;
                }
            }
            if (CFConfig.ErrorLogJiBie != enErrorLogJiBie.NoLog)
            {
                ErrorLog.WriteLog(e);
            }
            if (CFConfig.ErrorLogJiBie == enErrorLogJiBie.Direct2Usr)
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Write(string.Concat("<pre>", e.ToString()));
                if (e.InnerException != null)
                {
                    response.Write("<HR>");
                    response.Write(e.InnerException.ToString());
                }
                response.Write("</pre>");
                response.End();
                response = null;
            }
            if (!isXMLPage)
            {
                webForm.WriteErrorPage(enErrType.DevelopError, "开发者错误，请联系开发人员");
            }
            else
            {
                webForm.WirteXMLError(enErrType.SystemBusy, "系统错误！我们会尽快解决该问题的");
            }
        }

        public static void WriteLog(Exception e)
        {
            if (e.GetType().ToString()!= "System.Web.HttpRequestValidationException")
            {
                HttpRequest request = HttpContext.Current.Request;
                string[] str = new string[] { CFConfig.logFileDir, CFConfig.HomeID.ToString(), "/dotNetErr", DateTime.Now.ToString("yyyy-MM-dd"), ".log" };
                StreamWriter streamWriter = new StreamWriter(string.Concat(str), true, Encoding.UTF8);
                try
                {
                    streamWriter.WriteLine("——————————————————————————E");
                    streamWriter.WriteLine(DateTime.Now.ToString(CFConfig.RQFormatStr));
                    streamWriter.WriteLine(string.Concat("URL=", request.RawUrl));
                    streamWriter.WriteLine();
                    if (e != null)
                    {
                        streamWriter.WriteLine(e.ToString());
                    }
                    if(request.RequestType=="POST")
                    {
                        if (request.ContentLength <= 1000)
                        {
                            string str1 = request.Form.ToString();
                            streamWriter.WriteLine(string.Concat("POST:", str1.Substring(0, Math.Min(100, str1.Length))));
                        }
                        else
                        {
                            streamWriter.WriteLine(string.Concat("POST-length:", request.ContentLength.ToString()));
                        }
                    }
                    streamWriter.WriteLine(string.Concat("IP=", FWFunc.GetIP()));
                    streamWriter.Write(request.ServerVariables["ALL_RAW"]);
                }
                finally
                {
                    streamWriter.Close();
                }
            }
        }

        public static void WrtieBusy(Exception e)
        {
            HttpRequest request = HttpContext.Current.Request;
            string[] str = new string[] { CFConfig.logFileDir, CFConfig.HomeID.ToString(), "/busy", DateTime.Now.ToString("yyyy-MM-dd"), ".log"};
            StreamWriter streamWriter = new StreamWriter(string.Concat(str), true, Encoding.UTF8);
            try
            {
                streamWriter.Write(DateTime.Now.ToString(CFConfig.RQFormatStr));
                streamWriter.WriteLine(string.Concat(" = ", request.RawUrl, " ", e.Message));
            }
            finally
            {
                streamWriter.Close();
            }
        }
    }
}
