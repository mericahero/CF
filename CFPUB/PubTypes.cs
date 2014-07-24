using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace COM.CF
{
    public class PubTypes
    {
        private static Regex CheckGoodNameRegEx;

        public static Regex CheckIntsRegEx;

        private static Regex EmailEx;

        private static Regex regisnum;

        static PubTypes()
        {
            regisnum = new Regex("^-?\\d{1,10}$", RegexOptions.Compiled | RegexOptions.ECMAScript);
            EmailEx = new Regex("^(\\w|\\-|\\.)+\\w@(\\w|\\-)+(\\.(\\w|\\-)+)*\\.[A-Za-z]{2,}$", RegexOptions.Compiled | RegexOptions.ECMAScript);
            CheckGoodNameRegEx = new Regex("[\\\\\\|\\*\\.'\",<>\\[\\]%@&;]", RegexOptions.Compiled);
            CheckIntsRegEx = new Regex("^(\\s*\\-?\\d+\\s*,)*\\s*\\-?\\d+\\s*$", RegexOptions.Compiled);
        }

        public static bool CheckEmail(string email)
        {
            return EmailEx.IsMatch(email);
        }

        public static bool CheckGoodName(string s)
        {
            bool flag;
            if (!CheckGoodNameRegEx.IsMatch(s))
            {
                StringBuilder sb = new StringBuilder(s);
                int length = sb.Length - 1;
                int i = 0;
                while (true)
                {
                    if (i > length) break;
                    if (char.IsControl(sb.ToString(), i))
                    {
                        return false;
                    }
                    i++;
                }
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public static bool CheckInts(string s)
        {
            return s != "" && CheckIntsRegEx.IsMatch(s);
        }

        public static string DelBadUnionCodeString(string s)
        {
            if (s == "")
            {
                return "";
            }
            return DelBadUnionCodeStringBuilder(new StringBuilder(s)).ToString();
        }

        public static StringBuilder DelBadUnionCodeStringBuilder(string s)
        {
            return DelBadUnionCodeStringBuilder(new StringBuilder(s));
        }

        public static StringBuilder DelBadUnionCodeStringBuilder(StringBuilder sb)
        {
            int length = sb.Length - 1;
            for (int i = 0; i <= length; i++)
            {
                int iVal = Convert.ToInt32(sb[i]);
                if (iVal > 0xfffd)
                {
                    sb[i] = ' ';
                }
                else if ((iVal < 0x20) & (((iVal != 9) & (iVal != 13)) & (iVal != 10)))
                {
                    sb[i] = ' ';
                }
            }
            return sb;
        }




        public static string DelCtrlChar(string s)
        {
            int i = 0;
            while (true)
            {
                if (i > s.Length)
                { break; }
                if (char.IsControl(s, i)) { s.Remove(i, 1); }
                i++;
            }
            return s;
        }

        public static bool GetBoolean(string str)
        {
            return str == "1";
        }

        public static byte GetByte(string str)
        {
            byte num;
            if (str != null && str != "")
            {
                try
                {
                    num = byte.Parse(str);
                }
                catch
                {
                    num = 0;
                }
            }
            else
            {
                num = 0;
            }
            return num;
        }

        public static int GetByteLength(string s)
        {
            return (s != null && s != "") ? Encoding.Default.GetByteCount(s) : 0;
        }
        /// <summary>
        /// 从一个字符串中得日期字符串
        /// 
        ///     2014-7-23   完善  增加了时间格式，并带上默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static string GetDate(object o,string fmt="yyyy-MM-dd HH:mm")
        {
            if (o == null) return "";
            var str = o.ToString();
            if (string.IsNullOrWhiteSpace(str)) return "";
            try
            {
                DateTime dt = DateTime.Parse(str);
                return (DateTime.Compare(dt, new DateTime(0x851055320574000L)) < 0 | DateTime.Compare(dt, new DateTime(0x91a2ef3b5ed8000L)) > 0) ? "" : dt.ToString(fmt);
            }
            catch
            {
                return "";
            }
        }

        public static bool IsNum(string s)
        {
            return s != "" ? regisnum.IsMatch(s) : false;
        }
    }
}
