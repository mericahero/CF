using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace GeniusTek
{
    /// <summary>
    /// 功能：页面基类
    /// 时间：2013-10-22
    /// 作者：meric
    /// </summary>
    public abstract class PageBase
    {
        private HttpContext _context;
        /// <summary>
        /// 页面上下文
        /// </summary>
        protected HttpContext Context
        {
            get { return _context; }
        }

        private HttpRequest _request;
        /// <summary>
        /// 页面的HttpRequest对象
        /// </summary>
        protected HttpRequest Request
        {
            get { return _request; }
        }
        private HttpResponse _response;
        /// <summary>
        /// 页面的HttpResponse对象
        /// </summary>
        protected HttpResponse Response
        {
            get { return _response; }
        }
        private HttpSessionState _session;
        /// <summary>
        /// 页面的Session
        /// </summary>
        protected HttpSessionState Session
        {
            get { return _session; }
        }
        /// <summary>
        /// 构造函数，对页面内context、request、response、session对象进行初始化
        /// </summary>
        public PageBase()
        {
            _context = HttpContext.Current;
            _request = _context.Request;
            _response = _context.Response;
            _session = _context.Session;
        }
        /// <summary>
        /// 抽象函数，在具体类里面实现。页面的主函数
        /// </summary>
        protected internal abstract void EventMain();

        
    }
}
