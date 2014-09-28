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
        /// <summary>
        /// usrlogin，实现了ILoginUsr接口，实现了功能：判断用户是否登录，强制用户在该页面上必须登录
        /// </summary>
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


        private Int32 _curuid;
        protected Int32 CurUID
        {
            get
            {
                return _curuid=_curuid!=0? _curuid:(UsrLogin.Logined ? UsrInfo.UID : 0);                
            }
        }

        private Boolean _isdealer;
        protected Boolean ISDealer
        {
            get
            {
                return _isdealer = CheckIDType(8);
            }
        }

        private Boolean _isAdmin;
        protected Boolean ISAdmin
        {
            get
            {
                return _isAdmin = CheckIDType(2);
            }
        }

        private Boolean _isMember;
        protected Boolean ISMember
        {
            get
            {
                return _isMember = CheckIDType(1);
            }
        }

        private Boolean _isVendor;
        protected Boolean ISVendor
        {
            get
            {
                return _isVendor = CheckIDType(4);
            }
        }


        protected Boolean CheckIDType(int type)
        {
            return _isAdmin = UsrLogin.Logined && (UsrInfo.IDType & type) != 0;
        }

        /// <summary>
        /// 接管页面抛出的异常
        /// </summary>
        /// <param name="ee"></param>
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
                if (FWFunc.GetIisVersion() < 7)
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
