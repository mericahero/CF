using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.CF
{
    public interface ISpace
    {
        // Methods
        int AddFile(int filesize);
        void Auth(IUsr usr);
        bool AuthNoExcept(IUsr usr);
        bool CheckFileType(string filename);
        string GetAutoLink();
        int GetConfigSize();
        int GetFreeSize();
        string GetHomeDir();
        string GetHomeUrl();
        IPrivateSpace GetPrivateObject();
        int ReFreshUsedFileSize();

        // Properties
        bool HavePrivateDir { get; }
        bool HaveSysDir { get; }
        byte HomeID { get; }
        bool IsFree { get; }
        int MaxSingleFileSize { get; }
    }

 

}
