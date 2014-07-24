using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace CC.BL
{
    public class Dept
    {
        public static void Add(NameValueCollection form)
        {
            SqlCommand cm = CCDB.oDB.GetConnection().CreateCommand();
            try
            {
                cm.CommandText = "Insert Into tb_Dept([DeptName],[DeptDirector],[DeptTel],[ParentIndex]) values(@DeptName,@DeptDirector,@DeptTel,@ParentIndex)";
                cm.Parameters.Add("@DeptName", System.Data.SqlDbType.NVarChar, 50).Value = form["name"];
                cm.Parameters.Add("@DeptDirector", System.Data.SqlDbType.NVarChar, 50).Value = form["dir"];
                cm.Parameters.Add("@DeptTel", System.Data.SqlDbType.NVarChar, 50).Value = form["tel"];
                cm.Parameters.Add("@ParentIndex", System.Data.SqlDbType.Int).Value = form["index"];
                cm.ExecuteNonQuery();
            }
            finally
            {
                cm.Connection.Close();
            }
        }
    }
}
