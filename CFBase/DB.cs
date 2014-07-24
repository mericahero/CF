using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web;

namespace COM.CF
{
    public class DB
    {
        private readonly string _constr;

        public DB(string constr)
        {
            _constr = constr;
        }

        public SqlConnection GetConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(_constr);
            sqlConnection.Open();
            return sqlConnection;
        }


        public int ExecuteNonQuery(string sql)
        {
            int num;
            SqlCommand sqlComman = new SqlCommand(sql, GetConnection());
            try
            {
                num = sqlComman.ExecuteNonQuery();
            }
            finally
            {
                sqlComman.Connection.Close();
            }
            return num;
        }

        public object ExecuteScalar(string sql)
        {
            object obj;
            SqlCommand sqlCommand = new SqlCommand(sql, GetConnection());
            try
            {
                obj = sqlCommand.ExecuteScalar();
            }
            finally
            {
                sqlCommand.Connection.Close();
            }
            return obj;
        }

        public static object ExeCuteScalar(string sql, SqlConnection oConn)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, oConn);
            return sqlCommand.ExecuteScalar();
        }

        public AutoAdapter GetAutoAdapter(string sql)
        {
            return new AutoAdapter(sql, this);
        }

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

        public SqlConnection GetNotOpenConnection()
        {
            return new SqlConnection(_constr);
        }

        public String GetRawSQLXML(string sql)
        {
            return GetRawSQLXML(sql, new StringBuilder()).ToString();
        }

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


        public DataSet GetSQLDataSet(string strsql)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        public DataRowCollection GetSQLRows(string strsql)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable.Rows;
        }

        public static DataRowCollection GetSQLRows(string strsql, SqlConnection oConn)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, oConn);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable.Rows;
        }


        public DataRowCollection GetSQLRows(string strsql, int n)
        {
            string[] str = new string[5];
            str[0] = "set rowcount ";
            str[1] = n.ToString();
            str[2] = " ";
            str[3] = strsql;
            str[4] = " set rowcount 0";

            return GetSQLRows(string.Concat(str));
        }

        public DataRowCollection GetSQLRows(string strsql, int start, int n)
        {
            string[] str = new string[5];
            str[0] = "set rowcount ";
            int num = start + n;
            str[1] = num.ToString();
            str[2] = " ";
            str[3] = strsql;
            str[4] = " set rowcount 0";
            strsql = string.Concat(str);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql,GetNotOpenConnection());
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet, start, n, "t");

            return dataSet.Tables[0].Rows; 
        }

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

        public DataTable GetSQLTab(string strsql)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strsql, GetNotOpenConnection());
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public string GetSQLXML(string sql)
        {
            return GetSQLXML(sql, new StringBuilder()).ToString();
        }

        public string GetSQLXML(string sql, string rowname)
        {
            return GetSQLXML(sql, new StringBuilder(), -1, rowname).ToString();
        }

        public string GetSQLXML(string sql, int n)
        {
            return GetSQLXML(sql, new StringBuilder(), n, "").ToString();
        }

        public StringBuilder GetSQLXML(string sql, StringBuilder s)
        {
            return GetSQLXML(sql, s, -1, "");
        }

        public StringBuilder GetSQLXML(string sql, StringBuilder s, int n, string rowname = "")
        {
            bool flag = String.Compare(rowname, "", false) != 0;
            if (flag)
            {
                rowname = string.Concat("('", rowname, "')");
            }
            flag = n != -1;
            if (!flag)
            {
                sql = string.Concat(sql, " for xml raw", rowname);
            }
            else
            {
                string[] str = new string[7];
                str[0] = " set rowcount ";
                str[1] = Convert.ToString(n);
                str[2] = " ";
                str[3] = sql;
                str[4] = " for xml raw";
                str[5] = rowname;
                str[6] = " set rowcount 0";
                sql = string.Concat(str);
            }

            return GetRawSQLXML(sql, s);
        }

        public string GetSQLXMLBig(string sql, string rowname)
        {
            return GetSQLXMLBig(sql, rowname, -1);
        }

        public string GetSQLXMLBig(string sql, string rowname, int n)
        {
            if (n!=-1)
            {
                string[] str = new string[5];
                str[0] = " set rowcount ";
                str[1] = Convert.ToString(n);
                str[2] = " ";
                str[3] = sql;
                str[4] = " set rowcount 0";
                sql = string.Concat(str);
            }
            StringBuilder stringBuilder = new StringBuilder();
            SqlCommand sqlCommand = new SqlCommand(sql, GetConnection());
            try
            {
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleResult);
                while (true)
                {
                     
                    if (!sqlDataReader.Read())
                    {
                        break;
                    }
                    stringBuilder.Append(string.Concat("<", rowname));
                    int fieldCount = sqlDataReader.FieldCount - 1;
                    int num = 0;
                    while (true)
                    {
                        int num1 = fieldCount;
                        if (num > num1)
                        {
                            break;
                        }
                        stringBuilder.Append(" ").Append(sqlDataReader.GetName(num)).Append("='");
                        stringBuilder.Append(HttpUtility.HtmlEncode(sqlDataReader[num].ToString()));
                        stringBuilder.Append("'");
                        num++;
                    }
                    stringBuilder.Append(" />");
                }
                sqlDataReader.Close();
            }
            finally
            {
                sqlCommand.Connection.Close();
            }
            return stringBuilder.ToString();
        }
    }
}
