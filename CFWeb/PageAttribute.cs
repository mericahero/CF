using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF.Web
{
    /// <summary>
    /// 功能：控制类中方法的自定义页面属性，指定该方法输出后所展示的页面
    /// 时间：2013-10-21
    /// 作者：meric
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PageAttribute : Attribute
    {
        #region 属性
        private bool m_mustLogin;
        /// <summary>
        /// 调用此方法是否需要登录的用户
        /// </summary>
        public bool MustLogin
        {
            get
            {
                return m_mustLogin;
            }
        }

        private enPageType m_pageType;
        /// <summary>
        /// 页面类型
        /// </summary>
        public enPageType PageType
        {
            get
            {
                return m_pageType;
            }
        }

        private string m_title;
        /// <summary>
        /// 页面标题 
        /// </summary>
        public string Title
        {
            get
            {
                return m_title;
            }
        }
        #endregion



        #region 构造方法
        /// <summary>
        /// 无参构造方法，设置页面默认为自定义页面
        /// </summary>
        public PageAttribute()
        {
            m_pageType = enPageType.SelfPage;
        }
        /// <summary>
        /// 指定方法的执行页面
        /// </summary>
        /// <param name="pageType"></param>
        public PageAttribute(enPageType pageType) 
            : this(pageType, false, "")
        {
        }
        /// <summary>
        /// 指定方法的执行页面，并且标识是否必须登录
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="mustLogin"></param>
        public PageAttribute(enPageType pageType, bool mustLogin) 
            : this(pageType, mustLogin, "")
        {
        }
        /// <summary>
        /// 指定方法的执行页面，并指定页面的标题
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="title"></param>
        public PageAttribute(enPageType pageType, string title) 
            : this(pageType, false, title)
        {
        }
        /// <summary>
        /// 指定方法的执行页面，并指定是否需要登录及页面标题 
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="mustLogin"></param>
        /// <param name="title"></param>
        public PageAttribute(enPageType pageType, bool mustLogin, string title)
        {
            m_mustLogin = mustLogin;
            m_title = title;
            m_pageType = pageType;
        }
        #endregion        


    }
}
