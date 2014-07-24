using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using System.Web;

namespace CFTL
{
    public class CFPage : UIPage
    {
        private LoginUsr _usrInfo;

        private CFWebForm _webForm;

        protected IUsr UsrInfo
        {
            get
            {
                if (_usrInfo == null)
                {
                    _usrInfo = new LoginUsr(Context);
                }
                return _usrInfo;
            }
        }

        protected new ILoginUsr UsrLogin
        {
            get
            {
                if (_usrInfo == null)
                {
                    _usrInfo = new LoginUsr(Context);
                }
                return _usrInfo;
            }
        }

        protected CFWebForm WebForm
        {
            get
            {
                if (_webForm == null)
                {
                    _webForm = new CFWebForm(Context);
                }
                return _webForm;
            }
        }


        protected override void HandleException(CFException ee)
        {
            enErrType errType = ee.ErrType;
            bool flag = errType == enErrType.NotLogined;
            if (!flag)
            {
                Response.Clear();
                if (Response.Headers["Cache-Control"]=="public")
                {
                    Response.Headers["Cache-Control"] = "private";
                }
                Response.Headers.Remove("Expires");
                WebForm.WriteErrorNoEnd(ee.ErrType, ee.Message);
                HttpContext.Current.ClearError();
            }
            else
            {
                Response.Clear();
                HttpContext.Current.ClearError();
                WebForm.WriteLogin();
            }
        }
    }

}
