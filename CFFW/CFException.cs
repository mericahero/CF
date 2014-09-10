using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF
{
    /// <summary>
    /// 功能:CF框架异常
    /// 时间:2013-10-21
    /// 作者:meric
    /// </summary>
    public class CFException: ApplicationException
    {
        private enErrType m_errortype;
        public enErrType ErrType
        {
            get
            {
                return m_errortype;
            }
        }

        public CFException(enErrType errType, string message) 
            : base(message)
        {
            m_errortype = errType;
        }

        public CFException(string message)
            : this(enErrType.NormalError,message)
        {
        }
    }
}
