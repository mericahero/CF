using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;

namespace CCBWeb.Main.Web
{
    class MyException:CFException
    {
        public MyException(string msg):base(msg)
        {
            
        }

        public MyException(enErrType errType,string msg):base(errType,msg)
        {
        }


    }
}
