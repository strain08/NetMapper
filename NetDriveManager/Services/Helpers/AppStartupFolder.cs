using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Services.Helpers
{
    public class AppStartupFolder
    {
        public static string GetProcessFullPath()
        {
            var strExeFile = Process.GetCurrentProcess()?.MainModule?.FileName
                ?? throw new ApplicationException("Process.GetCurrentProcess()?.MainModule?.FileName null");
            return strExeFile;
        }

        public static string GetStartupFolder()
        {
            var strExeFile = GetProcessFullPath();

            var strWorkPath = Path.GetDirectoryName(strExeFile)
                ?? throw new ApplicationException("Path.GetDirectoryName(strExeFilePath) null");
            return strWorkPath;
        }
    }
}
