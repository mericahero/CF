using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;

namespace CWS
{
    public class CWException : CFException
    {
        public CWException(string message):base(message){ }

        public CWException(enErrType errType, string message) : base(errType, message) { }



    }
}
