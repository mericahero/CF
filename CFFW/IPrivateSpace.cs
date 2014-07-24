using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF
{
    public interface IPrivateSpace
    {
        // Methods
        int AddFile(int filesize);
        int GetConfigSize();
        int GetFreeSize();
        string GetHomeDir();
        int ReFreshUsedFileSize();

        // Properties
        int MaxSingleFileSize { get; }
    }

 

}
