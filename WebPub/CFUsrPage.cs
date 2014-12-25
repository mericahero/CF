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
                var adminroot = CWS.CWConfig.AdminRoot;
                var iVendorRoot = autogo.Contains("/" + CWS.CWConfig.Appset["VendorRoot"]);
                if (filePath.StartsWith(string.Format("/{0}/",adminroot)))
                {
                    Response.Redirect(string.Format("{0}/{1}/login.aspx?autogo={2}",CWS.CWConfig.AdminHost,adminroot,Server.UrlEncode(autogo)),false);
                }
                else
                {

                    Response.Redirect(string.Format("{0}/login.aspx?typeid={1}&autogo={2}", CWS.CWConfig.LoginHost,iVendorRoot?4:1, Server.UrlEncode(autogo)), false);
                }
            }
        }
    }

}
