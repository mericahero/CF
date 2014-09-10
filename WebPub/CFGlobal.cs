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
    /// <summary>
    /// 功能：CF框架的站点配置，包括系统的运行及错误的处理
    /// 时间：2013-10-22
    /// 作者：陈辰
    /// </summary>
    public class CFGlobal : HttpApplication
    {       

        //构造函数 
        public CFGlobal()
        {
            base.Error += new EventHandler(Global_Error);
        }

        /// <summary>
        /// 程序初始化 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Application_Start(object sender, EventArgs e)
        {
            CFConfig.InitConfig();
            CWConfig.SetConfig(WebConfigurationManager.AppSettings);
        }
        /// <summary>
        /// 接管错误处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
