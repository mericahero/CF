using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using COM.CF.Web;
using System.Web;

namespace CFTL
{
    public class CFCtrlPage : CtrlPage
    {
        // Fields
        private LoginUsr _login;
        private CFWebForm _webForm;

        // Methods
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

        protected override void WriteErrorNoEnd(enErrType errType, string msg)
        {
            WebForm.WriteErrorNoEnd(errType, msg);
        }

        protected override void WriteHead(enPageType pageType, string title)
        {
            WebForm.WriteHead(title);
        }

        protected override void WriteTail()
        {
            WebForm.WriteTail();
        }

        // Properties
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


        protected void WriteErrorMessage(IDictionary<int, string> dic,int r)
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


    }
}
