using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF.Web
{
    public class BodyParams
    {
        public readonly static string[] AryBodyParams;

        static BodyParams()
        {
            string[] strArrays = new string[3];
            strArrays[0] = "bgcolor=#F5FAF9";
            strArrays[1] = "bgcolor=#D5E6E1";
            strArrays[2] = "bgcolor=#F5FAF9";
            AryBodyParams = strArrays;
        }
    }
}
