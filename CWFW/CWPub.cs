using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using System.Web;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace CWS
{
    /// <summary>
    /// 功能：CF框架内的网站所能使用的一些方法及公共属性
    /// 时间：2014-10-21
    /// 作者：Meric
    /// </summary>
    public class CWPub
    {
        //网站可以上传的图片的后缀
        private static string[] extary = new string[] { "gif", "jpg", "png", "jpeg" };
        /// <summary>
        /// 全局的随机数对象
        /// </summary>
        public static Random Rand = new Random();
        //手机号正则       
        private static Regex regexsj = new Regex("^(13[0-9]{9})|(15[012356789][0-9]{8})|(18[056789][0-9]{8})$", RegexOptions.IgnoreCase);


        /// <summary>
        /// 判断是否是合法手机号
        /// </summary>
        /// <param name="sj"></param>
        /// <returns></returns>
        public static bool CheckSJ(string sj)
        {
            if (!Versioned.IsNumeric(sj))
            {
                return false;
            }
            if (sj.Length != 11)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        ///批量检测手机号是否合法
        /// </summary>
        /// <param name="sj">手机号数组</param>
        /// <returns>返回每个手机的检测结果，检测成功返回1否则返回0，如3个手机号，第1个和第3个检测成功，第2个失败，则返回101</returns>
        public static string CheckSJS(string[] sj)
        {
            string r = "";
            for (int i = 0; i <= sj.Length; i++)
            {
                if (!CheckSJ(sj[i]))
                {
                    r = r + "1";
                }
                else
                {
                    r = r + "0";
                }
            }
            return r;
        }
        /// <summary>
        /// 过滤数组中的重复项
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Array GetArrayDistinct(string[] v)
        {
            List<string> l = new List<string>();
            for (int i = 0; i <= v.Length; i++)
            {
                string v1 = v[i].Trim();
                if (((l.IndexOf(v1.ToLower()) == -1) & (v1 != "")) & !PubFunc.isNaN(v1))
                {
                    l.Add(v1);
                }
            }
            return l.ToArray();
        }

        /// <summary>
        /// 生成前台唯一的标识Cookie的GUID
        /// </summary>
        /// <returns>返回生成的Guid</returns>
        public static Guid GetCookieGUID()
        {
            Guid guid1;
            HttpCookie c = HttpContext.Current.Request.Cookies["guid"];
            if (c == null)
            {
                return Guid.Empty;
            }
            if (!Guid.TryParse(c.Value, out guid1))
            {
                return Guid.Empty;
            }
            return guid1;
        }
        /// <summary>
        /// 从一个整数中获得到IP地址
        /// </summary>
        /// <param name="ip">整数形式的IP地址</param>
        /// <returns>返回IP地址</returns>
        public static string GetIPFromInt32(int ip)
        {
            return string.Join(".", (ip >> 0x18) & 0xff, (ip >> 0x10) & 0xff, (ip >> 0x8) & 0xff, ip & 0xff);
        }
        /// <summary>
        /// 将IP地址转换成一个整数
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>返回转换后的整数</returns>
        public static int GetIPAsInt32(string ip)
        {
            string[] arrayIP = ip.Split(new char[] { '.' });
            if (arrayIP.Length < 4)
            {
                return 0;
            }
            int sip1 = PubFunc.GetInt(arrayIP[0]);
            int sip2 = PubFunc.GetInt(arrayIP[1]);
            int sip3 = PubFunc.GetInt(arrayIP[2]);
            int sip4 = PubFunc.GetInt(arrayIP[3]);

            return ((((sip1 << 0x18) | (sip2 << 0x10)) | (sip3 << 8)) | sip4);
        }

        #region 进制转换
        /// <summary>
        /// 生成64位随机整数
        /// </summary>
        /// <returns>返回生成的64整数</returns>
        public static Int64 GenerateBigInt()
        {
            return ((long)Rand.Next() << 32) + (long)Rand.Next();
        }
        
        /// <summary>
        /// 10进制转化为32进制并反转
        /// </summary>
        /// <param name="n">10进制数</param>
        /// <returns>转换为32进制并反转</returns>
        public static string Convert10To32(Int64 n)
        {
            //return TGPub.Convert10To32(n);
            Int64 a = n & 0x1f;
            string b = "";
            if (a < 10)
            {
                b = a.ToString();
            }
            else
            {
                b = ((Char)(a + 87)).ToString();
            }
            n >>= 5;
            while (n > 0)
            {
                a = n & 0x1f;
                if (a < 10)
                {
                    b += a.ToString();
                }
                else
                {
                    b += ((Char)(a + 87)).ToString();
                }
                n >>= 5;
            }
            return b;
        }

        /// <summary>
        /// 32进制转化为10进制
        /// </summary>
        /// <param name="s">32进制的数</param>
        /// <returns>返回10进制数</returns>
        public static Int64 Convert32To10(string s)
        {
            //return TGPub.Convert32To10(s);
            if (string.IsNullOrWhiteSpace(s)) return 0;
            var chs = s.ToCharArray().Reverse().ToArray();
            Int64 num = 0;
            for (int i = 0; i < chs.Length; ++i)
            {
                int k = 0;
                if (!int.TryParse(chs[i].ToString(), out k))
                {
                    k = Convert.ToInt32(chs[i]);
                    if (k > 9)
                    {
                        k = k - 87;
                    }
                }
                if (k > 31) return 0;
                num += (Int64)(k * Math.Pow(32, chs.Length - i - 1));
            }
            return num;
        }
        #endregion

        /// <summary>
        /// 字符串转为浮点数
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static decimal GetMoney(string dec)
        {
            decimal n;
            if (decimal.TryParse(dec, out n))
            {
                return n;
            }
            return decimal.Zero;
        }

        public static string GetMoneyStr(decimal dec)
        {
            return Math.Round(dec, 2).ToString();
        }
        /// <summary>
        /// 获得正确的手机号，+86的去掉+86
        /// </summary>
        /// <param name="sj">手机号</param>
        /// <returns>返回正确的手机号</returns>
        public static string GetSJ(string sj)
        {
            if (sj == null)
            {
                return "";
            }
            if (sj.StartsWith("+86"))
            {
                sj = sj.Substring(3);
            }
            return sj;
        }

        /// <summary>
        /// 判断一个对象是否是一个日期
        /// </summary>
        /// <param name="obj">待判断的对象</param>
        /// <returns>返回是否是日期</returns>
        public static bool IsDate(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            try
            {
                DateTime d = DateTime.Parse(obj.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是正确的图片后缀
        /// </summary>
        /// <param name="extName">后缀名</param>
        /// <returns>返回是否正确</returns>
        public static bool isExtName(string extName)
        {
            return extName.Contains(extName);
        }


        /// <summary>
        /// md5加密并计算Hash
        /// </summary>
        /// <param name="s">待计算的字符串</param>
        /// <returns>返回md5加密并计算hash后的值</returns>
        public static string Md5Hash(string s)
        {
            byte[] data = MD5.Create().ComputeHash(Encoding.Default.GetBytes(s));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i <= data.Length - 1; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="obj">待转换对象</param>
        /// <returns>转换后的日期值</returns>
        public static DateTime ToDate(object obj)
        {
            if (IsDate(RuntimeHelpers.GetObjectValue(obj)))
            {
                return Conversions.ToDate(obj);
            }
            return DateTime.Now;
        }

    }




}
