using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Diagnostics;

namespace GeniusTek
{
    public class ServerClass:IHttpHandler,IRequiresSessionState
    {
        private static string DefaultAssemblyName;

        private static string DefaultNamespace;

        private static enAnalyzeType AnalyzeType;

        private static IGetTypeName oGetTypeName;

        public static IGetTypeName GetTypeNameObject
        {
            get{return oGetTypeName;}
            set { oGetTypeName = value; }
        }
        //处理程序
        public void ProcessRequest(HttpContext context)
        {
            //HttpContext.Current.Response.Write(GetTypeName(context.Request));
            //return;


            Type type = Type.GetType(GetTypeName(context.Request));
            if (type != null)
            {
                ((PageBase)Activator.CreateInstance(type)).EventMain();
            }
            else
            {
                context.Response.Write(string.Concat(GetTypeName(context.Request), "不存在<hr>"));
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
        /// <param name="request"></param>
        /// <returns></returns>
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
