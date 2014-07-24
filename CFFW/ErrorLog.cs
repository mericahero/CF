﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using Microsoft.VisualBasic.CompilerServices;

namespace COM.CF
{
    public class ErrorLog
    {
        // Methods
        public static void PayExcept(int r,  bool throwOther=true)
        {
            if (r < 0)
            {
                switch (r)
                {
                    case -900:
                        throw new CFException("没有在该服务器上注册！");

                    case -901:
                        throw new CFException("礼金不足！");

                    case -1000:
                        throw new CFException(enErrType.NormalError, "帐户余额不足！");

                    case -11111:
                        throw new CFException(enErrType.NormalError, "SQL操作失败！errorno=" + Convert.ToString(r));
                }
                if (throwOther)
                {
                    throw new CFException(enErrType.NormalError, "未知错误！errorno=" + Convert.ToString(r));
                }
            }
        }

        public static void WriteLog(Exception e)
        {
            if (e.GetType().ToString() != "System.Web.HttpRequestValidationException")
            {
                HttpRequest request = HttpContext.Current.Request;
                StreamWriter writer = new StreamWriter(CFConfig.logFileDir + "deverror.log", true, Encoding.UTF8);
                try
                {
                    writer.WriteLine("——————————————————————————E1");
                    writer.WriteLine(DateTime.Now.ToString(CFConfig.RQFormatStr));
                    writer.WriteLine("URL=" + request.RawUrl);
                    writer.WriteLine();
                    if (e != null)
                    {
                        writer.WriteLine(e.ToString());
                    }
                    if (request.RequestType == "POST")
                    {
                        if (request.ContentLength > 0xfa0)
                        {
                            writer.WriteLine("POST-length:" + Convert.ToString(request.ContentLength));
                        }
                        else
                        {
                            writer.WriteLine("POST:" + request.Form.ToString());
                        }
                    }
                    writer.WriteLine("IP=" + request.UserHostAddress + ":" + FWFunc.GetIP());
                    writer.Write(request.ServerVariables["ALL_RAW"]);
                }
                finally
                {
                    writer.Close();
                }
            }
        }

        public static void WriteLog(string info)
        {
            WriteLog(info, "deverror.log");
        }

        public static void WriteLog(string info, string filename)
        {
            HttpRequest request = HttpContext.Current.Request;
            StreamWriter writer = new StreamWriter(CFConfig.logFileDir + filename, true, Encoding.UTF8);
            try
            {
                writer.WriteLine("——————————————————————————L");
                writer.WriteLine(DateTime.Now.ToString(CFConfig.RQFormatStr));
                writer.WriteLine("URL=" + request.RawUrl);
                writer.WriteLine();
                writer.WriteLine(info);
                if (request.RequestType == "POST")
                {
                    if (request.ContentLength > 0xfa0)
                    {
                        writer.WriteLine("POST-length:" + Convert.ToString(request.ContentLength));
                    }
                    else
                    {
                        writer.WriteLine("POST:" + request.Form.ToString());
                    }
                }
                writer.WriteLine("IP=" + request.UserHostAddress + ":" + FWFunc.GetIP());
                writer.Write(request.ServerVariables["ALL_RAW"]);
            }
            finally
            {
                writer.Close();
            }
        }

        public static void WriteRawLog(string info, string filename)
        {
            StreamWriter writer = new StreamWriter(CFConfig.logFileDir + filename, true, Encoding.UTF8);
            try
            {
                writer.Write(DateTime.Now.ToString(CFConfig.RQFormatStr));
                writer.Write(":");
                writer.WriteLine(info);
            }
            finally
            {
                writer.Close();
            }
        }


        public static void WrtieBusy(Exception e)
        {
            HttpRequest request = HttpContext.Current.Request;
            string[] str = new string[] { CFConfig.logFileDir, Conversions.ToString(CFConfig.HomeID), "/busy", null, null };
            DateTime now = DateTime.Now;
            str[3] = now.ToString("yyyy-MM-dd");
            str[4] = ".log";
            StreamWriter streamWriter = new StreamWriter(string.Concat(str), true, Encoding.UTF8);
            try
            {
                now = DateTime.Now;
                streamWriter.Write(now.ToString(CFConfig.RQFormatStr));
                streamWriter.WriteLine(string.Concat(" = ", request.RawUrl, " ", e.Message));
            }
            finally
            {
                streamWriter.Close();
            }
        }

        
    }


}
