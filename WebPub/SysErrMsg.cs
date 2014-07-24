using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CWS;
using System.Collections;
using System.Data;
using COM.CF;

namespace CFTL
{
    public class SysErrMsg
    {
        private static Hashtable ErrMsg
        {
            get
            {
                var o = CWConfig.XHDB.GetSQLRows("select * from error_sys");
                if (o != null)
                {
                    var _errmsg = new Hashtable();
                    foreach (DataRow row in o)
                    {
                        _errmsg.Add(PubFunc.GetInt(row["id"].ToString()), row["msg"].ToString());
                    }
                    return _errmsg;
                }
                return null;
            }
        }


        public static String GetErrMsg(Int32 errId)
        {
            if (errId > 0)
            {
                return errId.ToString();
            }
            if (ErrMsg.ContainsKey(errId))
            {
                return ErrMsg[errId].ToString();
            }
            return errId.ToString();
        }

        public static String GetErrMsgXML(Int32 errId)
        {
            if (errId > 0) return errId.ToString();
            if (errId == -999) return "-999";
            if (ErrMsg.ContainsKey(errId))
            {
                return ErrMsg[errId].ToString();
            }
            return errId.ToString();
        }
    }
}
