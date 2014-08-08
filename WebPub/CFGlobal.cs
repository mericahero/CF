using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.CompilerServices;
using COM.CF;
using System.Web.Configuration;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic;
using CWS;


namespace CFTL
{
    public class CFGlobal : HttpApplication
    {       

        //构造函数 
        public CFGlobal()
        {
            base.Error += new EventHandler(this.Global_Error);
        }


        public void Application_Start(object sender, EventArgs e)
        {
            CFConfig.initconfig();
            CWConfig.SetConfig(WebConfigurationManager.AppSettings);
        }

        private void Global_Error(object sender, EventArgs e)
        {
            Exception ee = HttpContext.Current.Error;
            if (ee is HttpException)
            {
                if (ee.InnerException is HttpParseException)
                {
                    ee = ee.InnerException;
                }
                else if (ee.InnerException is HttpException)
                {
                    if (ee.InnerException.InnerException is HttpParseException)
                    {
                        ee = ee.InnerException.InnerException;
                    }
                }
            }
            if (ee is HttpParseException)
            {
                ErrorLog.WriteLog(ee);
            }
            if (ee is HttpException)
            {
                if (LikeOperator.LikeString(ee.Message, "文件*不存在。", CompareMethod.Binary) || LikeOperator.LikeString(ee.Message, "The file*does not exist.", CompareMethod.Binary))
                {
                    this.Server.ClearError();                    
                    Response.Redirect("/res/inc/404.aspx",false);
                    return;
                }
            }
        }
    }

 









}
