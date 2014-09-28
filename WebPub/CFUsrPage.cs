using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFTL
{
    /// <summary>
    /// 功能：CF框架中需要登录的页面
    /// 时间：2013-10-22
    /// 作者：meric
    /// </summary>
    public class CFUsrPage : CFPage
    {
        public CFUsrPage()
        {
            base.Load += Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!UsrLogin.Logined)
            {
                var autogo = Request.Url.ToString();
                var filePath = Request.FilePath;
                var type = 1;
                if(filePath.StartsWith("/admin/"))
                {
                    type=2;
                }
                Response.Redirect(string.Format( "{0}/login.aspx?typeid={1}&autogo={2}" ,CWS.CWConfig.LoginHost,type,System.Web.HttpUtility.UrlEncode(autogo)),false);
            }
        }
    }

}
