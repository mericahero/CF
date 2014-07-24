using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace COM.CF
{
    public interface IUpLoadFile
    {
        string FileExt
        { 
            get;
        }

        string FileNameWithoutExt
        {
            get;
        }

        int FileSize
        {
            get;
        }

        Stream InputStream
        {
            get;
        }
        void SaveAs(string filename);
    }
}
