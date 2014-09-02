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
    public class CWPub
    {
        // Fields
        private static string[] extary = new string[] { "gif", "jpg", "png", "jpeg" };
        public static Random Rand = new Random();
        private static Regex regex1 = new Regex("['\"<>\\/\\\\]", RegexOptions.Compiled);
        private static Regex regexsj = new Regex("^(13[0-9]{9})|(15[012356789][0-9]{8})|(18[056789][0-9]{8})$", RegexOptions.IgnoreCase);

        // Methods

        public static bool CheckGuiZe1(string s)
        {
            if (regex1.IsMatch(s))
            {
                return false;
            }
            return true;
        }

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


        public static string GetIPFromInt32(int ip)
        {
            return string.Join(".", (ip >> 0x18) & 0xff, (ip >> 0x10) & 0xff, (ip >> 0x8) & 0xff, ip & 0xff);
        }

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
        /// <returns></returns>
        public static Int64 GenerateBigInt()
        {
            return ((long)Rand.Next() << 32) + (long)Rand.Next();
        }



        /// <summary>
        /// 10进制转化为32进制
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
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
        /// <param name="s"></param>
        /// <returns></returns>
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

        public static bool isExtName(string extName)
        {
            return extName.Contains(extName);
        }



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
