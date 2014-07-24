using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF
{
    public interface IUsr
    {

        string Account
        {
            get;
        }

        string Name
        {
            get;
        }

        int IDType
        {
            get;
        }

        int BZ
        {
            get;
        }

        int UID
        {
            get;
        }
    }
}
