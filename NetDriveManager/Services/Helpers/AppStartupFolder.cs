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
            var strExeFile = Process.GetCurrentProcess()?.MainModule?.FileName;
            return strExeFile ?? throw new ArgumentNullException();
        }

        public static string GetStartupFolder()
        {
            var strWorkPath = Path.GetDirectoryName(GetProcessFullPath());                
            return strWorkPath ?? throw new ArgumentNullException();
        }
    }
}
