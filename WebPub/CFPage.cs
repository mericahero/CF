using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using System.Web;
using COM.CF.Web;

namespace CFTL
{
    /// <summary>
    /// 功能：CF框架内页面的基类
    /// 时间：2013-10-22
    /// 作者：meric
    /// </summary>
    public class CFPage : UIPage
    {
        
        private LoginUsr _usrInfo;
        /// <summary>
        /// _usrInfo 同时实现了IUsr以及ILoginUsr接口
        /// </summary>
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
        /// <summary>
        /// 页面控制类
        /// </summary>
        protected CFWebForm WebForm
        {
            get
            {
                return _webForm=_webForm??new CFWebForm(Context,enPageType.DefaultPage);
            }
        }


        private Int32 _curuid;
        /// <summary>
        /// 当前用户UID，未登录为0
        /// </summary>
        protected Int32 CurUID
        {
            get
            {
                return _curuid=_curuid!=0? _curuid:(UsrLogin.Logined ? UsrInfo.UID : 0);                
            }
        }

        private Boolean _isdealer;
        /// <summary>
        /// 是否是采购商——XH365定制
        /// </summary>
        public Boolean ISDealer
        {
            get
            {
                return _isdealer = CheckIDType(8);
            }
        }

        private Boolean _isAdmin;
        /// <summary>
        /// 是否是管理员——XH365定制
        /// </summary>
        public Boolean ISAdmin
        {
            get
            {
                return _isAdmin = CheckIDType(2);
            }
        }

        private Boolean _isMember;
        /// <summary>
        /// 是否是注册用户——XH365定制
        /// </summary>
        public Boolean ISMember
        {
            get
            {
                return _isMember = CheckIDType(1);
            }
        }

        private Boolean _isVendor;
        /// <summary>
        /// 是否是供应商——XH365定制
        /// </summary>
        public Boolean ISVendor
        {
            get
            {
                return _isVendor = CheckIDType(4);
            }
        }

        /// <summary>
        /// 根据传入诉身份类型判断当前用户是符合身份
        /// </summary>
        /// <param name="type">身份类型代码</param>
        /// <returns>返回是否符合身份</returns>
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
