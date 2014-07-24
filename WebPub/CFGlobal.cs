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
        // Fields
        private static List<WeakReference> __ENCList = new List<WeakReference>();

        // Methods
        public CFGlobal()
        {
            base.Error += new EventHandler(this.Global_Error);
            __ENCAddToList(this);
        }


        private static void __ENCAddToList(object value)
        {
            List<WeakReference> list = __ENCList;
            lock (list)
            {
                if (__ENCList.Count == __ENCList.Capacity)
                {
                    int index = 0;
                    int num3 = __ENCList.Count - 1;
                    for (int i = 0; i <= num3; i++)
                    {
                        WeakReference reference = __ENCList[i];
                        if (reference.IsAlive)
                        {
                            if (i != index)
                            {
                                __ENCList[index] = __ENCList[i];
                            }
                            index++;
                        }
                    }
                    __ENCList.RemoveRange(index, __ENCList.Count - index);
                    __ENCList.Capacity = __ENCList.Count;
                }
                __ENCList.Add(new WeakReference(RuntimeHelpers.GetObjectValue(value)));
            }
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
                if ((LikeOperator.LikeString(ee.Message, "文件*不存在。", CompareMethod.Binary) || LikeOperator.LikeString(ee.Message, "The file*does not exist.", CompareMethod.Binary) ? true : false))
                {
                    this.Server.ClearError();
                    //this.Response.WriteFile(CFConfig.MapPath("/inc/404.txt"));
                    Response.Write("文件不存在或已被删除");
                    return;
                }
            }
        }
    }

 









}
