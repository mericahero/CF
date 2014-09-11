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

        private double GetIisVersion()
        {
            double r = -1;

            Version ver = System.Environment.OSVersion.Version;

            if (ver.Major == 4 && ver.Minor == 0)
            {
                r = 4.0;
            }
            else if (ver.Major == 5)
            {
                if (ver.Minor == 0)
                {
                    r = 5.0;
                }
                else if (ver.Minor == 1)
                {
                    r=5.1;
                }
                else if (ver.Minor == 2)
                {
                    r=6.0;
                }
            }
            else if (ver.Major == 6)
            {
                if (ver.Minor == 0)
                {
                    r=7.0;
                }
                else if (ver.Minor == 1)
                {
                    r=7.5;
                }
            }

            return r;
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
                if (GetIisVersion() < 7)
                {
                    Response.ClearHeaders();
                    Response.AddHeader("Cache-Control", "private");
                }
                else
                {
                    if (Response.Headers["Cache-Control"] == "public")
                    {
                        Response.Headers["Cache-Control"] = "private";
                    }
                    Response.Headers.Remove("Expires");
                }
                WebForm.WriteErrorNoEnd(ee.ErrType, ee.Message);
                HttpContext.Current.ClearError();
            }

        }
    }

}
