using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GeniusTek
{
    /// <summary>
    /// 功能：获取处理请求的程序名
    /// 时间：2014-10-21
    /// 作者：Meric
    /// </summary>
    internal interface IGetTypeName
    {
        /// <summary>
        /// 获取处理请求的程序名
        /// </summary>
        /// <param name="request">请求的request对象</param>
        /// <returns></returns>
        string GetTypeName(HttpRequest request);
    }
}
