using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;
using System.Collections;

namespace COM.CF
{
    public class AutoAdapter
    {
        private SqlDataAdapter ad;

        private DataTable dt;

        public DataRow Row
        {
            get{return dt.Rows[0];}
        }

        public DataRowCollection Rows
        {
            get { return dt.Rows; }
        }

        internal AutoAdapter(string sql, DB oDB)
        {
            ad = new SqlDataAdapter(sql, oDB.GetNotOpenConnection());
            dt = new DataTable();
            ad.Fill(dt);
        }

        public DataRow AddNewRow()
        {
            DataRow dataRow = dt.NewRow();
            dt.Rows.InsertAt(dataRow, 0);
            return dataRow;
        }


        public void Update()
        {
            ad.Update(dt);
        }
    }
}
