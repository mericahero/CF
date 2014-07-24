using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using System.Configuration;

namespace CC.BL
{
    public static class CCDB
    {
        public static DB oDB;

        static CCDB()
        {
            oDB=new DB(ConfigurationManager.ConnectionStrings["CCConnectionString"].ConnectionString);
        }
    }
}
