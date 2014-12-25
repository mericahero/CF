using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using COM.CF.Web;
using CWS;


namespace CFTL
{
    /// <summary>
    /// 功能：判断用户是否登录、返回登录用户的信息，同时实现ILoginUsr以及IUsr接口，全框架统一用户类
    /// 时间：2013-10-22
    /// 作者：meric
    /// </summary>
    public class LoginUsr : ILoginUsr, IUsr
    {
        
        private HttpContext Context;        
        private bool m_logined = false;
        /// <summary>
        /// 用户是否已登录
        /// </summary>
        public bool Logined
        {
            get
            {
                return m_logined = m_logined ? m_logined : CheckLogin();
            }
        }

        private string m_account;
        /// <summary>
        /// 用户账户
        /// </summary>
        public string Account
        {
            get
            {
                NotLoginError();
                return m_account;
            }
        }

        private string m_name;
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name
        {
            get
            {
                NotLoginError();
                return m_name;
            }
        }

        private int m_uid;
        /// <summary>
        /// 用户 UID
        /// </summary>
        public int UID
        {
            get
            {
                NotLoginError();
                return m_uid;
            }
        }

        private int m_bz;
        /// <summary>
        /// 用户标志位
        /// </summary>
        public int BZ
        {
            get
            {
                NotLoginError();
                return m_bz;
            }
        }

        private int m_idtype;
        /// <summary>
        /// 用户的身份，具体参照网站设计
        /// </summary>
        public int IDType
        {
            get
            {
                NotLoginError();
                return m_idtype;
            }
        }


        /// <summary>
        /// 当前的guid，先从URL中获取，URL中没有则从cookie中获得
        /// </summary>
        private Guid m_guid
        {
            get
            {
                var temp = Context.Request["_GUID_SIGN_"];
                var g = Guid.Empty;
                if (string.IsNullOrWhiteSpace(temp))
                {
                    g = CWPub.GetCookieGUID();
                }
                else
                {
                    if (!Guid.TryParse(temp, out g))
                    {
                        g = Guid.Empty;
                    }
                }
                return g;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public LoginUsr(HttpContext context)
        {
            Context = context;
        }
        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        /// <returns></returns>
        private bool CheckLogin()
        {
            if (m_guid == Guid.Empty)
            {
                m_logined = false;
                return false;
            }
            SqlCommand cm_login = new SqlCommand("p_session_cookies_login", CWConfig.SessionDB.GetConnection());
            try
            {
                cm_login.CommandType = CommandType.StoredProcedure;
                SqlParameterCollection cp = cm_login.Parameters;
                cp.Add("@guid", SqlDbType.UniqueIdentifier).Value = m_guid;
                cp.Add("@account", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cp.Add("@name", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cp.Add("@uid", SqlDbType.Int).Direction = ParameterDirection.Output;
                cp.Add("@bz", SqlDbType.Int).Direction = ParameterDirection.Output;
                cp.Add("@idtype", SqlDbType.Int).Direction = ParameterDirection.Output;
                cm_login.ExecuteNonQuery();
                if (Information.IsDBNull(RuntimeHelpers.GetObjectValue(cm_login.Parameters["@account"].Value)))
                {
                    return false;
                }
                m_account = cp["@account"].Value.ToString();
                m_name = cp["@name"].Value.ToString();
                m_uid = PubFunc.GetInt(cp["@uid"].Value.ToString());
                m_bz = PubFunc.GetInt(cp["@bz"].Value.ToString());
                m_idtype = PubFunc.GetInt(cp["@idtype"].Value.ToString());
            }
            finally
            {
                cm_login.Connection.Close();
            }
            m_logined = true;
            return true;
        }

        /// <summary>
        /// 强制页面必须登录
        /// </summary>
        public void MustLogin()
        {
            //如果请求方法是“HEAD”（非POST,GET），则当前返回结束
            CFCache.HEADEnd();
            if (!Logined)
            {
                NotLoginError();
            }
        }

        /// <summary>
        /// 如果没有登录，则返回未登录异常
        /// </summary>
        private void NotLoginError()
        {
            if (!m_logined)
            {
                throw new CFException(enErrType.NotLogined, "没有登陆");
            }
        }

    }





}
