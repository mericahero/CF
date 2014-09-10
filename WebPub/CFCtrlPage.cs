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
                if (_login == null)
                {
                    _login = new LoginUsr(Context);
                }
                return _login;
            }
        }
        /// <summary>
        /// 用户登录控制，可以获得用户是否登录，且能强制用户登录
        /// </summary>
        protected override ILoginUsr UsrLogin
        {
            get
            {
                if (_login == null)
                {
                    _login = new LoginUsr(Context);
                }
                return _login;
            }
        }
        /// <summary>
        /// 页面控制
        /// </summary>
        private CFWebForm _webForm;
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
            enErrType errType = ee.ErrType;
            if (errType != enErrType.NotLogined)
            {
                WebForm.WriteErrorNoEnd(ee.ErrType, ee.Message);
            }
            else
            {
                WebForm.WriteLogin();
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
