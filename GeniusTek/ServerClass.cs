using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Diagnostics;

namespace GeniusTek
{
    /// <summary>
    /// 功能：对进入站点的请求进行URL转发，分发到交互层的方法中进行处理
    /// 时间：2013-10-22
    /// 作者：Meric
    /// </summary>
    public class ServerClass:IHttpHandler,IRequiresSessionState
    {
        private static string DefaultAssemblyName;

        private static string DefaultNamespace;

        private static enAnalyzeType AnalyzeType;

        private static IGetTypeName oGetTypeName;

        private static IGetTypeName GetTypeNameObject
        {
            get{return oGetTypeName;}
            set { oGetTypeName = value; }
        }
        /// <summary>
        /// 处理程序
        /// </summary>
        /// <param name="context">执行上下文</param>
        public void ProcessRequest(HttpContext context)
        {
            Type type = Type.GetType(GetTypeName(context.Request));
            if (type != null)
            {
                ((PageBase)Activator.CreateInstance(type)).EventMain();
            }
            else
            {
                context.Response.Write(string.Format("{{\"r\":\"-9999\",\"msg\":\"{0}\"}}",GetTypeName(context.Request)+" 不存在"));
            }
        }

        /// <summary>
        /// IsResulable
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ServerClass()
        {
            AnalyzeType = enAnalyzeType.DefaultNamespaceAndAssemblyNameAppendWeb;
            DefaultNamespace = "";
            DefaultAssemblyName = "";
        }

        /// <summary>
        /// 根据传入的HttpRequest对象获得类 方法名称
        /// </summary>
        /// <param name="request">当前请求的Request对象</param>
        /// <returns>返回接管请求的程序名称</returns>
        private string GetTypeName(HttpRequest request)
        {
            string defaultAssemblyName;
            string str;
            if (oGetTypeName==null)
            {
                string str1 = request.FilePath.Substring(request.FilePath.LastIndexOf("/") + 1);
                if (request.ApplicationPath!="/")
                {
                    defaultAssemblyName = request.FilePath.Substring(request.ApplicationPath.Length + 1);
                }
                else
                {
                    defaultAssemblyName = request.FilePath.Substring(1);
                }
                if (defaultAssemblyName.StartsWith("cgi-bin/"))
                {
                    defaultAssemblyName = defaultAssemblyName.Substring(8);
                }
                if (defaultAssemblyName.StartsWith("handle/"))
                {
                    defaultAssemblyName = defaultAssemblyName.Substring(7);
                }
                int length = defaultAssemblyName.Length - str1.Length - 1;
                if (length<=0)
                {
                    defaultAssemblyName = DefaultAssemblyName;
                }
                else
                {
                    defaultAssemblyName = defaultAssemblyName.Substring(0, length).Replace("/", ".");
                }
                str1 = str1.Substring(0, str1.Length - 5);
                StringBuilder sb = new StringBuilder(32);               

                switch (AnalyzeType)
                {
                    case enAnalyzeType.DefaultNamespaceAndAssemblyName:
                        str = sb.Append(DefaultNamespace).Append(defaultAssemblyName).Append(".").Append(str1).Append(",").Append(defaultAssemblyName).ToString();
                        break;
                    case enAnalyzeType.FullClassName:
                        str = string.Concat(str1, ",", defaultAssemblyName);
                        break;
                    case enAnalyzeType.DefaultNamespace:
                        str = string.Concat(DefaultNamespace, str1, ",", defaultAssemblyName);
                        break;
                    case enAnalyzeType.DefaultNamespaceAndAssemblyNameAppendWeb:
                        str = sb.Append(DefaultNamespace).Append(defaultAssemblyName).Append(".Web.").Append(str1).Append(",").Append(defaultAssemblyName).Append(".Web").ToString();
                        break;
                    default:
                        str = string.Concat(str1, ",", defaultAssemblyName);
                        break;
                }
            }
            else
            {
                str = oGetTypeName.GetTypeName(request);
            }
            return str;
        }

        public static void Init(string defaultNameSpace, enAnalyzeType analyzeType, string defaultAssemblyNmae)
        {
            DefaultNamespace = defaultNameSpace;
            if (DefaultNamespace != "")
            {
                DefaultNamespace = string.Concat(DefaultNamespace, ".");
            }
            AnalyzeType = analyzeType;
            DefaultAssemblyName = defaultAssemblyNmae;
        }



    }
}
