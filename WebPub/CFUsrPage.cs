using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFTL
{
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
                Response.Redirect(string.Format( "{0}/login.aspx?autogo={1}" ,CWS.CWConfig.LoginHost,System.Web.HttpUtility.UrlEncode(autogo)));
            }
            //UsrLogin.MustLogin();
        }
    }

}
