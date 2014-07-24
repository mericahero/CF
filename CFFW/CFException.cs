using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF
{
    public class CFException: ApplicationException
    {
         private enErrType fieldErrType;


        public enErrType ErrType
        {
            get
            {
                enErrType _enErrType = fieldErrType;
                return _enErrType;
            }
        }

        public CFException(enErrType errType, string message) : base(message)
        {
            fieldErrType = errType;
        }

        public CFException(string message)
            : base(message)
        {
            fieldErrType = enErrType.NormalError;
        }
    }
}
