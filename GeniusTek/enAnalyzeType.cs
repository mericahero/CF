using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeniusTek
{
    /// <summary>
    /// 功能：解析程序集的方式
    /// 时间：2014-10-22
    /// 作者：meric
    /// </summary>
    public enum enAnalyzeType
    {
        /// <summary>
        /// 默认命名空间+程序集
        /// </summary>
        DefaultNamespaceAndAssemblyName,
        /// <summary>
        /// 完整的类名（包括程序集、命名空间）
        /// </summary>
        FullClassName,
        /// <summary>
        /// 默认命名空间
        /// </summary>
        DefaultNamespace,
        /// <summary>
        /// 默认命名空间+程序集+拼接上的".Web"字符串
        /// </summary>
        DefaultNamespaceAndAssemblyNameAppendWeb
    }
}
