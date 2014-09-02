using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Net;
using System.IO.Compression;
using System.Globalization;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;


namespace COM.CF
{
    /// <summary>
    /// 功能：CF框架的公共方法
    /// 时间：2013-10-21
    /// 作者：meric
    /// </summary>
    public class PubFunc
    {
        /// <summary>
        /// 删除QueryString中的指定键
        /// </summary>
        /// <param name="query"></param>
        /// <param name="delkey"></param>
        /// <returns></returns>
        public static String DelQueryName(string query, string delkey)
        {
            string str;
            if (string.IsNullOrWhiteSpace(query)) return "";

            if (!query.StartsWith("?"))
            {
                query = "?" + query;
            }
            query = Regex.Replace(query, String.Concat(@"[\?&]*", delkey, "=[^&]*"), "", RegexOptions.IgnoreCase);
            if (!query.StartsWith("&"))
            {
                str = query;
            }
            else
            {
                str = query.Substring(1);
            }

            return str;
        }
        /// <summary>
        /// 重复给定的字符串
        /// </summary>
        /// <param name="s">给定字符串</param>
        /// <param name="n">重复次数</param>
        /// <returns></returns>
        public static String DupStr(string s, int n)
        {
            string str;
            if (s != "" && n != 0)
            {
                StringBuilder sb = new StringBuilder(s.Length * n);
                int i = 1;
                while (true)
                {
                    if (n < i) break;
                    sb.Append(s);
                    i++;
                }
                str = sb.ToString();
            }
            else
            {
                str = "";
            }
            return str;
        }

        #region HTTP请求
        public static bool FowardHTTP(string newhost)
        {
            return FowardHTTP(newhost, null);
        }
        public static bool FowardHTTP(string newhost, string newpath)
        {
            bool flag;
            string end;
            Encoding @default;
            Stream responseStream;
            HttpContext context = HttpContext.Current;
            string str = newhost;
            str = (newpath == null) ? str + context.Request.RawUrl : str + newpath;

            try
            {
                HttpWebRequest userAgent = (HttpWebRequest)WebRequest.Create(str);
                HttpWebRequest httpWebRequest = userAgent;
                httpWebRequest.AllowAutoRedirect = false;
                httpWebRequest.Headers["Accept-Encoding"] = "gzip";
                if (context.Request.UserAgent != "")
                {
                    userAgent.UserAgent = context.Request.UserAgent;
                }
                if (context.Request.Headers["Cookie"] != "")
                {
                    userAgent.Headers["Cookie"] = context.Request.Headers["Cookie"];
                }
                HttpWebResponse response = (HttpWebResponse)userAgent.GetResponse();
                try
                {
                    if (!response.ContentType.ToLower().Contains("utf-8"))
                    {
                        @default = Encoding.Default;
                    }
                    else
                    {
                        @default = Encoding.UTF8;
                    }
                    if (response.ContentEncoding != "gzip")
                    {
                        responseStream = response.GetResponseStream();
                    }
                    else
                    {
                        responseStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress, true);
                    }
                    StreamReader streamReader = new StreamReader(responseStream, @default);
                    end = streamReader.ReadToEnd();
                }
                finally
                {
                    response.Close();
                }
                HttpStatusCode statusCode = response.StatusCode;
                if (statusCode != HttpStatusCode.OK)
                {
                    if (statusCode != HttpStatusCode.Found)
                    {
                        if (statusCode != HttpStatusCode.MovedPermanently)
                        {
                            flag = false;
                        }
                        else
                        {
                            context.Response.Redirect(response.Headers["Location"], false);
                            flag = true;
                        }
                    }
                    else
                    {
                        context.Response.Redirect(response.Headers["Location"], false);
                        flag = true;
                    }
                }
                else
                {
                    context.Response.Write(end);
                    context.Response.ContentType = response.ContentType;
                    flag = true;
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }



        public static string SendHTTP(string url)
        {
            return SendHTTP(url, "", true);
        }





        public static string SendHTTP(string url, ref HttpWebResponse rep)
        {
            try
            {
                string str2;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebRequest request2 = request;
                request2.AllowAutoRedirect = false;
                request2.Headers["Accept-Encoding"] = "gzip";
                rep = (HttpWebResponse)request2.GetResponse();
                try
                {
                    Encoding encoding;
                    Stream responseStream;
                    if (rep.ContentType.ToLower().IndexOf("utf-8") >= 0)
                    {
                        encoding = Encoding.UTF8;
                    }
                    else
                    {
                        encoding = Encoding.Default;
                    }
                    if (rep.ContentEncoding == "gzip")
                    {
                        responseStream = new GZipStream(rep.GetResponseStream(), CompressionMode.Decompress, true);
                    }
                    else
                    {
                        responseStream = rep.GetResponseStream();
                    }
                    str2 = new StreamReader(responseStream, encoding).ReadToEnd();
                }
                finally
                {
                    rep.Close();
                }
                return str2;
            }
            catch
            {
                return "";
            }
        }







        public static string SendHTTP(string url, string agent)
        {
            return SendHTTP(url, agent, true);
        }









        public static string SendHTTP(string url, string agent, bool gzip)
        {
            try
            {
                string str2;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebRequest request2 = request;
                if (agent != "")
                {
                    request2.UserAgent = agent;
                }
                if (gzip)
                {
                    request2.Headers["Accept-Encoding"] = "gzip";
                }
                HttpWebResponse response = (HttpWebResponse)request2.GetResponse();
                try
                {
                    Encoding encoding;
                    Stream responseStream;
                    if (response.ContentType.ToLower().IndexOf("utf-8") >= 0)
                    {
                        encoding = Encoding.UTF8;
                    }
                    else
                    {
                        encoding = Encoding.Default;
                    }
                    if (response.ContentEncoding == "gzip")
                    {
                        responseStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress, true);
                    }
                    else
                    {
                        responseStream = response.GetResponseStream();
                    }
                    str2 = new StreamReader(responseStream, encoding).ReadToEnd();
                }
                finally
                {
                    response.Close();
                }
                return str2;
            }
            catch
            {
                return "";

            }
        }




        /// <summary>
        /// 获得http返回字符串，提交方式为post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <param name="sendEncoding"></param>
        /// <param name="isgunzip"></param>
        /// <returns></returns>
        public static string PostHTTP(string url,
            NameValueCollection form,
             Encoding sendEncoding = null,
             bool isgunzip = true)
        {

            StringBuilder sb = new StringBuilder();

            foreach (string s1 in form.AllKeys)
            {
                if (sb.Length > 0)
                {
                    sb.Append('&');
                }
                //sb.Append(s1).Append('=').Append(HttpUtility.UrlEncode(form[s1], sendEncoding));
                sb.Append(s1).Append('=').Append(form[s1]);
            }
            return PostHTTP(url, sb.ToString(), sendEncoding, isgunzip);
        }



        /// <summary>
        /// 获得http返回字符串，提交方式为post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <param name="sendEncoding"></param>
        /// <param name="isgunzip"></param>
        /// <returns></returns>
        public static string PostHTTP(string url,
            string postStr,
            Encoding sendEncoding = null,
            bool isgunzip = true)
        {
            string returnStr;
            StringBuilder sb = new StringBuilder();
            if (sendEncoding == null)
            {
                sendEncoding = Encoding.UTF8;
            }
            sb.Append(postStr);
            byte[] a = Encoding.ASCII.GetBytes(sb.ToString());
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                myReq.Method = "POST";
                myReq.ContentType = "application/x-www-form-urlencoded";
                myReq.ContentLength = a.Length;
                myReq.Headers["Accept-Encoding"] = "gzip";
                Stream s = myReq.GetRequestStream();
                try
                {
                    s.Write(a, 0, a.Length);
                }
                finally
                {
                    s.Close();
                }
                HttpWebResponse rep = (HttpWebResponse)myReq.GetResponse();
                try
                {
                    Encoding encoding;
                    if (rep.ContentType.ToLower().Trim().IndexOf("utf-8") > 0)
                    {
                        encoding = Encoding.UTF8;
                    }
                    else
                    {
                        encoding = Encoding.Default;
                    }
                    if (rep.ContentEncoding == "gzip")
                    {
                        s = new GZipStream(rep.GetResponseStream(), CompressionMode.Decompress, true);
                    }
                    else
                    {
                        s = rep.GetResponseStream();
                    }
                    returnStr = new StreamReader(s, encoding).ReadToEnd();
                }
                finally
                {
                    rep.Close();
                }
                return returnStr;
            }
            catch
            {
                returnStr = "";
            }
            return returnStr;
        }


        #endregion
        







        public static byte GetByte(string s)
        {
            byte num;
            if (string.IsNullOrWhiteSpace(s)) return 0;
            
            int num1 = PubFunc.GetInt(s);
            if (num1 < 0 || num1 > 0xff)
            {
                num = 0;
            }
            else
            {
                num = Convert.ToByte(num1);
            }

            return num;
        }

        public static char GetCharByNumber(int n)
        {
            return n < 10 ? Convert.ToChar(n + Convert.ToInt32(48)) :  Convert.ToChar(n - 10 + Convert.ToInt32(97));
        }

        /// <summary>
        /// 获得默认整型
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="defaultInt">默认数</param>
        /// <returns></returns>
        public static int GetDefaultInt(string inputStr, int defaultInt)
        {
            if (string.IsNullOrWhiteSpace(inputStr)) return defaultInt;
            inputStr = inputStr.Trim();
            try
            {
                if (!inputStr.StartsWith("0x"))
                {
                    if (string.IsNullOrWhiteSpace(inputStr) || !(new Regex("^-?\\d{1,10}$", RegexOptions.Compiled | RegexOptions.ECMAScript).IsMatch(inputStr)))
                    {
                        return defaultInt;
                    }
                    else
                    {
                        return int.Parse(inputStr);
                    }
                }
                else
                {
                    return int.Parse(inputStr.Substring(2), NumberStyles.AllowHexSpecifier);
                }
            }
            catch                     
            {
                return defaultInt;
            }

        }

        public static string GetDefaultStr(string inputStr)
        {

            return PubFunc.GetDefaultStr(inputStr, "");
        }





        public static string GetDefaultStr(string inputStr, string defaultStr)
        {
            string str;
            if (inputStr != null)
            {
                inputStr = inputStr.Trim();
                if (inputStr !="")
                {
                    str = inputStr;
                }
                else
                {
                    str = defaultStr;
                }
            }
            else
            {
                str = defaultStr;
            }
            return str;
        }

        public static string GetDefaultValue(NameValueCollection settings, string key, string defaultValue)
        {
            string str;
            try
            {
                object item = settings[key];
                if (item!=null)
                {
                    str = Convert.ToString(item);
                }
                else
                {
                    str = defaultValue;
                }
            }
            catch
            {
                str = defaultValue;
            }
            return str;
        }

        public static bool GetDefaultValue(NameValueCollection settings, string key, bool defaultValue)
        {
            bool flag;
            try
            {
                object item = settings[key];
                if (item!=null)
                {
                    flag = Convert.ToBoolean(item);
                }
                else
                {
                    flag = defaultValue;
                }
            }
            catch
            {
                flag = defaultValue;
            }
            return flag;
        }

        public static int GetDefaultValue(NameValueCollection settings, string key, int defaultValue)
        {
            int num;
            try
            {
                string item = settings[key];
                if (item!=null)
                {
                    if (!item.StartsWith("0x"))
                    {
                        num = int.Parse(item);
                    }
                    else
                    {
                        num = int.Parse(item.Substring(2), NumberStyles.AllowHexSpecifier);
                    }
                }
                else
                {
                    num = defaultValue;
                }
            }
            catch
            {
                num = defaultValue;
            }
            return num;
        }        
        /// <summary>
        /// 从对象中获得整数
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int GetInt(object o)
        {
            if (o == null) return 0;
            var s = o.ToString();
            if (string.IsNullOrWhiteSpace(s)) return 0;
            try
            {
                return s.StartsWith("0x") ? int.Parse(s.Substring(2), NumberStyles.AllowHexSpecifier) : (PubTypes.IsNum(s) ? int.Parse(s) : 0);

            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 从对象中获得长整数
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static long GetBigInt(object o)
        {
            if (o == null) return 0;
            var s = o.ToString();
            if (string.IsNullOrWhiteSpace(s)) return 0;

            try
            {
                return s.StartsWith("0x") ? long.Parse(s.Substring(2), NumberStyles.AllowHexSpecifier) : (PubTypes.IsNum(s) ? long.Parse(s) : 0);
            }
            catch
            {
                return 0;
            }
        }


        public static string GetHost(string s)
        {
            if ((s == null) || (s == ""))
            {
                return "";
            }
            s = s.ToLower();
            if (!s.StartsWith("http://"))
            {
                return "";
            }
            s = s.Substring(7);
            int index = s.IndexOf("/");
            if (index < 0)
            {
                return s;
            }
            return s.Substring(0, index);
        }




        public static int GetLowInt32(long i)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(i), 0);
        }

        public static decimal GetMoney(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return decimal.Zero;


            try
            {
                return decimal.Parse(s);
            }
            catch
            {
                return decimal.Zero;
            }

        }

        public static string GetNothing(string s, string defaultStr)
        {
            return (s != null) ? s : defaultStr;
        }

        /// <summary>
        /// 获得小数转化为百分比的字符串
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetPercent(double n)
        {
            return Math.Round(n,3).ToString("P");
        }
        /// <summary>
        /// 获得QueryString
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetQeuryString(string s)
        {
            return !string.IsNullOrWhiteSpace(s)? s.Replace("'", "").Trim() :"";
        }


        public static char GetRandChar()
        {
            return PubFunc.GetCharByNumber((new Random()).Next(36));
        }

        /// <summary>
        /// 获得指定位数的随机字符串
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static string GetRandChar(int l)
        {
            string str="";
            Random random = new Random();
            int i = 0;
            while (++i <= l)
            {
                str += Convert.ToString(GetCharByNumber(random.Next(36)));
            }

            return str;
        }
        /// <summary>
        /// 判断输入字符串是否为非法数字，如果合法，输出数字
        /// </summary>
        /// <param name="inputstr"></param>
        /// <param name="outInt"></param>
        /// <returns></returns>
        public static bool isNaN(string inputstr, ref int outInt)
        {
            if (string.IsNullOrWhiteSpace(inputstr)) return true;
            try
            {
                if (!inputstr.StartsWith("0x"))
                {
                    outInt = int.Parse(inputstr);
                    return false;
                }
                else
                {
                    outInt = int.Parse(inputstr.Substring(2), NumberStyles.AllowHexSpecifier);
                    return false;
                }
            }
            catch 
            {
                return true;
            }

        }

        /// <summary>
        /// 判断 字符串是否为非法数字
        /// </summary>
        /// <param name="inputstr"></param>
        /// <returns></returns>
        public static bool isNaN(string inputstr)
        {
            if (string.IsNullOrWhiteSpace(inputstr)) return true;
            try
            {
                if (!inputstr.StartsWith("0x"))
                {
                    if (!PubTypes.IsNum(inputstr)) return true;
                    if (inputstr.Length > 10) return true;
                    int.Parse(inputstr);
                    return false;
                }
                else
                {
                    int.Parse(inputstr.Substring(2), NumberStyles.AllowHexSpecifier);
                    return false;
                }
            }
            catch
            {
                return true;
            }
        }

        public static string JSString(string s)
        {
            return (s==null) ?"" :s.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'");
        }


        public static string HTML2JSStr(string s, string yinhao)
        {
            if (yinhao=="")
            {
                yinhao = "'";
            }
            s = s.Replace("\\", "\\\\").Replace(yinhao, string.Concat("\\", yinhao, "")).Replace("\r\n", "\\r\\n");
            string[] strArrays = new string[5];
            strArrays[0] = "<";
            strArrays[1] = yinhao;
            strArrays[2] = "+";
            strArrays[3] = yinhao;
            strArrays[4] = "/script>";
            return string.Concat(yinhao, Regex.Replace(s, "<\\/scipt>", string.Concat(strArrays), RegexOptions.IgnoreCase), yinhao);
        }


        public static string HTML2JSStr(string s)
        {
            return PubFunc.HTML2JSStr(s, "");
        }


        public static object Nothing2blank(object o)
        {
            return (o==null) ? "" :o;
        }

        public static object Nothing2Null(object o)
        {
            return (o==null) ? DBNull.Value:o;
        }

        public static string Null2Blank(object o)
        {
            return (o!=null && !Convert.IsDBNull(RuntimeHelpers.GetObjectValue(0))) ? o.ToString() : "";
        }

        public static DataRowCollection GetSQLRows(string strsql, SqlConnection con)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, con);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable.Rows;
        }

        public static DataRowCollection GetSQLRows(string strsql, SqlConnection con, int n)
        {
            string[] str = new string[5];
            str[0] = "set rowcount ";
            str[1] = n.ToString();
            str[2] = " ";
            str[3] = strsql;
            str[4] = " set rowcount 0";
            return PubFunc.GetSQLRows(string.Concat(str), con);
        }

        public static DataRowCollection GetSQLRows(string strsql, SqlConnection con, int start, int n)
        {
            string[] str = new string[5];
            str[0] = "set rowcount ";
            int num = start + n;
            str[1] = num.ToString();
            str[2] = " ";
            str[3] = strsql;
            str[4] = " set rowcount 0";
            strsql = string.Concat(str);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, con);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet, start, n, "t");
            return dataSet.Tables[0].Rows;
        }


        public static DataRow GetSQLSingleRow(string strsql, SqlConnection con)
        {
            DataRow item;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, con);
            DataTable dataTable = new DataTable();
            if (sqlDataAdapter.Fill(dataTable) != 0)
            {
                item = dataTable.Rows[0];
            }
            else
            {
                item = null;
            }
            return item;
        }


        public static DataRowCollection CopyRows(DataRowCollection srcRows, int n)
        {
            if (srcRows.Count == 0)
            {
                return new DataTable().Rows;
            }
            DataRowCollection rows = srcRows[0].Table.Copy().Rows;
            int num3 = n;
            for (int i = rows.Count - 1; i >= num3; i += -1)
            {
                rows.RemoveAt(i);
            }
            return rows;
        }

 


    }
}
