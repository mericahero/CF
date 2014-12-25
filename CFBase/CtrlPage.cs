using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeniusTek;
using System.Collections.Specialized;
using COM.CF.Web;
using System.Web;
using System.Diagnostics;
using System.Reflection;

namespace COM.CF
{
    /// <summary>
    /// 功能：后台控制页面，该页面会处理前台的HTTP请求，该类为一个抽象类，实现部分方法，另一部分方法到实例类里去实现 
    /// 时间：2013-10-25
    /// 作者：meric
    /// </summary>
    public abstract class CtrlPage : PageBase
    {


        #region 控制类页面的属性
        private NameValueCollection m_Form;
        /// <summary>
        /// 请求参数，包括POST和GET，但不能混合 
        /// </summary>
        protected NameValueCollection RequestForm
        {
            get
            {
                if (m_Form == null)
                {
                    if (Request.HttpMethod != "POST")
                    {
                        m_Form = Request.QueryString;
                    }
                    else
                    {
                        try
                        {
                            m_Form = Request.Form;
                        }
                        catch
                        {
                            WriteErrorNoEnd(enErrType.NormalError, "上传文件过大！");
                        }
                    }
                }
                return m_Form;
            }
        }

        private CFPageControl m_webForm;
        /// <summary>
        /// 页面控制类
        /// </summary>
        private CFPageControl WebForm
        {
            get
            {
                if (m_webForm == null)
                {
                    m_webForm = new CFPageControl(Context);
                }
                return m_webForm;
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        protected CtrlPage()
        {
        }
        /// <summary>
        /// 处理方法
        /// </summary>
        protected override void EventMain()
        {
            string name = RequestForm["act"];
            //不传act参数，则执行控制类里的EnterIn方法
            if (string.IsNullOrWhiteSpace(name)) name = "EnterIn";
            MethodInfo method = GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (method == null)
            {
                WriteHead();
                
                WriteErrorNoEnd(enErrType.NormalError, "方法 " + name + " 没有找到！");
                return;
            }

            PageAttribute customAttribute = (PageAttribute) Attribute.GetCustomAttribute(method, typeof(PageAttribute));
            if (customAttribute == null)
            {
                WriteHead();
                WriteErrorNoEnd(enErrType.NormalError, "方法 " + name + " 没有找到！");
                return;
            }

            try
            {
                if (customAttribute.MustLogin)
                {
                    UsrLogin.MustLogin();
                }
                if(customAttribute.PageType!=enPageType.SelfPage)
                {
                    WebForm.CurPageType=customAttribute.PageType;
                }
                switch (customAttribute.PageType)
                {
                    case enPageType.SelfPage:
                        break;
                    case enPageType.DefaultPage:
                        WriteHead(customAttribute.PageType, customAttribute.Title);
                        break;
                    case enPageType.XMLPage:
                        WebForm.SetXMLPage();
                        break;
                    case enPageType.JSPage:
                        WebForm.SetJSPage();
                        break;
                    default:
                        WriteHead(customAttribute.PageType, customAttribute.Title);
                        break;
                }
                method.Invoke(this, null);
                switch (customAttribute.PageType)
                {
                    case enPageType.SelfPage:
                    case enPageType.XMLPage:
                    case enPageType.JSPage:
                        break;
                    case enPageType.DefaultPage:
                        WriteTail();
                        break;
                    default:
                        WriteTail();
                        break;
                }
            }
            catch (CFException exception1)
            {
                switch (customAttribute.PageType)
                {
                    case enPageType.SelfPage:
                        WebForm.WriteJSONError(exception1.ErrType, exception1.Message);
                        break;
                    case enPageType.XMLPage:
                        WebForm.WirteXMLError(exception1.ErrType, exception1.Message);
                        break;
                    default:
                        WriteErrorNoEnd(exception1.ErrType, exception1.Message);
                        break;
                }
            }
            catch(Exception exception2)
            {
                if (exception2.InnerException == null)
                {
                    COM.CF.Web.ErrorLog.UnControlException(WebForm, exception2, customAttribute.PageType == enPageType.XMLPage);
                }
                else
                {
                    CFException innerException = (exception2.InnerException) as CFException;
                    if (innerException != null)
                    {
                        switch (customAttribute.PageType)
                        {
                            case enPageType.SelfPage:
                                WebForm.WriteJSONError(innerException.Message);
                                break;
                            case enPageType.XMLPage:
                                WebForm.WirteXMLError(innerException.Message);
                                break;
                            case enPageType.JSPage:
                                WebForm.WirteJSError(innerException.ErrType, innerException.Message);
                                break;
                            default:
                                HandleException(innerException);
                                break;
                        }
                    }
                    else
                    {
                        COM.CF.Web.ErrorLog.UnControlException(WebForm, exception2.InnerException, customAttribute.PageType == enPageType.XMLPage);
                    }
                }
            }
        }
        /// <summary>
        /// 输出页面的头部
        /// </summary>
        protected virtual void WriteHead()
        {
            WriteHead(WebForm.CurPageType, "");
        }
        /// <summary>
        /// 抽象方法，接管异常处理
        /// </summary>
        /// <param name="e">异常信息</param>
        protected abstract void HandleException(CFException e);
        /// <summary>
        /// 抽象方法，输出错误信息，但不输出页面尾部
        /// </summary>
        /// <param name="errType">错误类型</param>
        /// <param name="msg">错误信息</param>
        protected abstract void WriteErrorNoEnd(enErrType errType, string msg);
        /// <summary>
        /// 抽象方法，输出页面头部
        /// </summary>
        /// <param name="pageType">页面类型</param>
        /// <param name="title">页面标题</param>
        protected abstract void WriteHead(enPageType pageType, string title);
        /// <summary>
        /// 抽象方法，输出页面尾部
        /// </summary>
        protected abstract void WriteTail();        
        /// <summary>
        /// 当前页面的登录控制，继承类里必须实现
        /// </summary>
        protected abstract ILoginUsr UsrLogin { get; }



    }

}
