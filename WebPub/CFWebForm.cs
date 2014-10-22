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
    /// <summary>
    /// 功能：CFPage的页面控制类，包括页面类型，请求与返回，跳转
    /// 时间：2013-10-5
    /// 作者：Meric
    /// </summary>
    public class CFWebForm : CFPageControl
    {
        #region 构造函数
        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="context1"></param>
        public CFWebForm(HttpContext con)
            : base(con){}

        public CFWebForm(HttpContext cont, enPageType pt)
            : base(cont,pt){}
        #endregion

        

        public void WriteXMLError(string errmsg)
        {
            base.WirteXMLError(errmsg);
        }



        #region 异常输出
        public void WriteLogin()
        {
            WriteErrorPage(enErrType.NotLogined, "没有登录");
        }
        #endregion

        

    }

}
