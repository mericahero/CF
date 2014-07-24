using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFTL;
using COM.CF.Web;

namespace CCBWeb.Main.Web
{
    public class DeptListPage:CFPage
    {
        private FenYe GetFenYe(string sqls, string urls)
        {
            FenYe fy = new FenYe(RequestForm, "DeptID", sqls, 20, urls);
            fy.NotOpenConnection = CC.BL.CCDB.oDB.GetNotOpenConnection();
            return fy;
        }

        private FenYe _deptList;

        protected FenYe DeptList
        {
            get
            {
                if (_deptList == null) 
                {
                    string sql = "select * from tb_Dept where 1=1";
                    string url = "";
                    _deptList = GetFenYe(sql, url);
                }
                return _deptList;
            }
        }
    }
}
