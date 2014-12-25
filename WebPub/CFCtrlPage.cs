using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using COM.CF.Web;
using System.Web;

namespace CFTL
{
    /// <summary>
    /// 功能：CF控制页面，用于前后台交互，前台请求将通过global里的httphandler转接到控制页面里
    /// 时间：2013-10-26
    /// 作者：meric
    /// </summary>
    public class CFCtrlPage : CtrlPage
    {
        #region 页面属性
        /// <summary>
        /// 登录的用户
        /// </summary>
        private LoginUsr _login;
        /// <summary>
        /// 用户信息，当且仅当用户登录成功时能调用，否则抛出未登录异常
        /// </summary>
        public IUsr UsrInfo
        {
            get
            {
                return _login =_login ?? new LoginUsr(Context);
            }
        }
        /// <summary>
        /// 用户登录控制，可以获得用户是否登录，且能强制用户登录
        /// </summary>
        protected override ILoginUsr UsrLogin
        {
            get
            {
                return _login =_login?? new LoginUsr(Context);
            }
        }
        
        private CFWebForm _webForm;
        /// <summary>
        /// 页面控制
        /// </summary>
        protected CFWebForm WebForm
        {
            get
            {
                return _webForm=_webForm?? new CFWebForm(Context,enPageType.DefaultPage);
            }
        }

        private Int32 _curuid;
        /// <summary>
        /// 当前登录用户的UID，未登录为0
        /// </summary>
        protected Int32 CurUID
        {
            get
            {
                return _curuid = _curuid != 0 ? _curuid : (UsrLogin.Logined ? UsrInfo.UID : 0);
            }
        }

        private Boolean _isdealer;
        /// <summary>
        /// 是否是采购商用户——XH365定制
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
        /// 是否是管理员用户——XH365定制
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
        /// 是否是供应商用户——XH365定制
        /// </summary>
        public Boolean ISVendor
        {
            get
            {
                return _isVendor = CheckIDType(4);
            }
        }

        /// <summary>
        /// 根据传入的用户身份验证用户是否符合身份
        /// </summary>
        /// <param name="type">身份代码</param>
        /// <returns>返回是否符合</returns>
        protected Boolean CheckIDType(int type)
        {
            return _isAdmin = UsrLogin.Logined && (UsrInfo.IDType & type) != 0;
        }

        #endregion

        #region 错误处理
        /// <summary>
        /// 处理异常信息
        /// </summary>
        /// <param name="ee"></param>
        protected override void HandleException(CFException ee)
        {
            if (ee == null)
            {
                HttpContext.Current.Response.Write("系统错误");
                return;
            }
            HttpContext current = HttpContext.Current;
            
            if (ee.ErrType == enErrType.NotLogined)
            {
                WebForm.WriteLogin();
            }
            else
            {
                WebForm.WriteErrorNoEnd(ee.ErrType, ee.Message);
            }
        }
        #endregion

        #region 输出信息
        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="errType"></param>
        /// <param name="msg"></param>
        protected override void WriteErrorNoEnd(enErrType errType, string msg)
        {
            WebForm.WriteErrorNoEnd(errType, msg);
        }

        /// <summary>
        /// 根据不同的页面类型输出不同的头部信息
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="title"></param>
        protected override void WriteHead(enPageType pageType, string title)
        {
            if (pageType == enPageType.SelfPage) return;
            WebForm.WriteHead(title);
        }
        /// <summary>
        /// 输出页面尾部信息
        /// </summary>
        protected override void WriteTail()
        {
            WebForm.WriteTail();
        }



        /// <summary>
        /// 输出页面的错误信息，传递一个dictionary以及错误码
        /// </summary>
        /// <param name="dic">一系列错误代码及错误说明的键值对</param>
        /// <param name="r">程序输出的错误码</param>
        protected void WriteErrorMessage(IDictionary<int, string> dic, int r)
        {
            if (dic.ContainsKey(r))
            {
                WebForm.WriteErrorNoEnd(enErrType.NormalError, dic[r]);
            }
            else
            {
                WebForm.WriteErrorNoEnd(enErrType.NormalError, SysErrMsg.GetErrMsg(r));
            }
        }
        /// <summary>
        /// 输出页面的错误信息，传递一个dictionary以及错误码，以XML形式展现
        /// </summary>
        /// <param name="dic">一系列错误代码及错误说明的键值对</param>
        /// <param name="r">程序输出的错误码</param>
        protected void WriteErrorMessageXML(IDictionary<int, string> dic, int r)
        {
            if (dic.ContainsKey(r))
            {
                WebForm.WriteXMLError(dic[r]);
            }
            else
            {
                WebForm.WriteXMLError(SysErrMsg.GetErrMsgXML(r));
            }
        }

        #endregion       

    }
}
