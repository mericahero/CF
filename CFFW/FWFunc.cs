using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.VisualBasic.CompilerServices;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Data;
using System.Web.Caching;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;

namespace COM.CF
{
    public class FWFunc
{
    // Fields
    private static Random _r = new Random();
    private static int[] _randAry = new int[] { 20, 15, 15, 10, 10 };
    private static string[] anQuanbotAry = new string[] { "baiduspider", "googlebot", "yahoo", "qqbot", "sogou", "msnbot", "yodaobot", "yisou" };
    private static string[] botAry = new string[] { 
        "google", "isaac", "bot", "slurp", "spider", "baidu", "archiver", "p.arthur", "crawler", "java", "gather", "sleipnir", "yahoo", "3721", "yisou", "sohu", 
        "openfind", "aol"
     };

    // Methods


    public static bool CheckNeiRong2(string rules, string text)
    {
        if (rules != "")
        {
            if (text == null)
            {
                text = "";
            }
            if (Regex.IsMatch(text, rules, RegexOptions.Singleline | RegexOptions.IgnoreCase))
            {
                return false;
            }
        }
        return true;
    }




    public static string encodeurl(string s)
    {
        return HttpUtility.UrlEncode(s, Encoding.UTF8);
    }




    public static string GetCitySearchSQL(string zname, NameValueCollection requestform, ref string nextstring)
    {
        int @int = PubFunc.GetInt(requestform["sheng"]);
        if (@int != 0)
        {
            @int = @int << 8;
            int num2 = PubFunc.GetInt(requestform["city"]);
            if (num2 != 0)
            {
                nextstring = nextstring + "&sheng=" + Conversions.ToString(@int) + "&city=" + Conversions.ToString(num2);
                return (" and " + zname + "=" + Conversions.ToString((int) (@int + num2)));
            }
            nextstring = nextstring + "&sheng=" + Conversions.ToString(@int) + "&city=0";
            return (" and " + zname + " between " + Conversions.ToString(@int) + " and " + Conversions.ToString((int) (@int + 0xff)));
        }
        return "";
    }

    public static string GetFuJianLink(string autolink)
    {
        if ((autolink != "") && (autolink[0] == '#'))
        {
            switch (autolink[1])
            {
                case 'U':
                case 'L':
                    return ("/cgi-bin/sys/link/f.aspx/" + Conversions.ToString(autolink[1]) + "/" + autolink.Substring(3));
            }
            return "";
        }
        return "";
    }

    public static NameValueCollection GetGBForm(string url)
    {
        NameValueCollection values = new NameValueCollection();
        if (url != "")
        {
            foreach (string str in url.Split(new char[] { '&' }))
            {
                string[] strArray = str.Split(new char[] { '=' });
                if (strArray.Length == 2)
                {
                    values.Add(strArray[0], HttpUtility.UrlDecode(strArray[1], Encoding.Default));
                }
            }
        }
        return values;
    }

   
    public static MemoryStream GetHTTPFile(ref HttpWebResponse rep,string url, string @ref=null, int timeoutsec=0)
    {
        MemoryStream stream;
        try
        {
            MemoryStream stream2;
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            HttpWebRequest request2 = request;
            if (@ref == null)
            {
                request2.Referer = uri.GetLeftPart(UriPartial.Authority);
            }
            else
            {
                request2.Referer = @ref;
            }
            if (timeoutsec > 0)
            {
                request2.Timeout = timeoutsec * 0x3e8;
            }
            rep = (HttpWebResponse) request2.GetResponse();
            try
            {
                if (rep.ContentLength > 0)
                {
                    stream2 = new MemoryStream(Convert.ToInt32(rep.ContentLength));
                }
                else
                {
                    stream2 = new MemoryStream(0x20000);
                }
                Stream responseStream = rep.GetResponseStream();
                byte[] buffer = new byte[0x20000];
                for (int i = responseStream.Read(buffer, 0, buffer.Length); i > 0; i = responseStream.Read(buffer, 0, buffer.Length))
                {
                    stream2.Write(buffer, 0, i);
                }
            }
            finally
            {
                rep.Close();
            }
            return stream2;
        }
        catch (Exception exception1)
        {
            ErrorLog.WriteLog(exception1);
            stream = null;
        }
        return stream;
    }

    public static string GetIP()
    {
        HttpRequest request = HttpContext.Current.Request;
        string input = request.ServerVariables["HTTP_CDN_SRC_IP"];
        if (input == null)
        {
            input = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (input == null)
            {
                return request.UserHostAddress;
            }
        }
        input = input.Replace("127.0.0.1,", "").Replace("127.0.0.1:", "").Trim();
        if ((input != "") && (input.IndexOf("unknown") <= 0))
        {
            input = input.Split(new char[] { ',', ':', ' ' })[0];
            if (Regex.IsMatch(input, @"^\d{1,3}(\.\d{1,3}){3}$"))
            {
                return input;
            }
        }
        return request.UserHostAddress;
    }

    public static string GetMIME(string ext)
    {
        switch (ext.ToLower())
        {
            case ".jpg":
            case ".jpge":
            case ".jpe":
                return "image/jpeg";

            case ".png":
            case ".gif":
            case ".bmp":
                return ("image/" + ext.Substring(1));

            case ".zip":
                return "application/x-zip-compressed";

            case ".doc":
                return "application/msword";

            case ".ram":
            case ".ra":
                return "audio/x-pn-realaudio";

            case ".mp3":
                return "audio/mpeg";

            case ".mpg":
            case ".mpa":
            case ".mpeg":
            case ".mpe":
                return "video/mpeg";

            case ".rm":
                return "application/vnd.rn-realmedia";

            case ".htm":
            case ".html":
                return "text/html";

            case ".aif":
                return "audio/x-aiff";

            case ".au":
                return "audio/basic";

            case ".wave":
                return "audio/wav";

            case ".asf":
                return "video/x-ms-asf";

            case ".mid":
            case ".rmi":
                return "audio/mid";

            case ".torrent":
                return "application/x-bittorrent";
        }
        return "application/octet-stream";
    }

    public static StringBuilder GetMoreSQLXML(SqlCommand cm, StringBuilder s)
    {
        try
        {
            XmlReader reader = cm.ExecuteXmlReader();
            reader.Read();
            while (!reader.EOF)
            {
                s.Append(reader.ReadOuterXml());
            }
            reader.Close();
        }
        finally
        {
            cm.Connection.Close();
        }
        return s;
    }

    public static string GetPaiMingChange(object c)
    {
        if (Information.IsDBNull(RuntimeHelpers.GetObjectValue(c)))
        {
            return "<span style='color:green'>↑↑</span>";
        }
        int num = Convert.ToInt32(RuntimeHelpers.GetObjectValue(c));
        if (num > 0)
        {
            return (Conversions.ToString(num) + "<span style='color:green'>↑</span>");
        }
        if (num < 0)
        {
            return (Conversions.ToString((int) (0 - num)) + "<span style='color:red'>↓</span>");
        }
        return "-";
    }

    public static string GetPathFirstPart()
    {
        string str2 = null;
        return GetPathFirstPart(ref str2);
    }

    public static string GetPathFirstPart(ref string p2)
    {
        string str2;
        string pathInfo = HttpContext.Current.Request.PathInfo;
        if (pathInfo == "")
        {
            return "";
        }
        pathInfo = pathInfo.Substring(1);
        int index = pathInfo.IndexOf('/');
        if (index < 0)
        {
            str2 = pathInfo;
            p2 = "";
        }
        else
        {
            str2 = pathInfo.Substring(0, index);
            p2 = pathInfo.Substring(index + 1);
        }
        if (str2.EndsWith(".htm"))
        {
            str2 = str2.Substring(0, str2.Length - 4);
        }
        return str2;
    }

    public static int GetRand()
    {
        int num = _r.Next(70);
        int num4 = _randAry.Length - 2;
        for (int i = 0; i <= num4; i++)
        {
            if (num < _randAry[i])
            {
                return i;
            }
            num -= _randAry[i];
        }
        return (_randAry.Length - 1);
    }

    public static int[] GetRandAry(int n,  int topn=-1)
    {
        List<int> list = new List<int> {
            Capacity = n
        };
        int num4 = n - 1;
        for (int i = 0; i <= num4; i++)
        {
            list.Add(i);
        }
        Random random = new Random();
        if ((topn > 0) && (topn < n))
        {
            n = topn;
        }
        int[] numArray = new int[(n - 1) + 1];
        int num5 = n - 1;
        for (int j = 0; j <= num5; j++)
        {
            int index = random.Next(list.Count);
            numArray[j] = list[index];
            list.RemoveAt(index);
        }
        return numArray;
    }




    public static bool IsBot()
    {
        return IsBot(HttpContext.Current.Request.UserAgent);
    }

    public static bool IsBot(string b)
    {
        return IsSafeBot(b);
        //if (b == null)
        //{
        //    return true;
        //}
        //b = b.ToLower();
        //foreach (string str in botAry)
        //{
        //    if (b.Contains(str))
        //    {
        //        return true;
        //    }
        //}
        //return false;
    }

    public static bool IsBotQQ()
    {
        HttpRequest request = HttpContext.Current.Request;
        if (IsBot(request.UserAgent))
        {
            return true;
        }
        return (request.Cookies["QQbot"] != null);
    }

    public static bool IsSafeBot()
    {
        bool flag=false;
        HttpRequest request = HttpContext.Current.Request;
        if (IsSafeBot(request.UserAgent) || (request.QueryString["qqbot"] == "1"))
        {
            return true;
        }
        request = null;
        return flag;
    }

    public static bool IsSafeBot(string b)
    {
        if (b == null)
        {
            return true;
        }
        b = b.ToLower();
        foreach (string str in anQuanbotAry)
        {
            if (b.Contains(str))
            {
                return true;
            }
        }
        return false;
    }




}


}
