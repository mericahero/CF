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
    public class LoginUsr : ILoginUsr, IUsr
{
    // Fields
    private HttpContext Context;
    private bool m_checked = false;
    private bool m_logined = false;
    private string m_account;
    private string m_name;
    private int m_uid;
    private int m_bz;
    private int m_idtype;

    // Methods
    public LoginUsr(HttpContext context)
    {
        Context = context;
    }

    private bool CheckLogin()
    {

        m_checked = true;
        Guid guid1 = CWPub.GetCookieGUID();
        if (guid1 == Guid.Empty)
        {
            m_logined = false;
            return false;
        }
        SqlCommand cm_login = new SqlCommand("p_session_cookies_login", CWConfig.SessionDB.GetConnection());
        try
        {
            cm_login.CommandType = CommandType.StoredProcedure;
            SqlParameterCollection cp = cm_login.Parameters;
            cp.Add("@guid", SqlDbType.UniqueIdentifier).Value = guid1;
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
            m_name =  cp["@name"].Value.ToString();
            m_uid =PubFunc.GetInt( cp["@uid"].Value.ToString());
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

    public void MustLogin()
    {
        CFCache.HEADEnd();
        if (!Logined)
        {
            NotLoginError();
        }
    }

    private void NotLoginError()
    {
        if (!m_logined)
        {
            throw new CFException(enErrType.NotLogined, "没有登陆");
        }
    }

    // Properties
    public bool Logined
    {
        get
        {
            if (!m_checked)
            {
                m_logined = CheckLogin();
            }
            return m_logined;
        }
    }

    public string Account
    {
        get
        {
            NotLoginError();
            return m_account;
        }
    }

    public string Name
    {
        get
        {
            NotLoginError();
            return m_name;
        }
    }


    public int UID
    {
        get
        {
            NotLoginError();
            return m_uid;
        }
    }


    public int BZ
    {
        get
        {
            NotLoginError();
            return m_bz;
        }
    }

    public int IDType
    {
        get
        {
            NotLoginError();
            return m_idtype;
        }
    }
}


 


}
