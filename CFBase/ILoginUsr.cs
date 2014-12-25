using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF
{
    /// <summary>
    /// 功能：登录用户接口
    /// 时间：2013-10-1
    /// 作者：meric
    /// </summary>
    public interface ILoginUsr
    {
        /// <summary>
        /// 判断是否登录
        /// </summary>
        bool Logined { get; }
        /// <summary>
        /// 控制页面必须登录
        /// </summary>
        void MustLogin();
    }
}
