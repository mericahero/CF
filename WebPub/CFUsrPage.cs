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
            UsrLogin.MustLogin();
        }
    }

}
