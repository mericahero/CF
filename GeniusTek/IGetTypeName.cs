using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GeniusTek
{
    public interface IGetTypeName
    {
        string GetTypeName(HttpRequest request);
    }
}
