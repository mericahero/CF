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
        protected HttpContext Context
        {
            get { return _context; }
        }

        private HttpRequest _request;
        protected HttpRequest Request
        {
            get { return _request; }
        }
        private HttpResponse _response;
        protected HttpResponse Response
        {
            get { return _response; }
        }
        private HttpSessionState _session;
        protected HttpSessionState Session
        {
            get { return _session; }
        }

        public PageBase()
        {
            _context = HttpContext.Current;
            _request = _context.Request;
            _response = _context.Response;
            _session = _context.Session;
        }

        protected internal abstract void EventMain();

        
    }
}
