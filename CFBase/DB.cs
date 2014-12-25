using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web;

namespace COM.CF
{
    /// <summary>
    /// 功能：封装Sql操作类
    /// 时间：2013-10-2
    /// 作者：Meric
    /// </summary>
    public class DB
    {
        private readonly string _constr;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="constr">连接字符串</param>
        public DB(string constr)
        {
            _constr = constr;
        }

        /// <summary>
        /// 获得打开的连接
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(_constr);
            sqlConnection.Open();
            return sqlConnection;
        }

        /// <summary>
        /// 执行sql语句，返回影响条数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pars">传递给sql语句的参数</param>
        /// <returns>返回受影响的条数</returns>
        public int ExecuteNonQuery(string sql,params SqlParameter [] pars)
        {
            SqlCommand sqlComman = new SqlCommand(sql, GetConnection());
            try
            {
                sqlComman.Parameters.AddRange(pars);
                return sqlComman.ExecuteNonQuery();
            }
            finally
            {
                sqlComman.Connection.Close();
            }
        }


        /// <summary>
        /// 执行sql语句，返回查询结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回查询反返回结果的第一行的第一列，忽略其它行和列</returns>
        public object ExecuteScalar(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, GetConnection());
            try
            {
                return sqlCommand.ExecuteScalar();
            }
            finally
            {
                sqlCommand.Connection.Close();
            }
        }

        /// <summary>
        /// 执行sql语句，返回查询结果，该方法需要传入连接
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="oConn">已打开的数据库连接</param>
        /// <returns>返回查询反返回结果的第一行的第一列，忽略其它行和列</returns>
        public static object ExeCuteScalar(string sql, SqlConnection oConn)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, oConn);
            return sqlCommand.ExecuteScalar();
        }
        /// <summary>
        /// 得到一个AutoAdapter的实例
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回一个AutoAdapter的实例</returns>
        public AutoAdapter GetAutoAdapter(string sql)
        {
            return new AutoAdapter(sql, this);
        }
        /// <summary>
        /// 获得。。。。
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static StringBuilder GetMoreSQLXML(SqlCommand cm, StringBuilder s)
        {
            try
            {
                XmlReader xmlReader = cm.ExecuteXmlReader();
                xmlReader.Read();
                while (true)
                {
                    if (xmlReader.EOF)
                    {
                        break;
                    }
                    s.Append(xmlReader.ReadOuterXml());
                }
                xmlReader.Close();
            }
            finally
            {
                cm.Connection.Close();
            }
            return s;
        }

        /// <summary>
        /// 获得一个没有打开的数据库连接
        /// </summary>
        /// <returns>返回一个没有打开的数据库连接</returns>
        public SqlConnection GetNotOpenConnection()
        {
            return new SqlConnection(_constr);
        }

        /// <summary>
        /// 获得。。。。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public String GetRawSQLXML(string sql)
        {
            return GetRawSQLXML(sql, new StringBuilder()).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public StringBuilder GetRawSQLXML(string sql, StringBuilder s)
        {
            SqlCommand cm = new SqlCommand(sql, GetConnection());
            try
            {
                XmlReader xmlReader = cm.ExecuteXmlReader();
                xmlReader.Read();
                while (true)
                {
                    if (xmlReader.EOF) break;
                    s.Append(xmlReader.ReadOuterXml());
                }
                xmlReader.Close();
            }
            finally
            {
                cm.Connection.Close();
            }
            return s;
        }
        /// <summary>
        /// 获得。。。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public String GetSingleSQLXML(string sql)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand cm = new SqlCommand(sql, GetConnection());
            try
            {
                SqlDataReader sqlDataReader = cm.ExecuteReader(CommandBehavior.SingleRow);
                if (sqlDataReader.Read())
                {
                    int fieldCount = sqlDataReader.FieldCount - 1;
                    int num = 0;
                    while (true)
                    {
                        if (num > fieldCount) break;                  
                        sb.Append(" ").Append(sqlDataReader.GetName(num)).Append("=");
                        sb.Append(HttpUtility.HtmlEncode(sqlDataReader[num].ToString()));
                        sb.Append("'");
                        num++;
                    }
                }
                sqlDataReader.Close();
            }
            finally
            {
                cm.Connection.Close();
            }
            return sb.ToString();
        }


        /// <summary>
        /// 根据sql获得DataSet
        /// </summary>
        /// <param name="strsql">sql</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetSQLDataSet(string strsql)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// 通过存储过程返回DataSet
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="sqlparams">参数</param>
        /// <returns></returns>
        public DataSet GetSQLDataSet(string procedureName,params SqlParameter[] sqlparams)
        {
            var cm = new SqlCommand(procedureName,GetNotOpenConnection());
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddRange(sqlparams);
            var adpter = new SqlDataAdapter(cm);
            var ds = new DataSet();
            adpter.Fill(ds);
            return ds;

        }
        /// <summary>
        /// 根据sql获得DataRow集合
        /// </summary>
        /// <param name="strsql">sql</param>
        /// <returns>返回DataRow集合</returns>
        public DataRowCollection GetSQLRows(string strsql)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable.Rows;
        }
        /// <summary>
        /// 传入数据库连接，根据sql获得DataRow集合
        /// </summary>
        /// <param name="strsql">sql</param>
        /// <param name="oConn">数据库连接 无须打开</param>
        /// <returns>返回DataRow集合</returns>
        public static DataRowCollection GetSQLRows(string strsql, SqlConnection oConn)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, oConn);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable.Rows;
        }

        /// <summary>
        /// 根据sql语句返回指定行数的数据
        /// </summary>
        /// <param name="strsql">sql语句</param>
        /// <param name="n">需要的数据行数</param>
        /// <returns>返回指定行数的DataRow</returns>
        public DataRowCollection GetSQLRows(string strsql, int n)
        {
            string str = string.Format("set rowcount{0} {1} set rowcount 0",n,strsql);            
            return GetSQLRows(str);
        }

        /// <summary>
        /// 根据sql语句返回从指定行开始的需要的行数的数据
        /// </summary>
        /// <param name="strsql">sql语句</param>
        /// <param name="start">开始的行数</param>
        /// <param name="n">需要获取的行数</param>
        /// <returns>返回指定行数的DataRow</returns>
        public DataRowCollection GetSQLRows(string strsql, int start, int n)
        {
            strsql = string.Format("set rowcount {0} {1} set rowcount 0",start+n,strsql);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql,GetNotOpenConnection());
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet, start, n, "t");

            return dataSet.Tables[0].Rows; 
        }

        /// <summary>
        /// 根据sql获得单条数据
        /// </summary>
        /// <param name="strsql">sql语句</param>
        /// <returns>返回指定的数据</returns>
        public DataRow GetSQLSingleRow(string strsql)
        {
            DataRow item;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
            DataTable dataTable = new DataTable();
            if (sqlDataAdapter.Fill(dataTable) != 0)
            {
                item = dataTable.Rows[0];
            }
            else
            {
                item = null;
            }
            return item;
        }
        /// <summary>
        /// 传入数据库连接 根据sql获得单条数据
        /// </summary>
        /// <param name="strsql">sql</param>
        /// <param name="oConn">数据库连接</param>
        /// <returns>条例sql检索的音箱数据</returns>
        public static DataRow GetSQLSingleRow(string strsql, SqlConnection oConn)
        {
            DataRow item;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, oConn);
            DataTable dataTable = new DataTable();
            if (sqlDataAdapter.Fill(dataTable) != 0)
            {
                item = dataTable.Rows[0];
            }
            else
            {
                item = null;
            }
            return item;
        }

        /// <summary>
        /// 根据sql语句获得DataTable
        /// </summary>
        /// <param name="strsql">sql</param>
        /// <returns>返回符合查询条件的DataTable</returns>
        public DataTable GetSQLTab(string strsql)
        {
            var sqlDataAdapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        /// <summary>
        /// 通过存储过程获得DataTable
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="param">存储过程参数</param>
        /// <returns></returns>
        public DataTable GetSQLTab(string procedureName, params SqlParameter[] param)
        {
            var cm = new SqlCommand(procedureName, GetConnection());
            try
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddRange(param);
                var adapter = new SqlDataAdapter(cm);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            finally
            {
                cm.Connection.Close();
            }
        }
    }
}
