using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFTL;
using System.Web.Security;
using COM.CF.Web;
using COM.CF;

namespace CCBWeb.Main.Web
{
    class Usr :CFCtrlPage
    {
        [Page(enPageType.DefaultPage, false)]
        private void Login()
        {
            string userName = RequestForm["userName"];
            string userPwd = RequestForm["userPwd"];

            if (FormsAuthentication.Authenticate(userName, userPwd))
            {
                Response.Cookies["un"].Value = userName;
                WebForm.WriteOK("登录成功");
                WebForm.AutoGo();
            }
            else
            {
                WebForm.WriteErrorNoEnd(enErrType.NormalError,"登录失败");
                WebForm.AutoGo();
            }
        }
    }
}
