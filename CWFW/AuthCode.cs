using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using COM.CF;
using System.Data.SqlClient;
using System.Data;

namespace CWS
{
    public class AuthCode
    {
        // Fields
        private static string[] ary = new string[] { 
        "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", 
        "i", "j", "k", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", 
        "z", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", 
        "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
     };

        // Methods
        public static bool Check(NameValueCollection form)
        {
            string c = CFConfig.GetStr(form, "code", 4);
            if (c != "")
            {
                c = c.Trim();
                long k = PubFunc.GetBigInt("0x" + form["key"]);
                SqlCommand cm = new SqlCommand("p_authcode_check", CWConfig.SessionDB.GetConnection());
                try
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.Add("@key", SqlDbType.BigInt).Value = k;
                    cm.Parameters.Add("@Code", SqlDbType.NVarChar, 4).Value = c;
                    cm.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                    cm.ExecuteNonQuery();
                    int i = (int)cm.Parameters["@RETURN_VALUE"].Value;
                    return (i == 1);
                }
                finally
                {
                    cm.Connection.Close();
                }
            }
            return false;
        }

        public static string CreateAuthCode(long key)
        {
            return CreateAuthCode(4, key);
        }

        public static string CreateAuthCode(int Length, long key)
        {
            if (Length <= 0)
            {
                return "";
            }
            string c = "";
            int i = -1;
            for (int j = 0; j <= Length - 1; j++)
            {
                i = CWPub.Rand.Next(0, ary.Length - 1);
                c = c + ary[i];
            }
            UpdateOrCreate(key, c);
            return c;
        }

        private static void UpdateOrCreate(long key, string code)
        {
            SqlCommand cm = new SqlCommand("p_authcode_updateorcreate", CWConfig.SessionDB.GetConnection());
            try
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.Add("@key", SqlDbType.BigInt).Value = key;
                cm.Parameters.Add("@Code", SqlDbType.NVarChar, 4).Value = code.ToLower();
                cm.ExecuteNonQuery();
            }
            finally
            {
                cm.Connection.Close();
            }
        }
    }



}
