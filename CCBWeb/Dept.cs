using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFTL;
using COM.CF.Web;

namespace CCBWeb.Main.Web
{
    public class Dept : CFCtrlPage
    {
        [Page(enPageType.DefaultPage, false)]
        private void Add()
        {
            CC.BL.Dept.Add(RequestForm);
            WebForm.WriteOK("添加成功！");
            WebForm.AutoGo();
        }
    }
}
