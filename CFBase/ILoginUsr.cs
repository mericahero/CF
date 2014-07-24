using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF
{
    public interface ILoginUsr
    {
        bool Logined { get; }
        void MustLogin();
    }
}
