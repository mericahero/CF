using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF.Web
{
    /// <summary>
    /// 功能：页面类型枚举
    /// 时间：2013-10-21
    /// 作者：meric
    /// </summary>
    public enum enPageType
    {
        /// <summary>
        /// 自定义页面，可任意输出
        /// </summary>
        SelfPage=-1,
        /// <summary>
        /// 默认页面，包含正常的html代码
        /// </summary>
        DefaultPage=0,
        /// <summary>
        /// XML页面，输出此页面时，需要按xml规范输出
        /// </summary>
        XMLPage=3,
        /// <summary>
        /// JS页面，此页面的返回头部为application/x-javascript
        /// </summary>
        JSPage=4
    }
}
