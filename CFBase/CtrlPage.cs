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
    /// 功能：后台控制页面，该页面会处理前台的HTTP请求
    /// 时间：2013-10-25
    /// 作者：meric
    /// </summary>
    public abstract class CtrlPage : PageBase
    {
        
        private NameValueCollection m_Form;
        private CFPageControl m_webForm;

        // Methods
        protected CtrlPage()
        {
        }

        protected override void EventMain()
        {
            string name = RequestForm["act"];
            if (string.IsNullOrWhiteSpace(name)) name = "EnterIn";
            MethodInfo method = GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (method == null)
            {
                WriteHead();
                WriteErrorNoEnd(enErrType.DevelopError, "方法 " + name + " 没有找到！");
            }
            else
            {
                PageAttribute customAttribute = (PageAttribute) Attribute.GetCustomAttribute(method, typeof(PageAttribute));
                if (customAttribute == null)
                {
                    WriteHead();
                    WriteErrorNoEnd(enErrType.DevelopError, "方法 " + name + " 没有找到！！");
                }
                else
                {
                    try
                    {
                        if (customAttribute.MustLogin)
                        {
                            UsrLogin.MustLogin();
                        }

                        switch (customAttribute.PageType)
                        {
                            case enPageType.SelfPage:
                                break;
                            case enPageType.DefaultPage:
                            case enPageType.DarkPage:
                            case enPageType.FriendSetting:
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
                            case enPageType.DarkPage:
                            case enPageType.FriendSetting:
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
                                WebForm.WriteJSONError((int)enXMLErrorCode.CFError, exception1.Message);
                                break;
                            case enPageType.XMLPage:
                                WebForm.WirteXMLError(enXMLErrorCode.CFError, exception1.Message, exception1.ErrType);
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
                                    case enPageType.XMLPage:
                                        WebForm.WirteXMLError(enXMLErrorCode.CFError, innerException.Message, innerException.ErrType);
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
            }
        }

        protected abstract void HandleException(CFException e);
        protected abstract void WriteErrorNoEnd(enErrType errType, string msg);

        protected virtual void WriteHead()
        {
            WriteHead(enPageType.DefaultPage, "");
        }

        protected abstract void WriteHead(enPageType pageType, string title);
        protected abstract void WriteTail();

        // Properties
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

        protected abstract ILoginUsr UsrLogin { get; }


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
    }

}
