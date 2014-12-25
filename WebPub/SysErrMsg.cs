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
    /// <summary>
    /// 功能：CF框架的系统错误信息类
    /// 时间：2014-10-1
    /// 作者：陈辰
    /// </summary>
    public class SysErrMsg
    {
        /// <summary>
        /// 从数据库中加载错误代码和错误信息的对应关系
        /// </summary>
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

        /// <summary>
        /// 根据错误代码返回错误信息
        /// </summary>
        /// <param name="errId">错误代码</param>
        /// <returns>返回错误代码对应的错误信息，找不到对应的错误信息则返回错误代码</returns>
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
        /// <summary>
        /// 根据错误代码返回错误信息，以XML形式返回
        /// </summary>
        /// <param name="errId">错误代码</param>
        /// <returns>返回错误代码对应的错误信息，找不到对应的错误信息则返回错误代码</returns>
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
