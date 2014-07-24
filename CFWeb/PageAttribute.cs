using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF.Web
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PageAttribute : Attribute
    {
        // Fields
        private bool m_mustLogin;
        private enPageType m_pageType;
        private string m_title;

        // Properties
        public bool MustLogin
        {
            get
            {
                return m_mustLogin;
            }
        }

        public enPageType PageType
        {
            get
            {
                return m_pageType;
            }
        }

        public string Title
        {
            get
            {
                return m_title;
            }
        }


        // Methods
        public PageAttribute()
        {
            m_pageType = enPageType.SelfPage;
        }

        public PageAttribute(enPageType pageType)
            : this(pageType, false, "")
        {
        }

        public PageAttribute(enPageType pageType, bool mustLogin)
            : this(pageType, mustLogin, "")
        {
        }

        public PageAttribute(enPageType pageType, string title)
            : this(pageType, false, title)
        {
        }

        public PageAttribute(enPageType pageType, bool mustLogin, string title)
        {
            m_mustLogin = mustLogin;
            m_title = title;
            m_pageType = pageType;
        }


    }
}
