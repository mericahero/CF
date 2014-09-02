using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using System.Web;

namespace CFTL
{
    /// <summary>
    /// 功能：CF框架内页面的基类
    /// 时间：2013-10-22
    /// 作者：meric
    /// </summary>
    public class CFPage : UIPage
    {
        /// <summary>
        /// _usrInfo 同时实现了IUsr以及ILoginUsr接口
        /// </summary>
        private LoginUsr _usrInfo;
        protected IUsr UsrInfo
        {
            get
            {
                return _usrInfo = _usrInfo ?? new LoginUsr(Context);
            }
        }
        
        protected new ILoginUsr UsrLogin
        {
            get
            {
                return _usrInfo = _usrInfo ?? new LoginUsr(Context);
            }
        }

        private CFWebForm _webForm;
        protected CFWebForm WebForm
        {
            get
            {
                return _webForm=_webForm??new CFWebForm(Context);
            }
        }




        protected override void HandleException(CFException ee)
        {
            enErrType errType = ee.ErrType;
            if (ee.ErrType == enErrType.NotLogined)
            {
                Response.Clear();
                HttpContext.Current.ClearError();
                WebForm.WriteLogin();
            }
            else
            {
                Response.Clear();
                if (Response.Headers["Cache-Control"] == "public")
                {
                    Response.Headers["Cache-Control"] = "private";
                }
                Response.Headers.Remove("Expires");
                WebForm.WriteErrorNoEnd(ee.ErrType, ee.Message);
                HttpContext.Current.ClearError();
            }

        }
    }

}
