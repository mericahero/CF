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
    /// <summary>
    /// 功能：封装CF的Adapter
    /// 时间：2014-10-3
    /// 作者：meric
    /// </summary>
    public class AutoAdapter
    {
        //SqlDataAdapter
        private SqlDataAdapter ad;
        //当前DataTable
        private DataTable dt;
        /// <summary>
        /// 当前数据行（单条）
        /// </summary>
        public DataRow Row
        {
            get{return dt.Rows[0];}
        }
        /// <summary>
        /// 当前数据行（多条）
        /// </summary>
        public DataRowCollection Rows
        {
            get { return dt.Rows; }
        }
        /// <summary>
        /// 构造函数，初始化SqlAdapter和DataTable
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="oDB">DB对象的一个实例</param>
        public AutoAdapter(string sql, DB oDB)
        {
            ad = new SqlDataAdapter(sql, oDB.GetNotOpenConnection());
            dt = new DataTable();
            ad.Fill(dt);
        }
        /// <summary>
        /// 在数据表的第一行插入一条数据，并返回该数据
        /// </summary>
        /// <returns>返回插入到数据表的数据</returns>
        public DataRow AddNewRow()
        {
            DataRow dataRow = dt.NewRow();
            dt.Rows.InsertAt(dataRow, 0);
            return dataRow;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            ad.Update(dt);
        }
    }
}
