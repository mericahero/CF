using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF
{
    public enum enXMLErrorCode
    {
        PasswdInvalid = 1,
        
        CFError = 100,
        
        SystemError = 110,
        
        SystemBusy = 120,
        
        FreeExpires = 140,
        
        UnKonwError = 150,
        
        DNSNotRealIP = 200,
        
        IMGInvalidImgType = 300,
        
        IMGNoUploadFile = 301,
        
        IMGDupImg = 302,
        
        IMGCanShuError = 303
    }
}
